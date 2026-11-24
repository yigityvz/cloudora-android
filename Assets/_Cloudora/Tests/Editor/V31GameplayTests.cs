using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Progression;
using Cloudora.Puzzle;
using Cloudora.Services;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    // v3.1: bulut içi Shuffle, başarısızlıkta "çözülebilen son duruma dön", coin, yeni buton düzeni.
    public sealed class V31GameplayTests
    {
        private const WeatherType S = WeatherType.Sun;
        private const WeatherType R = WeatherType.Rain;
        private const WeatherType N = WeatherType.Snow;
        private const int Limit = 200000;

        private static WeatherType[] E() => Array.Empty<WeatherType>();
        private static string Text(WeatherType[][] board) => V3BoardText.Format(board);

        // ---------- Bulut içi Shuffle ----------

        [Test]
        public void ShuffleRejectsCloudsThatCannotChange()
        {
            WeatherType[][] board = { E(), new[] { S }, new[] { R, R, R }, new[] { S, R, S, R }, E() };
            Assert.That(CloudShuffler.Shuffle(board, 4, 0, 1, Limit).Rejection, Is.EqualTo(CloudShuffleRejection.NotShuffleable)); // boş
            Assert.That(CloudShuffler.Shuffle(board, 4, 1, 1, Limit).Rejection, Is.EqualTo(CloudShuffleRejection.NotShuffleable)); // tek eleman
            Assert.That(CloudShuffler.Shuffle(board, 4, 2, 1, Limit).Rejection, Is.EqualTo(CloudShuffleRejection.NotShuffleable)); // tek renk
            Assert.That(CloudShuffler.CanChange(board[3]), Is.True);
            Assert.Throws<ArgumentOutOfRangeException>(() => CloudShuffler.Shuffle(board, 4, 9, 1, Limit));
        }

        [Test]
        public void ShuffleNeverTouchesProtectedClouds()
        {
            WeatherType[][] board = { new[] { S, R, S, R }, new[] { R, S, R, S }, E(), E() };
            CloudShuffleResult result = CloudShuffler.Shuffle(board, 4, 0, 1, Limit, new HashSet<int> { 0 });
            Assert.That(result.Success, Is.False);
            Assert.That(result.Rejection, Is.EqualTo(CloudShuffleRejection.Protected));
        }

        [Test]
        public void ShuffleChangesOnlyTheChosenCloudAndKeepsItsContents()
        {
            for (int seed = 1; seed <= 40; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(4, 3, 2), seed);
                int chosen = seed % 3;
                if (!CloudShuffler.CanChange(board[chosen])) continue; // dağıtımda tek renkli bulut çıkabilir
                string before = Text(board);
                CloudShuffleResult result = CloudShuffler.Shuffle(board, 4, chosen, seed, Limit);
                Assert.That(Text(board), Is.EqualTo(before), "input must not be mutated");
                Assert.That(result.Success, Is.True, $"seed {seed}: {result.Rejection}");
                for (int c = 0; c < board.Length; c++)
                {
                    if (c == chosen) continue;
                    Assert.That(result.Board[c], Is.EqualTo(board[c]), $"seed {seed} cloud {c} changed");
                }
                Assert.That(result.Board[chosen], Is.Not.EqualTo(board[chosen]), $"seed {seed}: order did not change");
                Assert.That(result.Board[chosen].OrderBy(w => w), Is.EqualTo(board[chosen].OrderBy(w => w)), $"seed {seed}: contents changed");
            }
        }

        [Test]
        public void ShuffleAlwaysLeavesASolvableBoard()
        {
            // 1 boş bulutlu çerçevede çıkmaz durumlar var; karıştırma asla çözülemez sonuç vermemeli.
            int checkedBoards = 0;
            for (int seed = 1; seed <= 60; seed++)
            {
                WeatherType[][] board = LevelCandidateFactory.Deal(new BoardFrame(4, 3, 1), seed);
                for (int cloud = 0; cloud < 3; cloud++)
                {
                    CloudShuffleResult result = CloudShuffler.Shuffle(board, 4, cloud, seed * 31 + cloud, Limit);
                    if (!result.Success) continue;
                    checkedBoards++;
                    Assert.That(PuzzleSolver.Solve(result.Board, 4, Limit).Status, Is.EqualTo(SolveStatus.Solved), $"seed {seed} cloud {cloud}");
                }
            }
            Assert.That(checkedBoards, Is.GreaterThan(100));
        }

        [Test]
        public void ShuffleReportsWhenNoSafeArrangementExists()
        {
            // Kapasite 2, boş bulut yok: tek farklı diziliş [R,S] tahtayı hamlesiz bırakır → güvenli sonuç yok.
            WeatherType[][] board = { new[] { S, R }, new[] { S, R } };
            CloudShuffleResult result = CloudShuffler.Shuffle(board, 2, 0, 1, Limit);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Rejection, Is.EqualTo(CloudShuffleRejection.NoSafeArrangement));
        }

        [Test]
        public void ShuffleIsDeterministicPerSeedAndVariesAcrossSeeds()
        {
            WeatherType[][] board = { new[] { S, R, N, S }, new[] { R, N, S, R }, new[] { N, S, R, N }, E(), E() };
            Assert.That(Text(CloudShuffler.Shuffle(board, 4, 0, 7, Limit).Board), Is.EqualTo(Text(CloudShuffler.Shuffle(board, 4, 0, 7, Limit).Board)));
            var outcomes = new HashSet<string>();
            for (int seed = 1; seed <= 20; seed++) outcomes.Add(Text(CloudShuffler.Shuffle(board, 4, 0, seed, Limit).Board));
            Assert.That(outcomes.Count, Is.GreaterThan(1));
        }

        // ---------- Başarısızlıkta devam: çözülebilen son durum ----------

        [Test]
        public void ContinueFindsNewestSolvableState()
        {
            WeatherType[][] dead = { new[] { S, R }, new[] { R, S } };   // hamle yok, çözülemez
            WeatherType[][] alive = { new[] { S, R }, new[] { R }, new[] { S } };
            WeatherType[][] start = { new[] { S, R }, new[] { R, S }, E() };
            // En yeniden en eskiye.
            Assert.That(FailRecovery.NewestSolvableIndex(new[] { dead, alive, start }, 2, Limit), Is.EqualTo(1));
            Assert.That(FailRecovery.NewestSolvableIndex(new[] { alive, dead }, 2, Limit), Is.EqualTo(0));
            Assert.That(FailRecovery.NewestSolvableIndex(new[] { dead, dead }, 2, Limit), Is.EqualTo(-1));
            Assert.That(FailRecovery.NewestSolvableIndex(Array.Empty<WeatherType[][]>(), 2, Limit), Is.EqualTo(-1));
        }

        // ---------- Coin ----------

        [Test]
        public void CoinRewardsFollowDifficultyAndContinueIsMeaningful()
        {
            int medium = CoinEconomy.RewardFor(1);   // Orta
            int hard = CoinEconomy.RewardFor(3);     // Zor
            int veryHard = CoinEconomy.RewardFor(5); // Çok zor
            Assert.That(medium, Is.GreaterThan(0));
            Assert.That(hard, Is.GreaterThan(medium));
            Assert.That(veryHard, Is.GreaterThan(hard));
            Assert.That(CoinEconomy.RewardFor(150), Is.GreaterThan(0)); // eski sistem bölümleri de kazandırır
            // Devam bedeli tek bölümün kazancından fazla, ama birkaç bölümde biriktirilebilir.
            Assert.That(CoinEconomy.ContinueCost, Is.GreaterThan(veryHard));
            Assert.That(CoinEconomy.ContinueCost, Is.LessThanOrEqualTo(medium * 5));
            Assert.That(CoinEconomy.StartingCoins, Is.EqualTo(0));
        }

        [Test]
        public void CoinWalletNeverGoesNegative()
        {
            var wallet = new CoinWallet(5);
            Assert.That(wallet.TrySpend(10), Is.False);
            Assert.That(wallet.Coins, Is.EqualTo(5));
            wallet.Earn(20);
            Assert.That(wallet.TrySpend(10), Is.True);
            Assert.That(wallet.Coins, Is.EqualTo(15));
            wallet.Earn(-50);
            Assert.That(wallet.Coins, Is.EqualTo(15));
            Assert.That(new CoinWallet(-3).Coins, Is.EqualTo(0));
        }

        [Test]
        public void CoinsSurviveSaveAndAreNormalized()
        {
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cloudora-coin-test-" + Guid.NewGuid().ToString("N"));
            string path = System.IO.Path.Combine(directory, "save.json");
            var service = new LocalJsonSaveService(path);
            SaveData data = SaveData.Defaults();
            Assert.That(data.coins, Is.EqualTo(CoinEconomy.StartingCoins));
            data.coins = 125;
            service.Save(data);
            Assert.That(service.Load().coins, Is.EqualTo(125));

            System.IO.File.WriteAllText(path, "{\"schemaVersion\":2,\"currentLevel\":3,\"coins\":-40}");
            Assert.That(new LocalJsonSaveService(path).Load().coins, Is.EqualTo(0));
            System.IO.File.WriteAllText(path, "{\"schemaVersion\":2,\"currentLevel\":3}");
            Assert.That(new LocalJsonSaveService(path).Load().coins, Is.EqualTo(0)); // eski kayıtta alan yok
            System.IO.Directory.Delete(directory, true);
        }

        // ---------- Başarısızlığın kaydı (kapat-aç açığı kapalı) ----------

        [Test]
        public void FailRecordRoundTripsThroughSave()
        {
            WeatherType[][] board = { new[] { S, S, R, R }, new[] { R, R, S }, new[] { S }, E() };
            WeatherType[][] previous = { new[] { S, S, R, R }, new[] { R, R, S, S }, E(), E() };
            WeatherType[][] start = { new[] { S, S, R, R }, new[] { R, R, S }, E(), new[] { S } };
            var frozen = new Cloudora.Modifiers.ModifierData { type = Cloudora.Modifiers.ModifierType.Frozen, cloudIndex = 3, counter = 1 };
            var history = new List<(WeatherType[][], Cloudora.Modifiers.ModifierData[])>
            {
                (previous, new[] { frozen }),
                (start, Array.Empty<Cloudora.Modifiers.ModifierData>())
            };

            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cloudora-fail-test-" + Guid.NewGuid().ToString("N"));
            string path = System.IO.Path.Combine(directory, "save.json");
            SaveData data = SaveData.Defaults();
            data.failRecord = FailRecord.Capture(19, board, history, 3);
            new LocalJsonSaveService(path).Save(data);
            SaveData loaded = new LocalJsonSaveService(path).Load();
            System.IO.Directory.Delete(directory, true);

            Assert.That(loaded.failRecord.IsEmpty, Is.False);
            Assert.That(loaded.failRecord.moveCount, Is.EqualTo(3));
            Assert.That(loaded.failRecord.TryRestore(19, out WeatherType[][] restored, out var restoredHistory), Is.True);
            Assert.That(Text(restored), Is.EqualTo(Text(board)));
            Assert.That(restoredHistory.Select(h => Text(h.Board)), Is.EqualTo(new[] { Text(previous), Text(start) }), "newest first");
            Assert.That(restoredHistory[0].Modifiers.Single().type, Is.EqualTo(Cloudora.Modifiers.ModifierType.Frozen));
            Assert.That(restoredHistory[0].Modifiers.Single().counter, Is.EqualTo(1));
            Assert.That(restoredHistory[1].Modifiers, Is.Empty);
        }

        [Test]
        public void FailRecordIsIgnoredWhenItDoesNotFit()
        {
            WeatherType[][] board = { new[] { S, R }, new[] { R, S }, E() };
            var none = new List<(WeatherType[][], Cloudora.Modifiers.ModifierData[])>();
            Assert.That(new FailRecord().IsEmpty, Is.True);
            Assert.That(new FailRecord().TryRestore(19, out _, out _), Is.False);
            Assert.That(FailRecord.Capture(19, board, none, 1).TryRestore(20, out _, out _), Is.False, "other level");

            FailRecord corrupt = FailRecord.Capture(19, board, none, 1);
            corrupt.board = "SX/";
            Assert.That(corrupt.TryRestore(19, out _, out _), Is.False, "unreadable board");

            WeatherType[][] fewerClouds = { new[] { S, R }, new[] { R, S } };
            FailRecord mismatched = FailRecord.Capture(19, board,
                new List<(WeatherType[][], Cloudora.Modifiers.ModifierData[])> { (fewerClouds, Array.Empty<Cloudora.Modifiers.ModifierData>()) }, 1);
            Assert.That(mismatched.TryRestore(19, out _, out _), Is.False, "history cloud count differs");
        }

        [Test]
        public void OldSavesLoadWithoutFailRecord()
        {
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "cloudora-fail-old-" + Guid.NewGuid().ToString("N"));
            string path = System.IO.Path.Combine(directory, "save.json");
            System.IO.Directory.CreateDirectory(directory);
            System.IO.File.WriteAllText(path, "{\"schemaVersion\":2,\"currentLevel\":19,\"lives\":3}");
            SaveData loaded = new LocalJsonSaveService(path).Load();
            System.IO.Directory.Delete(directory, true);
            Assert.That(loaded.currentLevel, Is.EqualTo(19));
            Assert.That(loaded.failRecord, Is.Not.Null);
            Assert.That(loaded.failRecord.IsEmpty, Is.True);
        }

        // ---------- Arayüz: buton düzeni ve başarısızlık ekranı ----------

        [Test]
        public void FigmaHelpersHaveDistinctTouchTargetsAndNoRestart()
        {
            (GameObject root, GameplayOverlay overlay) = CreateOverlay();
            try {
                Assert.That(Find(root,"RestartButton"),Is.Null);
                Vector2 undo=Pos(root,"UndoButton"),extra=Pos(root,"ExtraButton"),shuffle=Pos(root,"ShuffleButton");
                Assert.That(undo.x,Is.LessThan(extra.x));Assert.That(extra.x,Is.LessThan(shuffle.x));
                Assert.That(undo.y,Is.EqualTo(extra.y));Assert.That(extra.y,Is.EqualTo(shuffle.y));
                foreach(string name in new[]{"UndoButton","ExtraButton","ShuffleButton"}){
                    var r=(RectTransform)Find(root,name).transform;Assert.That(r.sizeDelta.x,Is.GreaterThanOrEqualTo(44));Assert.That(r.sizeDelta.y,Is.GreaterThanOrEqualTo(44));
                }
            } finally {Object.DestroyImmediate(root);}
        }

        [Test]
        public void FailPanelOffersLifeAdAndCoinsAndBlocksBoosters()
        {
            (GameObject root, GameplayOverlay overlay) = CreateOverlay();
            try
            {
                overlay.ShowFail(new FailOptions(retryAvailable: true, retryFree: false, adAvailable: true, coinCost: 40, coinsAffordable: false));
                GameObject panel = Find(root, "FailPanel");
                Assert.That(panel, Is.Not.Null);
                Assert.That(panel.activeSelf, Is.True);
                Assert.That(((RectTransform)panel.transform).offsetMin.y, Is.EqualTo(0f), "fail panel must cover the booster buttons");
                Assert.That(Find(panel, "RetryButton").GetComponent<Button>().interactable, Is.True);
                Assert.That(Find(panel, "AdButton").GetComponent<Button>().interactable, Is.True);
                Assert.That(Find(panel, "CoinsButton").GetComponent<Button>().interactable, Is.False, "not enough coins");

                overlay.ShowFail(new FailOptions(retryAvailable: false, retryFree: false, adAvailable: false, coinCost: 40, coinsAffordable: true));
                Assert.That(Find(panel, "RetryButton").GetComponent<Button>().interactable, Is.False, "no lives");
                Assert.That(Find(panel, "AdButton").GetComponent<Button>().interactable, Is.False, "no ads in this build");
                Assert.That(Find(panel, "CoinsButton").GetComponent<Button>().interactable, Is.True);

                overlay.HideFail();
                Assert.That(panel.activeSelf, Is.False);
            }
            finally { Object.DestroyImmediate(root); }
        }

        [Test]
        public void LifeAdRemainsAvailableFromPauseMenu()
        {
            (GameObject root,GameplayOverlay overlay)=CreateOverlay();
            try { var button=Find(root,"LifeAdButton");Assert.That(button,Is.Not.Null);Assert.That(button.GetComponent<Button>(),Is.Not.Null);Assert.That(button.transform.IsChildOf(Find(root,"PausePanel").transform),Is.True); }
            finally {Object.DestroyImmediate(root);}
        }

        [Test]
        public void ShuffleUnlocksAtSixteenByLevelNumber()
        {
            Assert.That(V3LevelPlan.ShuffleUnlockLevel, Is.EqualTo(16));
            Assert.That(PuzzleController.IsShuffleUnlocked(15), Is.False);
            Assert.That(PuzzleController.IsShuffleUnlocked(16), Is.True);
            Assert.That(PuzzleController.IsShuffleUnlocked(250), Is.True);
        }

        private static (GameObject, GameplayOverlay) CreateOverlay()
        {
            var root = new GameObject("TestCanvas", typeof(RectTransform), typeof(Canvas));
            GameplayOverlay overlay = GameplayOverlay.Create(root.GetComponent<Canvas>(),
                null, null, null, null, null, null, null, null, null, null);
            return (root, overlay);
        }

        private static GameObject Find(GameObject root, string name) =>
            root.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == name)?.gameObject;

        private static Vector2 Pos(GameObject root, string name) => ((RectTransform)Find(root, name).transform).anchoredPosition;
    }
}
