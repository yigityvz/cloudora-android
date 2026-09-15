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
        [SerializeField] private bool useDebugStartLevel;
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
        private LifeManager _lifeManager;
        private BoosterManager _boosterManager;
        private int _moveCount;
        private int _shuffleUseCount;
        private ISaveService _saveService;
        private SaveData _saveData;
        private IAnalyticsService _analytics;
        private float _levelStartedAt;
        private bool _isCompleted;
        private bool _inputLocked;
        private LevelDefinition _currentLevel;

        private void Start()
        {
            _moveAnimator = gameObject.AddComponent<MoveAnimator>();
            _feedback = gameObject.AddComponent<GameFeedbackService>();
            Canvas canvas = boardRoot.GetComponentInParent<Canvas>();
            _saveService = new LocalJsonSaveService();
            _saveData = _saveService.Load();
            _analytics = new FirebaseAnalyticsService();
            _analytics.Track(AnalyticsEvents.GameStarted, Params("save_schema", _saveData.schemaVersion));
            int startLevel = useDebugStartLevel ? debugStartLevel : _saveData.currentLevel;
            _progression = new ProgressionManager(startLevel, _saveData.highestCompletedLevel);
            _modifierRuntime = new ModifierRuntime();
            System.DateTime nextLife = _saveData.nextLifeUtcTicks > 0 ? new System.DateTime(_saveData.nextLifeUtcTicks, System.DateTimeKind.Utc) : default;
            _lifeManager = new LifeManager(_saveData.lives, nextLife);
            _boosterManager = new BoosterManager(_saveData.undoCharges, _saveData.extraCloudCharges, _saveData.safeShuffleCharges);
            _feedback.SoundEnabled = _saveData.soundEnabled;
            _feedback.HapticsEnabled = _saveData.hapticsEnabled;
            _worldScreen = WorldScreenController.Create(canvas);
            _overlay = GameplayOverlay.Create(canvas, RestartLevel, Undo, HandleContinue, _worldScreen.Toggle, UseExtraCloud, UseSafeShuffle);
            _boardLayout = boardRoot.GetComponent<AdaptiveBoardLayout>();
            if (_boardLayout == null) _boardLayout = boardRoot.gameObject.AddComponent<AdaptiveBoardLayout>();
            LoadLevel(_progression.CurrentLevel);
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused) SaveProgress();
        }

        private void OnApplicationQuit() => SaveProgress();

        private void Update()
        {
            if (_overlay == null) return;
            System.DateTime now = System.DateTime.UtcNow;
            _lifeManager.Refresh(now);
            System.TimeSpan remaining = _lifeManager.TimeUntilNext(now);
            string countdown = remaining == System.TimeSpan.Zero ? string.Empty : $"{(int)remaining.TotalMinutes:00}:{remaining.Seconds:00}";
            _overlay.SetLives(_lifeManager.Lives, countdown);
        }

        public void RestartLevel()
        {
            if (_inputLocked)
            {
                return;
            }

            if (_moveCount > 0 && !_lifeManager.TryConsumeRetry(System.DateTime.UtcNow, _currentLevel.levelId <= 5))
            {
                _overlay.ShowBlock("Out of Lives", "A life returns every 30 minutes. Reward hooks are available.");
                return;
            }

            StopAllCoroutines();
            LoadState(_currentLevel.CreateBoard());
            _analytics.Track(AnalyticsEvents.LevelRestarted, LevelParams());
            SaveProgress();
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
            _levelStartedAt = Time.realtimeSinceStartup;
            _analytics.Track(AnalyticsEvents.LevelStarted, LevelParams());
            if (_currentLevel.modifiers != null)
                foreach (ModifierData modifier in _currentLevel.modifiers)
                    _analytics.Track(AnalyticsEvents.ModifierEncountered, Params("type", modifier.type.ToString(), "level", _currentLevel.levelId));
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

            if (!_boosterManager.TryUseUndo())
            {
                _overlay.ShowBlock("No Undo Charges", "Reward hook: +3 Undo");
                return;
            }

            RestoreSnapshot(_undoHistory.Pop());
            _isCompleted = false;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(_undoHistory.Count > 0);
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.UndoUsed, Params("level", _currentLevel.levelId, "remaining", _boosterManager.UndoCharges));
            SaveProgress();
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
            _moveCount = 0;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(false);
            _overlay.HideBlock();
            RefreshMetaUI();

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
                _moveCount++;
                _feedback.Move();
                _modifierRuntime.OnSuccessfulMove(_containers.IndexOf(source), _containers.IndexOf(target));
                _overlay.SetUndoAvailable(true);
                CelebrateNewlySolvedClouds();
                CheckWin();
                if (!_isCompleted && !HasLegalMove())
                {
                    _analytics.Track(AnalyticsEvents.LevelFailed, Params("level", _currentLevel.levelId, "reason", "no_moves", "move_count", _moveCount));
                    _overlay.ShowBlock("No Moves", "Use Undo, Extra Cloud, or Safe Shuffle");
                }
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
            if (_currentLevel.levelId <= 15) _saveData.tutorialFlags[_currentLevel.levelId - 1] = true;
            _worldScreen.Refresh(_progression.CurrentWorld, _progression.HighestCompletedLevel);
            _overlay.ShowComplete(true);
            _feedback.Complete();
            _analytics.Track(AnalyticsEvents.LevelCompleted, Params("level", _currentLevel.levelId, "duration_seconds", Time.realtimeSinceStartup - _levelStartedAt, "move_count", _moveCount, "seed", _currentLevel.seed));
            if (_currentLevel.levelId == _progression.CurrentWorld.LastLevel)
                _analytics.Track(AnalyticsEvents.WorldCompleted, Params("world", _progression.CurrentWorld.Id));
            Debug.Log("Level completed!");
            SaveProgress();
        }

        private void HandleContinue()
        {
            _progression.Advance();
            LoadLevel(_progression.CurrentLevel);
            SaveProgress();
        }

        private void UseExtraCloud()
        {
            if (_inputLocked || !_boosterManager.TryUseExtraCloud())
            {
                _overlay.ShowBlock("Extra Cloud Unavailable", "Reward hook: Extra Cloud for this level");
                return;
            }

            CloudContainerView cloud = Instantiate(cloudPrefab, boardRoot);
            cloud.name = $"Cloud_{_containers.Count + 1}_Extra";
            cloud.Initialize(_currentLevel.capacity, System.Array.Empty<WeatherType>(), HandleCloudClicked);
            _containers.Add(cloud);
            _undoHistory.Clear();
            _boardLayout.Configure(_containers.Count, _currentLevel.capacity);
            _modifierRuntime.Initialize(_currentLevel, _containers.ToArray());
            _overlay.HideBlock();
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.ExtraCloudUsed, LevelParams());
            SaveProgress();
        }

        private void UseSafeShuffle()
        {
            if (_inputLocked || !_boosterManager.TryUseSafeShuffle())
            {
                _overlay.ShowBlock("Shuffle Unavailable", "Reward hook: solvability-preserving shuffle");
                return;
            }

            if (_currentLevel.generated)
                _currentLevel = LevelGenerator.Generate(_currentLevel.levelId, _currentLevel.seed + (++_shuffleUseCount * 997));
            LoadState(_currentLevel.CreateBoard());
            _overlay.HideBlock();
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.SafeShuffleUsed, LevelParams());
            SaveProgress();
        }

        private bool HasLegalMove()
        {
            for (int source = 0; source < _containers.Count; source++)
            {
                if (!_containers[source].TryGetTopElement(out WeatherType type)) continue;
                for (int target = 0; target < _containers.Count; target++)
                    if (source != target && _modifierRuntime.CanMove(source, target) && _containers[target].CanReceive(type)) return true;
            }
            return false;
        }

        private void RefreshMetaUI()
        {
            if (_overlay != null)
                _overlay.SetBoosters(_boosterManager.UndoCharges, _boosterManager.ExtraCloudCharges, _boosterManager.SafeShuffleCharges);
        }

        private void SaveProgress()
        {
            if (_saveService == null || _saveData == null || _progression == null) return;
            _saveData.currentLevel = _progression.CurrentLevel;
            _saveData.highestCompletedLevel = _progression.HighestCompletedLevel;
            _saveData.lives = _lifeManager.Lives;
            _saveData.nextLifeUtcTicks = _lifeManager.NextLifeUtc == default ? 0 : _lifeManager.NextLifeUtc.Ticks;
            _saveData.undoCharges = _boosterManager.UndoCharges;
            _saveData.extraCloudCharges = _boosterManager.ExtraCloudCharges;
            _saveData.safeShuffleCharges = _boosterManager.SafeShuffleCharges;
            _saveData.soundEnabled = _feedback.SoundEnabled;
            _saveData.hapticsEnabled = _feedback.HapticsEnabled;
            _saveData.lastGeneratedSeed = _currentLevel != null ? _currentLevel.seed : 0;
            _saveService.Save(_saveData);
        }

        private Dictionary<string, object> LevelParams()
        {
            return Params("level", _currentLevel.levelId, "world", _currentLevel.worldId, "seed", _currentLevel.seed, "difficulty", _currentLevel.difficultyScore);
        }

        private static Dictionary<string, object> Params(params object[] pairs)
        {
            var result = new Dictionary<string, object>();
            for (int i = 0; i + 1 < pairs.Length; i += 2) result[(string)pairs[i]] = pairs[i + 1];
            return result;
        }
    }
}
