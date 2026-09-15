using System.Collections;
using System.Collections.Generic;
using Cloudora.Services;
using Cloudora.UI;
using Cloudora.Level;
using Cloudora.Progression;
using Cloudora.Modifiers;
using UnityEngine;

namespace Cloudora.Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        [Header("Puzzle References")]
        [SerializeField] private Transform boardRoot;
        [SerializeField] private CloudContainerView cloudPrefab;

        [Header("Level Debug")]
        [SerializeField, Min(1)] private int debugStartLevel = 1;
        private readonly List<CloudContainerView> _containers = new();
        private readonly Stack<WeatherType[][]> _undoHistory = new();
        private readonly HashSet<CloudContainerView> _celebratedSolvedClouds = new();
        private CloudContainerView _selectedCloud;
        private MoveAnimator _moveAnimator;
        private GameFeedbackService _feedback;
        private GameplayOverlay _overlay;
        private AdaptiveBoardLayout _boardLayout;
        private WorldScreenController _worldScreen;
        private ProgressionManager _progression;
        private ModifierRuntime _modifierRuntime;
        private bool _isCompleted;
        private bool _inputLocked;
        private LevelDefinition _currentLevel;

        private void Start()
        {
            _moveAnimator = gameObject.AddComponent<MoveAnimator>();
            _feedback = gameObject.AddComponent<GameFeedbackService>();
            Canvas canvas = boardRoot.GetComponentInParent<Canvas>();
            _progression = new ProgressionManager(debugStartLevel);
            _modifierRuntime = new ModifierRuntime();
            _worldScreen = WorldScreenController.Create(canvas);
            _overlay = GameplayOverlay.Create(canvas, RestartLevel, Undo, HandleContinue, _worldScreen.Toggle);
            _boardLayout = boardRoot.GetComponent<AdaptiveBoardLayout>();
            if (_boardLayout == null) _boardLayout = boardRoot.gameObject.AddComponent<AdaptiveBoardLayout>();
            LoadLevel(_progression.CurrentLevel);
        }

        public void RestartLevel()
        {
            if (_inputLocked)
            {
                return;
            }

            StopAllCoroutines();
            LoadState(_currentLevel.CreateBoard());
        }

        public void LoadLevel(int levelNumber)
        {
            _currentLevel = levelNumber <= AuthoredLevelCatalog.Count
                ? AuthoredLevelCatalog.Get(levelNumber)
                : LevelGenerator.Generate(levelNumber);
            LoadState(_currentLevel.CreateBoard());
            _overlay.SetLevelInfo(_currentLevel.levelId, _currentLevel.worldId);
            _overlay.SetTutorialCue(_currentLevel.tutorialCue);
            _boardLayout.Configure(_currentLevel.clouds.Length, _currentLevel.capacity);
            _worldScreen.Refresh(_progression.CurrentWorld, _progression.HighestCompletedLevel);
        }

        [ContextMenu("Load Debug Level")]
        private void LoadDebugLevel()
        {
            if (Application.isPlaying)
            {
                LoadLevel(debugStartLevel);
            }
        }

        public void Undo()
        {
            if (_inputLocked || _undoHistory.Count == 0)
            {
                return;
            }

            RestoreSnapshot(_undoHistory.Pop());
            _isCompleted = false;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(_undoHistory.Count > 0);
        }

        private void LoadState(WeatherType[][] levelData)
        {
            ClearSelection();
            foreach (CloudContainerView container in _containers)
            {
                Destroy(container.gameObject);
            }

            _containers.Clear();
            _undoHistory.Clear();
            _celebratedSolvedClouds.Clear();
            _isCompleted = false;
            _inputLocked = false;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(false);

            for (int i = 0; i < levelData.Length; i++)
            {
                CloudContainerView cloud = Instantiate(cloudPrefab, boardRoot);
                cloud.name = $"Cloud_{i + 1}";
                cloud.Initialize(_currentLevel.capacity, levelData[i], HandleCloudClicked);
                _containers.Add(cloud);
            }
            _modifierRuntime.Initialize(_currentLevel, _containers.ToArray());
        }

        private void HandleCloudClicked(CloudContainerView clickedCloud)
        {
            if (_isCompleted || _inputLocked)
            {
                return;
            }

            if (_selectedCloud == null)
            {
                if (!clickedCloud.IsEmpty)
                {
                    SelectCloud(clickedCloud);
                }
                return;
            }

            if (_selectedCloud == clickedCloud)
            {
                ClearSelection();
                return;
            }

            int sourceIndex = _containers.IndexOf(_selectedCloud);
            int targetIndex = _containers.IndexOf(clickedCloud);
            if (!_modifierRuntime.CanMove(sourceIndex, targetIndex))
            {
                clickedCloud.PlayInvalidFeedback();
                _feedback.Invalid();
                return;
            }

            if (_selectedCloud.TryGetTopElement(out WeatherType type) && clickedCloud.CanReceive(type))
            {
                int count = Mathf.Min(_selectedCloud.TopGroupCount, clickedCloud.Capacity - clickedCloud.ElementCount);
                StartCoroutine(PerformMove(_selectedCloud, clickedCloud, type, count));
                return;
            }

            clickedCloud.PlayInvalidFeedback();
            _feedback.Invalid();
            if (!clickedCloud.IsEmpty)
            {
                SelectCloud(clickedCloud);
            }
        }

        private IEnumerator PerformMove(CloudContainerView source, CloudContainerView target, WeatherType type, int count)
        {
            _inputLocked = true;
            WeatherType[][] beforeMove = CaptureSnapshot();
            yield return _moveAnimator.Animate(source, target, type, count);

            if (source.TryMoveTopGroupTo(target))
            {
                _undoHistory.Push(beforeMove);
                _feedback.Move();
                _modifierRuntime.OnSuccessfulMove(_containers.IndexOf(source), _containers.IndexOf(target));
                _overlay.SetUndoAvailable(true);
                CelebrateNewlySolvedClouds();
                CheckWin();
            }

            ClearSelection();
            _inputLocked = false;
        }

        private void CelebrateNewlySolvedClouds()
        {
            foreach (CloudContainerView cloud in _containers)
            {
                if (!cloud.IsEmpty && cloud.IsSolved() && _celebratedSolvedClouds.Add(cloud))
                {
                    cloud.PlaySolvedFeedback();
                    _feedback.Solved();
                }
            }
        }

        private void SelectCloud(CloudContainerView cloud)
        {
            if (_selectedCloud != null)
            {
                _selectedCloud.SetSelected(false);
            }
            _selectedCloud = cloud;
            _selectedCloud.SetSelected(true);
            _feedback.Select();
        }

        private void ClearSelection()
        {
            if (_selectedCloud != null)
            {
                _selectedCloud.SetSelected(false);
            }
            _selectedCloud = null;
        }

        private WeatherType[][] CaptureSnapshot()
        {
            var snapshot = new WeatherType[_containers.Count][];
            for (int i = 0; i < _containers.Count; i++)
            {
                snapshot[i] = _containers[i].CaptureElements();
            }
            return snapshot;
        }

        private void RestoreSnapshot(WeatherType[][] snapshot)
        {
            ClearSelection();
            _celebratedSolvedClouds.Clear();
            for (int i = 0; i < snapshot.Length && i < _containers.Count; i++)
            {
                _containers[i].RestoreElements(snapshot[i]);
            }
            CelebrateNewlySolvedClouds();
        }

        private void CheckWin()
        {
            foreach (CloudContainerView container in _containers)
            {
                if (!container.IsSolved())
                {
                    return;
                }
            }

            _isCompleted = true;
            _progression.CompleteCurrentLevel();
            _worldScreen.Refresh(_progression.CurrentWorld, _progression.HighestCompletedLevel);
            _overlay.ShowComplete(true);
            _feedback.Complete();
            Debug.Log("Level completed!");
        }

        private void HandleContinue()
        {
            _progression.Advance();
            LoadLevel(_progression.CurrentLevel);
        }
    }
}
