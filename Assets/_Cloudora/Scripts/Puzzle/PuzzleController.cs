using System.Collections;
using System.Collections.Generic;
using Cloudora.Services;
using Cloudora.UI;
using Cloudora.Level;
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
        private bool _isCompleted;
        private bool _inputLocked;
        private LevelDefinition _currentLevel;

        private void Start()
        {
            _moveAnimator = gameObject.AddComponent<MoveAnimator>();
            _feedback = gameObject.AddComponent<GameFeedbackService>();
            Canvas canvas = boardRoot.GetComponentInParent<Canvas>();
            _overlay = GameplayOverlay.Create(canvas, RestartLevel, Undo, HandleContinue);
            LoadLevel(debugStartLevel);
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
            _currentLevel = AuthoredLevelCatalog.Get(levelNumber);
            LoadState(_currentLevel.CreateBoard());
            _overlay.SetLevelInfo(_currentLevel.levelId, _currentLevel.worldId);
            _overlay.SetTutorialCue(_currentLevel.tutorialCue);
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
            _overlay.ShowComplete(true);
            _feedback.Complete();
            Debug.Log("Level completed!");
        }

        private void HandleContinue()
        {
            LoadLevel(Mathf.Min(_currentLevel.levelId + 1, AuthoredLevelCatalog.Count));
        }
    }
}
