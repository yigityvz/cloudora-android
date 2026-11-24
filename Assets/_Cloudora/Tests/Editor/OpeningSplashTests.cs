using System.Collections;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Cloudora.Tests.Editor
{
    public sealed class OpeningSplashTests
    {
        [UnityTest]
        public IEnumerator SplashWaitsForReadinessAndReleasesInputWithTimeScaleZero()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(
                UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,
                UnityEditor.SceneManagement.NewSceneMode.Single);
            yield return new EnterPlayMode();
            bool ready = false;
            OpeningSplash splash = OpeningSplash.Create(() => ready, false);
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(3.2f);
            Assert.That(splash != null, Is.True, "Splash must wait for ready content.");
            Assert.That(splash.GetComponent<GraphicRaycaster>().enabled, Is.True);
            Assert.That(splash.transform.Find("InputBlocker").GetComponent<Image>().raycastTarget, Is.True);
            ready = true;
            yield return new WaitForSecondsRealtime(.7f);
            Assert.That(splash == null, Is.True, "Finished overlay must release input even when game time is paused.");
            Time.timeScale = 1f;
            yield return new ExitPlayMode();
        }

        [Test]
        public void ReducedMotionUsesStaticCloudsAndNoGlint()
        {
            OpeningSplash splash = OpeningSplash.Create(() => true, true);
            try
            {
                Transform content = splash.transform.Find("Splash_Content");
                splash.RenderAt(.4f);
                Vector3 cloud = content.Find("Cloud_Front_Left").localPosition;
                Vector3 logo = content.Find("Logo_CLOUDORA").localPosition;
                splash.RenderAt(2f);
                Assert.That(content.Find("Cloud_Front_Left").localPosition, Is.EqualTo(cloud));
                Assert.That(content.Find("Logo_CLOUDORA").localPosition, Is.EqualTo(logo));
                Assert.That(content.Find("Glint").GetComponent<RawImage>().color.a, Is.Zero);
            }
            finally { Object.DestroyImmediate(splash.gameObject); }
        }
    }
}
