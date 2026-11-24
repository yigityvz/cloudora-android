using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Modifiers;
using Cloudora.Progression;
using Cloudora.Puzzle;
using Cloudora.Services;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object=UnityEngine.Object;
namespace Cloudora.Tests.Editor
{
    public class CompletionEconomyTests
    {
        [Test]
        public void EveryLevelUsesTheRequestedRewardCycleAndContinueFee()
        {
            for(int i=1;i<=10000;i++){int band=(i-1)%5;int expected=band==2?200:band==4?300:100;Assert.That(CoinEconomy.RewardFor(i),Is.EqualTo(expected),"level "+i);}
            Assert.That(Enumerable.Range(1,100).Sum(CoinEconomy.RewardFor),Is.EqualTo(16000));Assert.That(CoinEconomy.ContinueCost,Is.EqualTo(500));
        }
        [Test]
        public void CompletedSourcesAreImmutableInCoreViewAndShuffleIncludingWildcards()
        {
            foreach(var values in new[]{new[]{WeatherType.Sun,WeatherType.Sun,WeatherType.Sun,WeatherType.Sun},new[]{WeatherType.Sun,WeatherType.Rainbow,WeatherType.Sun,WeatherType.Sun}})
            {
                var state=new PuzzleState(new[]{new CloudState(4,values),new CloudState(4,Array.Empty<WeatherType>())});var type=values[^1];
                Assert.That(state.Clouds[0].IsComplete,Is.True);Assert.That(PuzzleRules.CanApply(state,new Move(0,1,state.Clouds[0].TopGroupCount(),type)),Is.False);
                Assert.That(CloudShuffler.Shuffle(new[]{values,Array.Empty<WeatherType>()},4,0,1,1000).Success,Is.False);
                var a=Make(null,"Completed");var b=Make(null,"Empty");a.RestoreElements(values);
                Assert.That(a.TryMoveTopGroupTo(b),Is.False);Assert.That(a.CaptureElements(),Is.EqualTo(values));Assert.That(b.IsEmpty,Is.True);
                Object.DestroyImmediate(a.gameObject);Object.DestroyImmediate(b.gameObject);
            }
        }
        private sealed class MemorySave:ISaveService{public SaveData Data=SaveData.Defaults();public SaveData Load()=>Data;public void Save(SaveData data)=>Data=data;}
        private static void Set(PuzzleController c,string name,object value)=>typeof(PuzzleController).GetField(name,BindingFlags.Instance|BindingFlags.NonPublic).SetValue(c,value);
        private static T Get<T>(PuzzleController c,string name)=>(T)typeof(PuzzleController).GetField(name,BindingFlags.Instance|BindingFlags.NonPublic).GetValue(c);
        private static object Call(PuzzleController c,string name)=>typeof(PuzzleController).GetMethod(name,BindingFlags.Instance|BindingFlags.NonPublic).Invoke(c,null);
        [UnityTest]
        public IEnumerator ControllerCreditsOnlyOnceAndCharges500OnlyForSuccessfulContinue()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);yield return new EnterPlayMode();
            foreach(int level in new[]{21,23,25})
            {
                var host=new GameObject("Economy canvas",typeof(RectTransform),typeof(Canvas));var canvas=host.GetComponent<Canvas>();
                var root=new GameObject("Controller");root.transform.SetParent(host.transform);var c=root.AddComponent<PuzzleController>();c.enabled=false;
                var board=new GameObject("Board",typeof(RectTransform),typeof(GridLayoutGroup),typeof(AdaptiveBoardLayout));board.transform.SetParent(host.transform,false);
                var prefab=Make(null,"Prefab");prefab.gameObject.SetActive(false);
                var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);overlay.SetLevelInfo(level,"green-valley");
                var analytics=new FirebaseAnalyticsService();var memory=new MemorySave();memory.Data.currentLevel=level;memory.Data.highestCompletedLevel=level-1;
                Set(c,"boardRoot",board.transform);Set(c,"cloudPrefab",prefab);Set(c,"_boardLayout",board.GetComponent<AdaptiveBoardLayout>());Set(c,"_overlay",overlay);Set(c,"_worldScreen",WorldScreenController.Create(canvas));
                Set(c,"_analytics",analytics);Set(c,"_adService",new FakeAdService(analytics));Set(c,"_lifeManager",new LifeManager(5));Set(c,"_progression",new ProgressionManager(level,level-1));Set(c,"_modifierRuntime",new ModifierRuntime());Set(c,"_boosterManager",new BoosterManager());Set(c,"_coinWallet",new CoinWallet(499));
                Set(c,"_saveService",memory);Set(c,"_saveData",memory.Data);Set(c,"_feedback",root.AddComponent<GameFeedbackService>());c.LoadLevel(level);
                for(int i=1;i<=100;i++){Get<LevelDefinition>(c,"_currentLevel").levelId=i;Assert.That(((FailOptions)Call(c,"CurrentFailOptions")).CoinCost,Is.EqualTo(500));}
                Get<LevelDefinition>(c,"_currentLevel").levelId=level;Set(c,"_failed",true);Call(c,"ContinueWithCoins");Assert.That(Get<CoinWallet>(c,"_coinWallet").Coins,Is.EqualTo(499));Assert.That(Get<bool>(c,"_failed"),Is.True);
                Set(c,"_coinWallet",new CoinWallet(500));Call(c,"ContinueWithCoins");Assert.That(Get<CoinWallet>(c,"_coinWallet").Coins,Is.Zero);Assert.That(Get<bool>(c,"_failed"),Is.False);Call(c,"ContinueWithCoins");Assert.That(Get<CoinWallet>(c,"_coinWallet").Coins,Is.Zero);
                var clouds=Get<List<CloudContainerView>>(c,"_containers");foreach(var cloud in clouds)cloud.RestoreElements(Array.Empty<WeatherType>());clouds[0].RestoreElements(Enumerable.Repeat(WeatherType.Sun,clouds[0].Capacity));
                Call(c,"CheckWin");int reward=CoinEconomy.RewardFor(level);Assert.That(Get<CoinWallet>(c,"_coinWallet").Coins,Is.EqualTo(reward));Assert.That(memory.Data.coins,Is.EqualTo(reward));Call(c,"CheckWin");Assert.That(Get<CoinWallet>(c,"_coinWallet").Coins,Is.EqualTo(reward));
                Object.Destroy(host);Object.Destroy(prefab.gameObject);yield return null;
            }
            yield return new ExitPlayMode();
        }
        [UnityTest]
        public IEnumerator RealControllerMoveTriggersEveryElementFinishAfterTopEntry()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);yield return new EnterPlayMode();
            int motion=PlayerPrefs.GetInt("Cloudora.ReducedMotion",0);PlayerPrefs.SetInt("Cloudora.ReducedMotion",0);
            var host=new GameObject("Controller finish canvas",typeof(RectTransform),typeof(Canvas));var canvas=host.GetComponent<Canvas>();
            var root=new GameObject("Controller");root.transform.SetParent(host.transform);var c=root.AddComponent<PuzzleController>();c.enabled=false;
            var board=new GameObject("Board",typeof(RectTransform),typeof(GridLayoutGroup),typeof(AdaptiveBoardLayout));board.transform.SetParent(host.transform,false);
            var prefab=Make(null,"Prefab");prefab.gameObject.SetActive(false);
            try
            {
                var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);var analytics=new FirebaseAnalyticsService();var memory=new MemorySave();memory.Data.currentLevel=21;memory.Data.highestCompletedLevel=20;
                Set(c,"boardRoot",board.transform);Set(c,"cloudPrefab",prefab);Set(c,"_boardLayout",board.GetComponent<AdaptiveBoardLayout>());Set(c,"_overlay",overlay);Set(c,"_worldScreen",WorldScreenController.Create(canvas));
                Set(c,"_analytics",analytics);Set(c,"_adService",new FakeAdService(analytics));Set(c,"_lifeManager",new LifeManager(5));Set(c,"_progression",new ProgressionManager(21,20));Set(c,"_modifierRuntime",new ModifierRuntime());Set(c,"_boosterManager",new BoosterManager());Set(c,"_coinWallet",new CoinWallet(500));
                Set(c,"_saveService",memory);Set(c,"_saveData",memory.Data);Set(c,"_feedback",root.AddComponent<GameFeedbackService>());Set(c,"_moveAnimator",root.AddComponent<MoveAnimator>());c.LoadLevel(21);
                var load=typeof(PuzzleController).GetMethod("LoadState",BindingFlags.Instance|BindingFlags.NonPublic);var move=typeof(PuzzleController).GetMethod("PerformMove",BindingFlags.Instance|BindingFlags.NonPublic);
                foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
                {
                    var other=type==WeatherType.Rain?WeatherType.Sun:WeatherType.Rain;
                    int capacity=Get<LevelDefinition>(c,"_currentLevel").capacity;
                    var initial=type==WeatherType.Rainbow?new[]{other,type,type}:Enumerable.Repeat(other,capacity-2).Concat(new[]{type,type}).ToArray();
                    var remainder=Enumerable.Repeat(other,type==WeatherType.Rainbow?capacity-1:2).ToArray();
                    load.Invoke(c,new object[]{new[]{initial,Enumerable.Repeat(type,capacity-2).ToArray(),remainder,Array.Empty<WeatherType>()}});
                    var clouds=Get<List<CloudContainerView>>(c,"_containers");
                    yield return (IEnumerator)move.Invoke(c,new object[]{clouds[0],clouds[1],type,2});
                    var finish=clouds[1].GetComponentInChildren<VesselCompletionFlourish>(true);
                    Assert.That(finish,Is.Not.Null,"Missing finish after a real controller move: "+type);Assert.That(finish.IsPlaying,Is.True);Assert.That(finish.Element,Is.EqualTo(type));
                    Assert.That(clouds[1].IsComplete,Is.True);Assert.That(clouds[1].TryMoveTopGroupTo(clouds[3]),Is.False);
                    finish.RenderAt(.65f);using var mesh=new VertexHelper();var draw=typeof(VesselCompletionFlourish).GetMethod("OnPopulateMesh",BindingFlags.Instance|BindingFlags.NonPublic,null,new[]{typeof(VertexHelper)},null);draw.Invoke(finish,new object[]{mesh});
                    Assert.That(mesh.currentVertCount,Is.GreaterThan(100),"Finish must have visible geometry for "+type);
                    Assert.That(host.GetComponentInChildren<WeatherFlowGraphic>(),Is.Null,"Flight is finished; celebration is separate.");
                }
            }
            finally{PlayerPrefs.SetInt("Cloudora.ReducedMotion",motion);Object.Destroy(host);Object.Destroy(prefab.gameObject);}
            yield return null;yield return new ExitPlayMode();
        }
        private static CloudContainerView Make(Transform parent,string name)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));if(parent)go.transform.SetParent(parent,false);((RectTransform)go.transform).sizeDelta=new Vector2(74,187);var v=go.GetComponent<CloudContainerView>();v.Initialize(4,Array.Empty<WeatherType>(),null);return v;
        }
    }
}
