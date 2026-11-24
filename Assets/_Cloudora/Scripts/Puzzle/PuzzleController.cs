using System.Collections;
using System.Collections.Generic;
using Cloudora.Core;
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
        private readonly Stack<PuzzleSnapshot> _undoHistory = new();
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
        private CoinWallet _coinWallet;
        private int _moveCount;
        private int _completionReward;
        private int _shuffleUseCount;
        private int _baseCloudCount;
        private const int MaxExtraCloudsPerLevel = 2;
        private ISaveService _saveService;
        private SaveData _saveData;
        private IAnalyticsService _analytics;
        private float _levelStartedAt;
        private IAdService _adService;
        private AdPlacementPolicy _adPolicy;
        private MainMenuController _mainMenu;
        private bool _isCompleted;
        private bool _inputLocked;
        private bool _attemptBlocked;
        private bool _failed;            // "No Moves": bölüm başarısız, seçenek ekranı açık
        private bool _shuffleSelecting;  // Shuffle'a basıldı, karıştırılacak bulut bekleniyor
        private GameObject _shuffleCancelArea;
        private LevelDefinition _currentLevel;
        private const int SolverStateLimit = 200000;
        private Coroutine _solvabilityRoutine;
        private int _boardRevision;
        private int _uiLives=int.MinValue,_uiCoins=int.MinValue,_uiCompleted=int.MinValue,_uiSeconds=int.MinValue;
        private bool _resourceRefreshQueued=true;
        private readonly Dictionary<string,SolveStatus> _solvabilityCache=new();

        private sealed class PuzzleSnapshot
        {
            public WeatherType[][] board;
            public ModifierData[] modifiers;
        }

        private void Start()
        {
            _moveAnimator = gameObject.AddComponent<MoveAnimator>();
            _feedback = gameObject.AddComponent<GameFeedbackService>();
            Canvas canvas = boardRoot.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Cloudora could not start because BoardRoot is not under a Canvas.");
                enabled = false;
                return;
            }
            MobileRuntimeBootstrap.EnsureCanvasVisible(canvas);
            _saveService = new LocalJsonSaveService();
            _saveData = _saveService.Load();
            _analytics = new FirebaseAnalyticsService();
#if CLOUDORA_DEVICE_QA
            // Local device QA keeps the existing test rewards without a development watermark.
            _adService = new FakeAdService(_analytics);
#else
            _adService = Debug.isDebugBuild ? new FakeAdService(_analytics) : new AdMobAdService();
#endif
            _adPolicy = new AdPlacementPolicy();
            _analytics.Track(AnalyticsEvents.GameStarted, Params("save_schema", _saveData.schemaVersion));
            int startLevel = useDebugStartLevel ? debugStartLevel : _saveData.currentLevel;
            _progression = new ProgressionManager(startLevel, _saveData.highestCompletedLevel);
            _modifierRuntime = new ModifierRuntime();
            System.DateTime nextLife = _saveData.nextLifeUtcTicks > 0 ? new System.DateTime(_saveData.nextLifeUtcTicks, System.DateTimeKind.Utc) : default;
            _lifeManager = new LifeManager(_saveData.lives, nextLife);
            _boosterManager = new BoosterManager(_saveData.undoCharges, _saveData.extraCloudCharges, _saveData.safeShuffleCharges);
            _coinWallet = new CoinWallet(_saveData.coins);
            _feedback.SoundEnabled = _saveData.soundEnabled;
            _feedback.HapticsEnabled = _saveData.hapticsEnabled;
            _worldScreen = WorldScreenController.Create(canvas);
            _worldScreen.SetPlayAction(()=>{_mainMenu?.Hide();if(_isCompleted)HandleContinue();else if(_attemptBlocked)_overlay.ShowBlock("Canın Kalmadı","Yeni canın dolmasını bekle.");});
            _overlay = GameplayOverlay.Create(canvas, Undo, HandleContinue,
                () => { CancelShuffleSelection(); _worldScreen.Toggle(); },
                UseExtraCloud, UseSafeShuffle,
                ReturnHome,
                RetryAfterFail, ContinueWithAd, ContinueWithCoins, WatchAdForLife, () => _mainMenu?.ShowSettings());
            _overlay.SetCoins(_coinWallet.Coins);
            _shuffleCancelArea = CreateShuffleCancelArea();
            _boardLayout = boardRoot.GetComponent<AdaptiveBoardLayout>();
            if (_boardLayout == null) _boardLayout = boardRoot.gameObject.AddComponent<AdaptiveBoardLayout>();
            LoadLevel(_progression.CurrentLevel);
            RestoreFailState();
            _mainMenu = MainMenuController.Create(canvas, _currentLevel.worldId, _currentLevel.levelId, _worldScreen.Toggle,
                () => _feedback.SoundEnabled, value => { _feedback.SoundEnabled = value; SaveProgress(); },
                () => _feedback.HapticsEnabled, value => { _feedback.HapticsEnabled = value; SaveProgress(); }, WatchAdForLife);
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
            if (_attemptBlocked && _lifeManager.CanBeginAttempt(now, _currentLevel.levelId <= 5))
            {
                _attemptBlocked = false;
                _overlay.HideBlock();
                SaveProgress();
            }
            System.TimeSpan remaining = _lifeManager.TimeUntilNext(now);
            int seconds=(int)remaining.TotalSeconds;
            if(_resourceRefreshQueued||_uiLives!=_lifeManager.Lives||_uiCoins!=_coinWallet.Coins||_uiCompleted!=_saveData.highestCompletedLevel||_uiSeconds!=seconds)
            {
                _resourceRefreshQueued=false;_uiLives=_lifeManager.Lives;_uiCoins=_coinWallet.Coins;_uiCompleted=_saveData.highestCompletedLevel;_uiSeconds=seconds;
                string countdown = remaining == System.TimeSpan.Zero ? string.Empty : $"{(int)remaining.TotalMinutes:00}:{remaining.Seconds:00}";
                _overlay.SetLives(_lifeManager.Lives, countdown);
                _worldScreen?.SetResources(_coinWallet.Coins, _lifeManager.Lives, countdown);
                _mainMenu?.SetResources(_coinWallet.Coins, _lifeManager.Lives, countdown, _saveData.highestCompletedLevel);
            }
            _overlay.SetBusy(_inputLocked);
            if (_failed) _overlay.ShowFail(CurrentFailOptions()); // can yenilenince "Retry" açılır
        }

        // ---- "No Moves" = bölüm başarısız: can ile baştan, reklam veya coin ile devam ----

        private FailOptions CurrentFailOptions()
        {
            bool tutorial = _currentLevel.levelId <= 5;
            return new FailOptions(
                retryAvailable: _lifeManager.CanBeginAttempt(System.DateTime.UtcNow, tutorial),
                retryFree: tutorial,
                adAvailable: _adService.IsAvailable,
                coinCost: CoinEconomy.ContinueCost,
                coinsAffordable: _coinWallet.Coins >= CoinEconomy.ContinueCost);
        }

        private void EnterFailState()
        {
            _failed = true;
            CancelShuffleSelection();
            ClearSelection();
            _overlay.ShowFail(CurrentFailOptions());
            SaveProgress(); // başarısızlık kayda yazılır: kapatıp açmak ekranı atlatmaz
        }

        // Uygulama "No Moves" ekranındayken kapandıysa: aynı tahta ve Undo geçmişiyle ekran geri gelir.
        private void RestoreFailState()
        {
            FailRecord record = _saveData.failRecord;
            if (record == null || !record.TryRestore(_currentLevel.levelId, out WeatherType[][] board,
                    out List<(WeatherType[][] Board, ModifierData[] Modifiers)> history))
                return;

            LoadState(board);
            for (int i = history.Count - 1; i >= 0; i--)
                _undoHistory.Push(new PuzzleSnapshot { board = history[i].Board, modifiers = history[i].Modifiers });
            _moveCount = record.moveCount;
            _attemptBlocked = false; // deneme zaten başlamıştı; can 0 olsa da devam seçenekleri kullanılabilir
            _overlay.HideBlock();
            _overlay.SetUndoAvailable(_undoHistory.Count > 0);
            EnterFailState();
        }

        private FailRecord CaptureFailRecord()
        {
            PuzzleSnapshot[] history = _undoHistory.ToArray(); // en yeniden en eskiye
            var entries = new List<(WeatherType[][] Board, ModifierData[] Modifiers)>(history.Length);
            foreach (PuzzleSnapshot snapshot in history) entries.Add((snapshot.board, snapshot.modifiers));
            return FailRecord.Capture(_currentLevel.levelId, CaptureSnapshot().board, entries, _moveCount);
        }

        // 1 can harcayarak bölümü sıfırdan başlatır (ekstra bulutlar silinir). 1–5'te can gitmez.
        private void RetryAfterFail()
        {
            if (!_failed || _inputLocked) return;
            if (!_lifeManager.TryConsumeRetry(System.DateTime.UtcNow, _currentLevel.levelId <= 5))
            {
                _overlay.ShowFail(CurrentFailOptions());
                return;
            }
            StopAllCoroutines();
            LoadState(_currentLevel.CreateBoard());
            _levelStartedAt = Time.realtimeSinceStartup;
            _analytics.Track(AnalyticsEvents.LevelRetried, LevelParams());
            SaveProgress();
        }

        // Geliştirme için geçici sağ üst buton: reklam izle → +1 can. Can 0 iken açılmış bölümün kilidini de açar.
        private void WatchAdForLife()
        {
            if (_lifeManager.Lives >= LifeManager.MaxLives)
            {
                _overlay.ShowBlock("Canların Dolu", $"Zaten {LifeManager.MaxLives} canın var.");
                return;
            }
            RequestReward(RewardedPlacement.Life, () =>
            {
                _lifeManager.Grant(1);
                _attemptBlocked = false;
            }, "Can Yenilendi", "+1 can kazandın.");
        }

        private void ContinueWithAd()
        {
            if (!_failed || !_adService.IsAvailable) return;
            _adService.ShowRewarded(RewardedPlacement.Continue, success =>
            {
                if (success) ResumeFromLastSolvable("ad");
                else _overlay.ShowBlock("Ödül Alınamadı", "Ödül tamamlanamadı. Biraz sonra tekrar dene.");
            });
        }

        private void ContinueWithCoins()
        {
            if (!_failed) return;
            if (!_coinWallet.TrySpend(CoinEconomy.ContinueCost))
            {
                _overlay.ShowFail(CurrentFailOptions());
                return;
            }
            _overlay.SetCoins(_coinWallet.Coins);
            _analytics.Track(AnalyticsEvents.CoinsSpent, Params("level", _currentLevel.levelId, "amount", CoinEconomy.ContinueCost, "reason", "continue"));
            ResumeFromLastSolvable("coins");
        }

        // Çözüme ulaşılabilen son duruma döner (gerekirse birkaç hamle geri). Geçmişte yoksa bölüm başına
        // döner ama satın alınmış ekstra bulutlar korunur.
        private void ResumeFromLastSolvable(string method)
        {
            PuzzleSnapshot[] history = _undoHistory.ToArray(); // en yeniden en eskiye
            var boards = new WeatherType[history.Length][][];
            for (int i = 0; i < history.Length; i++) boards[i] = history[i].board;
            int index = FailRecovery.NewestSolvableIndex(boards, _currentLevel.capacity, SolverStateLimit);

            _failed = false;
            _overlay.HideFail();
            if (index >= 0)
            {
                PuzzleSnapshot target = null;
                for (int i = 0; i <= index; i++) target = _undoHistory.Pop();
                RestoreSnapshot(target);
                _moveCount = Mathf.Max(0, _moveCount - (index + 1));
                _overlay.SetUndoAvailable(_undoHistory.Count > 0);
            }
            else
            {
                int extras = ExtraCloudCount;
                LoadState(_currentLevel.CreateBoard());
                AddExtraClouds(extras);
            }
            _analytics.Track(AnalyticsEvents.LevelContinued, Params("level", _currentLevel.levelId, "method", method, "rewound_moves", index + 1));
            RefreshMetaUI();
            SaveProgress();
        }

        public void LoadLevel(int levelNumber)
        {
            _progression.DebugJump(levelNumber);
            levelNumber=_progression.CurrentLevel;
            _shuffleUseCount = 0;
            if (levelNumber <= AuthoredLevelCatalog.Count) _currentLevel = AuthoredLevelCatalog.Get(levelNumber);
            else if (!AuthoredLevelCatalog.TryGetBuilt(levelNumber, out _currentLevel)) _currentLevel = LevelGenerator.Generate(levelNumber);
            _baseCloudCount = _currentLevel.clouds.Length;
            LoadState(_currentLevel.CreateBoard());
            _overlay.SetLevelInfo(_currentLevel.levelId, _currentLevel.worldId);
            _overlay.SetTutorialCue(_currentLevel.tutorialCue);
            _overlay.SetShuffleAvailable(IsShuffleUnlocked(_currentLevel.levelId));
            _worldScreen.Refresh(_progression.CurrentWorld, _progression.HighestCompletedLevel);
            _mainMenu?.SetProgress(_currentLevel.worldId, _currentLevel.levelId);
            _attemptBlocked = !_lifeManager.CanBeginAttempt(System.DateTime.UtcNow, _currentLevel.levelId <= 5);
            if (_attemptBlocked) _overlay.ShowBlock("Canın Kalmadı", "Yeni canın dolmasını bekle.");
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
            CancelShuffleSelection();
            ClearSelection();
            // Bitmiş bölümde Undo yok (aksi halde yeniden çözülüp coin iki kez kazanılabilirdi).
            if (_inputLocked || _attemptBlocked || _failed || _isCompleted || _undoHistory.Count == 0)
            {
                return;
            }

            if (!_boosterManager.TryUseUndo())
            {
                RequestReward(RewardedPlacement.Undo, () => _boosterManager.GrantUndo(), "Geri Alma Hakkın Yenilendi", "3 geri alma hakkı kazandın. Geri Al düğmesine dokun.");
                return;
            }

            CancelSolvabilityCheck();
            RestoreSnapshot(_undoHistory.Pop());
            BeginSolvabilityCheck();
            _moveCount = Mathf.Max(0, _moveCount - 1);
            _isCompleted = false;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(_undoHistory.Count > 0);
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.UndoUsed, Params("level", _currentLevel.levelId, "remaining", _boosterManager.UndoCharges));
            SaveProgress();
        }

        private void LoadState(WeatherType[][] levelData)
        {
            CancelSolvabilityCheck();
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
            _failed = false;
            _overlay.ShowComplete(false);
            _overlay.SetUndoAvailable(false);
            _overlay.HideBlock();
            _overlay.HideFail();
            CancelShuffleSelection();
            RefreshMetaUI();

            for (int i = 0; i < levelData.Length; i++)
            {
                CloudContainerView cloud = Instantiate(cloudPrefab, boardRoot);
                cloud.name = $"Cloud_{i + 1}";
                cloud.Initialize(_currentLevel.capacity, levelData[i], HandleCloudClicked);
                _containers.Add(cloud);
                if(cloud.IsComplete)_celebratedSolvedClouds.Add(cloud);
            }
            // Yeni tahta ekstra bulutları kaldırır; ızgara gerçek bulut sayısına göre yeniden boyutlanmalı.
            _boardLayout.Configure(_containers.Count, _currentLevel.capacity);
            _modifierRuntime.Initialize(_currentLevel, _containers.ToArray());
            RefreshExtraButton();
        }

        private void HandleCloudClicked(CloudContainerView clickedCloud)
        {
            if (_isCompleted || _inputLocked || _attemptBlocked || _failed)
            {
                return;
            }

            if(clickedCloud.IsComplete)return;
            if (_shuffleSelecting)
            {
                ShuffleCloud(clickedCloud);
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
                count = _modifierRuntime.AdjustMoveCount(sourceIndex, targetIndex, count);
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
            CancelSolvabilityCheck();
            _inputLocked = true;
            _overlay.SetBusy(true);
            PuzzleSnapshot beforeMove = CaptureSnapshot();
            yield return _moveAnimator.Animate(source, target, type, count);

            if (source.TryMoveTopGroupTo(target, count))
            {
                _undoHistory.Push(beforeMove);
                _moveCount++;
                _feedback.Move();
                _modifierRuntime.OnSuccessfulMove(_containers.IndexOf(source), _containers.IndexOf(target));
                _overlay.SetUndoAvailable(true);
                CelebrateNewlySolvedClouds();
                CheckWin();
                if(_isCompleted)
                {
                    ClearSelection();
                    yield return new WaitForSecondsRealtime(VesselCompletionFlourish.ResultDelay);
                    _worldScreen.Refresh(_progression.CurrentWorld,_progression.HighestCompletedLevel);
                    _overlay.ShowComplete(true,_completionReward);_feedback.Complete();
                    if(_adPolicy.RegisterCompletion(_currentLevel.levelId,System.DateTime.UtcNow))_adService.ShowInterstitial();
                }
                else BeginSolvabilityCheck();
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

        private PuzzleSnapshot CaptureSnapshot()
        {
            var snapshot = new WeatherType[_containers.Count][];
            for (int i = 0; i < _containers.Count; i++)
            {
                snapshot[i] = _containers[i].CaptureElements();
            }
            return new PuzzleSnapshot { board = snapshot, modifiers = _modifierRuntime.CaptureSnapshot() };
        }

        private void RestoreSnapshot(PuzzleSnapshot snapshot)
        {
            ClearSelection();
            _celebratedSolvedClouds.Clear();
            for (int i = 0; i < snapshot.board.Length && i < _containers.Count; i++)
            {
                _containers[i].RestoreElements(snapshot.board[i]);
                if(_containers[i].IsComplete)_celebratedSolvedClouds.Add(_containers[i]);
            }
            _modifierRuntime.RestoreSnapshot(snapshot.modifiers);
        }

        private void CheckWin()
        {
            if(_isCompleted)return;
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
            int reward = CoinEconomy.RewardFor(_currentLevel.levelId);
            _coinWallet.Earn(reward);
            _overlay.SetCoins(_coinWallet.Coins);
            _analytics.Track(AnalyticsEvents.CoinsEarned, Params("level", _currentLevel.levelId, "amount", reward));
            _completionReward=reward;
            _analytics.Track(AnalyticsEvents.LevelCompleted, Params("level", _currentLevel.levelId, "duration_seconds", Time.realtimeSinceStartup - _levelStartedAt, "move_count", _moveCount, "seed", _currentLevel.seed));
            if (_currentLevel.levelId == _progression.CurrentWorld.LastLevel)
                _analytics.Track(AnalyticsEvents.WorldCompleted, Params("world", _progression.CurrentWorld.Id));
            Debug.Log("Level completed!");
            SaveProgress();
        }

        private void ReturnHome()
        {
            if(_inputLocked)return;
            CancelShuffleSelection();CancelSolvabilityCheck();
            if(_isCompleted)
            {
                if(_progression.CurrentLevel<WorldCatalog.MaxLevel){_progression.Advance();LoadLevel(_progression.CurrentLevel);}
            }
            else
            {
                // Abandoning any unfinished attempt costs one available life, even in tutorials.
                // At zero lives the player can still leave; lives never become negative.
                _lifeManager.TryConsumeRetry(System.DateTime.UtcNow,false);
                LoadLevel(_progression.CurrentLevel);
            }
            SaveProgress();_mainMenu?.Show();
        }

        private void HandleContinue()
        {
            if(_progression.CurrentLevel>=WorldCatalog.MaxLevel){_overlay.ShowComplete(false);_mainMenu?.Show();return;}
            if (!_lifeManager.CanBeginAttempt(System.DateTime.UtcNow, _progression.CurrentLevel + 1 <= 5))
            {
                _overlay.ShowBlock("Canın Kalmadı", "Yeni canın dolmasını bekle.");
                return;
            }
            _progression.Advance();
            LoadLevel(_progression.CurrentLevel);
            SaveProgress();
        }

        private void UseExtraCloud()
        {
            CancelShuffleSelection();
            if (_inputLocked || _attemptBlocked || _isCompleted || _failed) return;
            // Limit kontrolü hak/reklam harcanmadan önce yapılır.
            if (ExtraCloudCount >= MaxExtraCloudsPerLevel)
            {
                // Shuffle 16'dan önce kilitli; kilitliyken önerilmez.
                _overlay.ShowBlock("Gökyüzü Doldu", IsShuffleUnlocked(_currentLevel.levelId)
                    ? "Bu bölümün ek hazne sınırına ulaştın. Geri Al veya Karıştır kullanabilirsin."
                    : "Bu bölümün ek hazne sınırına ulaştın. Geri Al kullanabilirsin.");
                return;
            }
            if (!_boosterManager.TryUseExtraCloud())
            {
                RequestReward(RewardedPlacement.ExtraCloud, () => _boosterManager.GrantExtraCloud(), "Ek Hazne Hakkın Yenilendi", "Ek hazne hakkı kazandın. Ek Bulut düğmesine dokun.");
                return;
            }

            CancelSolvabilityCheck();
            AddExtraClouds(1);
            BeginSolvabilityCheck();
            _undoHistory.Clear();
            _overlay.HideBlock();
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.ExtraCloudUsed, LevelParams());
            SaveProgress();
        }

        // ---- v3.1 Safe Shuffle: bir bulut seçilir, kendi içinde karıştırılır; sonuç her zaman çözülebilir ve farklıdır ----

        public static bool IsShuffleUnlocked(int level) => level >= V3LevelPlan.ShuffleUnlockLevel;

        private void UseSafeShuffle()
        {
            if (_shuffleSelecting)
            {
                CancelShuffleSelection(); // ikinci basış iptal eder
                return;
            }
            if (_inputLocked || _attemptBlocked || _isCompleted || _failed) return;
            if (!IsShuffleUnlocked(_currentLevel.levelId))
            {
                _overlay.ShowBlock("Karıştır Henüz Kilitli", $"{V3LevelPlan.ShuffleUnlockLevel}. bölümde açılır.");
                return;
            }
            if (_boosterManager.SafeShuffleCharges <= 0)
            {
                RequestReward(RewardedPlacement.SafeShuffle, () => _boosterManager.GrantSafeShuffle(), "Karıştırma Hakkın Yenilendi", "Karıştırma hakkı kazandın. Karıştır düğmesine dokun.");
                return;
            }

            ClearSelection(); // seçili bir bulut varsa seçim kaldırılır
            _shuffleSelecting = true;
            _overlay.SetModeHint("Karıştırmak için bir hazne seç.");
            _shuffleCancelArea.SetActive(true);
        }

        private void ShuffleCloud(CloudContainerView cloud)
        {
            if(cloud.IsComplete)return;
            int index = _containers.IndexOf(cloud);
            CloudShuffleResult result = CloudShuffler.Shuffle(CaptureSnapshot().board, _currentLevel.capacity, index,
                _currentLevel.seed + (++_shuffleUseCount * 997), SolverStateLimit, _modifierRuntime.ProtectedClouds());
            if (!result.Success)
            {
                // Hak düşmez; oyuncu başka bir bulut seçebilir veya iptal edebilir.
                cloud.PlayInvalidFeedback();
                _feedback.Invalid();
                _overlay.SetModeHint(result.Rejection switch
                {
                    CloudShuffleRejection.Protected => "Özel hazneler karıştırılamaz. Başka bir hazne seç.",
                    CloudShuffleRejection.NoSafeArrangement => "Bu hazne güvenle karıştırılamıyor. Başka bir hazne seç.",
                    _ => "Bu hazne değiştirilemiyor. Başka bir hazne seç."
                });
                return;
            }

            _boosterManager.TryUseSafeShuffle();
            CancelSolvabilityCheck();
            cloud.RestoreElements(result.Board[index]);
            // Shuffle geri alınamaz: Undo geçmişi temizlenir, Undo butonu söner.
            _undoHistory.Clear();
            _overlay.SetUndoAvailable(false);
            CancelShuffleSelection();
            _feedback.Move();
            RefreshMetaUI();
            _analytics.Track(AnalyticsEvents.SafeShuffleUsed, Params("level", _currentLevel.levelId, "cloud", index, "remaining", _boosterManager.SafeShuffleCharges));
            SaveProgress();
        }

        private void CancelShuffleSelection()
        {
            if (!_shuffleSelecting) return;
            _shuffleSelecting = false;
            _overlay.SetModeHint(null);
            if (_shuffleCancelArea != null) _shuffleCancelArea.SetActive(false);
        }

        // Bulutların arkasında, ekranın geri kalanını kaplayan görünmez alan: seçim modunda boş yere dokunmak iptal eder.
        private GameObject CreateShuffleCancelArea()
        {
            var go = new GameObject("ShuffleCancelArea", typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
            go.transform.SetParent(boardRoot.parent, false);
            go.transform.SetSiblingIndex(boardRoot.GetSiblingIndex());
            var rect = (RectTransform)go.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            go.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 0f, 0f, 0f);
            go.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CancelShuffleSelection);
            go.SetActive(false);
            return go;
        }

        // Seviyenin orijinal bulut sayısına göre şu an tahtada olan ekstra bulutlar (Shuffle'a katılmış olanlar dahil).
        private int ExtraCloudCount => Mathf.Max(0, _containers.Count - _baseCloudCount);

        private void RefreshExtraButton() => _overlay.SetExtraAvailable(ExtraCloudCount < MaxExtraCloudsPerLevel);

        private void AddExtraClouds(int count)
        {
            if (count <= 0) return;
            for (int i = 0; i < count; i++)
            {
                CloudContainerView cloud = Instantiate(cloudPrefab, boardRoot);
                cloud.name = $"Cloud_{_containers.Count + 1}_Extra";
                cloud.Initialize(_currentLevel.capacity, System.Array.Empty<WeatherType>(), HandleCloudClicked);
                _containers.Add(cloud);
            }
            _boardLayout.Configure(_containers.Count, _currentLevel.capacity);
            _modifierRuntime.UpdateClouds(_containers.ToArray());
            RefreshExtraButton();
        }

        private void CancelSolvabilityCheck()
        {
            _boardRevision++;
            if(_solvabilityRoutine!=null){StopCoroutine(_solvabilityRoutine);_solvabilityRoutine=null;}
        }
        private void BeginSolvabilityCheck()
        {
            CancelSolvabilityCheck();
            if(_isCompleted || _failed)return;
            _solvabilityRoutine=StartCoroutine(CheckCurrentBoard(_boardRevision));
        }
        private IEnumerator CheckCurrentBoard(int revision)
        {
            // Defer one frame so the move can finish and input can resume.
            yield return null;
            var board=CaptureSnapshot().board;
            string key=PuzzleSolver.ReachabilityKey(board,_currentLevel.capacity);
            if(!_solvabilityCache.TryGetValue(key,out var result))
            {
                var search=new PuzzleSolver.ReachabilitySearch(board,_currentLevel.capacity);
                do
                {
                    result=search.Step();
                    if(result==SolveStatus.LimitExceeded)yield return null;
                    if(revision!=_boardRevision)yield break;
                }while(result==SolveStatus.LimitExceeded);
                if(_solvabilityCache.Count>512)_solvabilityCache.Clear();
                _solvabilityCache[key]=result;
            }
            _solvabilityRoutine=null;
            if(revision==_boardRevision && !_isCompleted && result==SolveStatus.Unsolvable)
            {
                _analytics.Track(AnalyticsEvents.LevelFailed,Params("level",_currentLevel.levelId,"reason","unsolvable","move_count",_moveCount));
                EnterFailState();
            }
        }

        private void RequestReward(RewardedPlacement placement, System.Action grant, string successTitle, string successMessage)
        {
            if (!_adService.IsAvailable)
            {
                _overlay.ShowBlock("Ödül Alınamadı", "Bu sürümde ödüllü reklamlar kullanılamıyor.");
                return;
            }

            _overlay.ShowBlock("Ödül Bekleniyor", "Devam etmek için ödülü tamamla.");
            _adService.ShowRewarded(placement, success =>
            {
                if (!success)
                {
                    _overlay.ShowBlock("Ödül Alınamadı", "Ödül tamamlanamadı. Biraz sonra tekrar dene.");
                    return;
                }
                grant?.Invoke();
                RefreshMetaUI();
                SaveProgress();
                _overlay.ShowBlock(successTitle, successMessage);
            });
        }

        private void RefreshMetaUI()
        {
            _resourceRefreshQueued=true;
            if (_overlay != null)
                _overlay.SetBoosters(_boosterManager.UndoCharges, _boosterManager.ExtraCloudCharges, _boosterManager.SafeShuffleCharges, _adService.IsAvailable);
        }

        private void SaveProgress()
        {
            _resourceRefreshQueued=true;
            if (_saveService == null || _saveData == null || _progression == null) return;
            _saveData.currentLevel = _progression.ResumeLevel(_isCompleted);
            _saveData.highestCompletedLevel = _progression.HighestCompletedLevel;
            _saveData.lives = _lifeManager.Lives;
            _saveData.nextLifeUtcTicks = _lifeManager.NextLifeUtc == default ? 0 : _lifeManager.NextLifeUtc.Ticks;
            _saveData.undoCharges = _boosterManager.UndoCharges;
            _saveData.extraCloudCharges = _boosterManager.ExtraCloudCharges;
            _saveData.safeShuffleCharges = _boosterManager.SafeShuffleCharges;
            _saveData.coins = _coinWallet.Coins;
            _saveData.failRecord = _failed && _currentLevel != null ? CaptureFailRecord() : new FailRecord();
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
