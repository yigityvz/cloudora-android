using System;
using System.Collections;
using Cloudora.Puzzle;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    public class ElementMotionTests
    {
        [Test]
        public void EveryElementHasItsOwnStyleAndUnknownElementsHaveSafeFallback()
        {
            var styles=new System.Collections.Generic.HashSet<WeatherMotionStyle>();
            foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
                Assert.That(styles.Add(WeatherMotionProfile.Default(type).Style),Is.True,$"Missing distinct motion for {type}");
            Assert.That(WeatherMotionProfile.Default((WeatherType)99).Style,Is.EqualTo(WeatherMotionStyle.Essence));
        }

        [Test]
        public void FutureElementCanUseAuthoredProfileWithoutChangingAnimator()
        {
            var library=ScriptableObject.CreateInstance<WeatherMotionLibrary>();
            library.Profiles=new[]{new WeatherMotionProfile{Element=(WeatherType)99,Style=WeatherMotionStyle.Snow,FlightSeconds=.85f,ArcHeight=55,SymbolRadius=12,Drift=.7f,OverrideTint=true,Tint=Color.green}};
            var profile=library.Resolve((WeatherType)99);
            Assert.That(profile.Style,Is.EqualTo(WeatherMotionStyle.Snow));Assert.That(profile.Tint,Is.EqualTo(Color.green));
            Assert.That(library.Resolve(WeatherType.Rain).Style,Is.EqualTo(WeatherMotionStyle.Rain));
            Object.DestroyImmediate(library);
        }

        [UnityTest]
        public IEnumerator AllElementsKeepBothVesselsFixedAndCancellationRestoresPreviews()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            int previous=PlayerPrefs.GetInt("Cloudora.ReducedMotion",0);PlayerPrefs.SetInt("Cloudora.ReducedMotion",0);
            var root=new GameObject("Element motion tests",typeof(RectTransform),typeof(Canvas));
            ((RectTransform)root.transform).sizeDelta=new Vector2(390,844);
            var mover=root.AddComponent<MoveAnimator>();
            var a=Make(root.transform,"Source",new Vector2(-120,50));var b=Make(root.transform,"Target",new Vector2(120,-80));
            foreach(WeatherType type in Enum.GetValues(typeof(WeatherType)))
            {
                a.RestoreElements(new[]{type,type,type});b.RestoreElements(Array.Empty<WeatherType>());
                a.SetSelected(true);
                var ap=a.VisualBody.position;var bp=b.VisualBody.position;
                var animation=mover.Animate(a,b,type,2);bool sawFlow=false;
                while(animation.MoveNext())
                {
                    sawFlow|=root.GetComponentInChildren<WeatherFlowGraphic>()!=null;
                    Assert.That(a.VisualBody.position,Is.EqualTo(ap));Assert.That(b.VisualBody.position,Is.EqualTo(bp));
                    Assert.That(a.VisualBody.localRotation,Is.EqualTo(Quaternion.identity));Assert.That(b.VisualBody.localRotation,Is.EqualTo(Quaternion.identity));
                    Assert.That(a.transform.localScale,Is.EqualTo(Vector3.one));Assert.That(b.transform.localScale,Is.EqualTo(Vector3.one));
                    Assert.That(a.ElementCount,Is.EqualTo(3));Assert.That(b.ElementCount,Is.Zero);
                    yield return animation.Current;
                }
                Assert.That(sawFlow,Is.True,type.ToString());
                yield return null;
                Assert.That(!a.transform.Find("Vessel body/Essence regions/Incoming weather") || !a.transform.Find("Vessel body/Essence regions/Incoming weather").gameObject.activeSelf,Is.True);
                Assert.That(!b.transform.Find("Vessel body/Essence regions/Incoming weather") || !b.transform.Find("Vessel body/Essence regions/Incoming weather").gameObject.activeSelf,Is.True);
                yield return null;Assert.That(root.GetComponentInChildren<WeatherFlowGraphic>(),Is.Null);
            }
            ((RectTransform)b.transform).anchoredPosition=new Vector2(-120,-220);
            var interrupted=mover.Animate(a,b,WeatherType.Rainbow,2);
            Assert.That(interrupted.MoveNext(),Is.True);
            var rowFlow=root.GetComponentInChildren<WeatherFlowGraphic>();
            Assert.That(Mathf.Abs(rowFlow.LaneOffset),Is.GreaterThan(0),"Same-column transfers must travel beside the glass instead of retracing through the source.");
            Assert.That(rowFlow.Point(0),Is.EqualTo(rowFlow.From));Assert.That(Vector2.Distance(rowFlow.Point(1),rowFlow.Landing),Is.LessThan(.001f));
            for(int i=0;i<8;i++){Assert.That(interrupted.MoveNext(),Is.True);yield return null;}
            ((IDisposable)interrupted).Dispose();yield return null;
            Assert.That(root.GetComponentInChildren<WeatherFlowGraphic>(),Is.Null);
            Assert.That(!b.transform.Find("Vessel body/Essence regions/Incoming weather") || !b.transform.Find("Vessel body/Essence regions/Incoming weather").gameObject.activeSelf,Is.True);
            var top=(RectTransform)a.transform.Find("Vessel body/Essence regions").GetChild(0);
            Assert.That(top.anchorMax.y,Is.EqualTo(3f/8).Within(.00001f));
            PlayerPrefs.SetInt("Cloudora.ReducedMotion",1);
            var reduced=mover.Animate(a,b,WeatherType.Rainbow,2);
            while(reduced.MoveNext()){Assert.That(root.GetComponentInChildren<WeatherFlowGraphic>(),Is.Null);yield return reduced.Current;}
            PlayerPrefs.SetInt("Cloudora.ReducedMotion",previous);
            Object.Destroy(root);yield return null;yield return new ExitPlayMode();
        }

        private static CloudContainerView Make(Transform parent,string name,Vector2 position)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));go.transform.SetParent(parent,false);
            var r=(RectTransform)go.transform;r.sizeDelta=new Vector2(70,240);r.anchoredPosition=position;
            var v=go.GetComponent<CloudContainerView>();v.Initialize(8,Array.Empty<WeatherType>(),null);return v;
        }
    }
}
