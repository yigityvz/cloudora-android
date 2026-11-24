using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
namespace Cloudora.Tests.Editor
{
    public class AbandonAttemptTests
    {
        private class MemorySave : ISaveService {public SaveData Data;public SaveData Load()=>Data;public void Save(SaveData d)=>Data=d;}
        private static void Set(object o,string field,object value)=>typeof(PuzzleController).GetField(field,BindingFlags.NonPublic|BindingFlags.Instance).SetValue(o,value);
        private static T Get<T>(object o,string field)=>(T)typeof(PuzzleController).GetField(field,BindingFlags.NonPublic|BindingFlags.Instance).GetValue(o);
        [UnityTest]
        public IEnumerator HomeFromActiveAttemptSpendsOneLifeAndRestoresOriginalBoardButWinIsFree()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            foreach(bool won in new[]{false,true})
            {
                var host=new GameObject("Test canvas",typeof(RectTransform),typeof(Canvas));var canvas=host.GetComponent<Canvas>();
                var root=new GameObject("Controller");root.transform.SetParent(host.transform);var controller=root.AddComponent<PuzzleController>();controller.enabled=false;
                var board=new GameObject("Board",typeof(RectTransform),typeof(GridLayoutGroup),typeof(AdaptiveBoardLayout));board.transform.SetParent(host.transform,false);
                var prefab=new GameObject("Prefab",typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));prefab.SetActive(false);
                var overlay=GameplayOverlay.Create(canvas,null,null,null,null,null,null,null,null,null,null);
                var analytics=new FirebaseAnalyticsService();var save=new MemorySave{Data=new SaveData{currentLevel=21,highestCompletedLevel=20,coins=215}};
                var lives=new LifeManager(5);var progression=new ProgressionManager(21,20);
                Set(controller,"boardRoot",board.transform);Set(controller,"cloudPrefab",prefab.GetComponent<CloudContainerView>());
                Set(controller,"_boardLayout",board.GetComponent<AdaptiveBoardLayout>());Set(controller,"_overlay",overlay);Set(controller,"_worldScreen",WorldScreenController.Create(canvas));
                Set(controller,"_analytics",analytics);Set(controller,"_adService",new FakeAdService(analytics));Set(controller,"_lifeManager",lives);Set(controller,"_progression",progression);Set(controller,"_modifierRuntime",new ModifierRuntime());Set(controller,"_boosterManager",new BoosterManager());Set(controller,"_coinWallet",new CoinWallet(215));
                Set(controller,"_saveService",save);Set(controller,"_saveData",save.Data);Set(controller,"_feedback",root.AddComponent<GameFeedbackService>());
                controller.LoadLevel(21);var clouds=Get<List<CloudContainerView>>(controller,"_containers");clouds[0].RestoreElements(new WeatherType[0]);
                if(won){progression.CompleteCurrentLevel();Set(controller,"_isCompleted",true);}
                typeof(PuzzleController).GetMethod("ReturnHome",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(controller,null);
                Assert.That(lives.Lives,Is.EqualTo(won?5:4));Assert.That(progression.CurrentLevel,Is.EqualTo(won?22:21));Assert.That(save.Data.coins,Is.EqualTo(215));Assert.That(save.Data.failRecord.IsEmpty,Is.True);
                AuthoredLevelCatalog.TryGetBuilt(won?22:21,out var expected);clouds=Get<List<CloudContainerView>>(controller,"_containers");
                Assert.That(clouds.Select(c=>string.Join(",",c.Elements)),Is.EqualTo(expected.CreateBoard().Select(c=>string.Join(",",c))));
                Object.Destroy(host);Object.Destroy(prefab);yield return null;
            }
            yield return new ExitPlayMode();
        }
    }
}
