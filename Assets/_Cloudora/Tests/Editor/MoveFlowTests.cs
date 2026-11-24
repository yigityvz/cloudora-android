using System.Collections;
using Cloudora.Puzzle;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Cloudora.Tests.Editor
{
    public class MoveFlowTests
    {
        [UnityTest]
        public IEnumerator FlowCompletesWithTimeScaleZeroAndLeavesCommitToController()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            var root=new GameObject("Flow test canvas",typeof(RectTransform),typeof(Canvas));
            var a=Make(root.transform,"Source",new[]{WeatherType.Rain,WeatherType.Sun,WeatherType.Sun});var b=Make(root.transform,"Target",new WeatherType[0]);
            var position=a.VisualBody.position;var rotation=a.VisualBody.localRotation;var mover=root.AddComponent<MoveAnimator>();Time.timeScale=0;
            var animation=mover.Animate(a,b,WeatherType.Sun,2);
            a.SetSelected(true);
            while(animation.MoveNext())
            {
                Assert.That(a.VisualBody.position,Is.EqualTo(position),"The source glass must remain fixed on every frame.");
                Assert.That(a.VisualBody.localRotation,Is.EqualTo(rotation));
                Assert.That(a.transform.localScale,Is.EqualTo(Vector3.one),"Selection must not resize the vessel.");
                Assert.That(a.ElementCount,Is.EqualTo(3));Assert.That(b.ElementCount,Is.Zero);
                yield return animation.Current;
            }
            Assert.That(a.VisualBody.position,Is.EqualTo(position));Assert.That(a.VisualBody.localRotation,Is.EqualTo(rotation));Assert.That(a.ElementCount,Is.EqualTo(3));Assert.That(b.ElementCount,Is.Zero);Assert.That(a.transform.localRotation,Is.EqualTo(Quaternion.identity));
            yield return null;Assert.That(root.GetComponentInChildren<WeatherFlowGraphic>(),Is.Null);
            Assert.That(a.TryMoveTopGroupTo(b,2),Is.True);Assert.That(a.ElementCount,Is.EqualTo(1));Assert.That(b.ElementCount,Is.EqualTo(2));
            Object.Destroy(root);Time.timeScale=1;yield return new ExitPlayMode();
        }
        private static CloudContainerView Make(Transform parent,string name,WeatherType[] values)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));go.transform.SetParent(parent,false);((RectTransform)go.transform).sizeDelta=new Vector2(90,187);var c=go.GetComponent<CloudContainerView>();c.Initialize(4,values,null);return c;
        }
    }
}
