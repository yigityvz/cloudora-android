using System.Text.RegularExpressions;
using Cloudora.Services;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    // v3.2: Editor'da tek basışın birden çok işlem yapıp yapmadığını görmek için kanıt (2026-10-15, L30'da 21 Undo).
    public sealed class V32InputTests
    {
        [Test]
        public void AnalyticsLinesCarryFrameAndTime()
        {
            // Aynı karede iki olay görünür olsun: "[Analytics] frame=<kare> t=<sn> <olay> ..."
            LogAssert.Expect(LogType.Log, new Regex(@"^\[Analytics\] frame=\d+ t=\d+\.\d{3} test_event level=7$"));
            new DebugAnalyticsService().Track("test_event", new System.Collections.Generic.Dictionary<string, object> { ["level"] = 7 });
        }

        [Test]
        public void InfoPopupInterceptsInputAndDismissesWithoutUsingHelper()
        {
            var canvasObject=new GameObject("Canvas",typeof(Canvas));int used=0;
            try {
                var overlay=GameplayOverlay.Create(canvasObject.GetComponent<Canvas>(),()=>used++,null,null,null,null,null,null,null,null,null);
                Transform panel=overlay.transform.Find("BlockingPanel");
                overlay.ShowBlock("Notice","Test");
                Assert.That(panel.GetComponent<Image>().raycastTarget,Is.True);
                Assert.That(panel.GetSiblingIndex(),Is.GreaterThan(overlay.transform.Find("Helpers").GetSiblingIndex()));
                panel.GetComponent<Button>().onClick.Invoke();Assert.That(panel.gameObject.activeSelf,Is.False);Assert.That(used,Is.Zero);
            } finally {Object.DestroyImmediate(canvasObject);}
        }

        [Test]
        public void BottomButtonClicksAreLoggedWithFrame()
        {
            var canvasObject = new GameObject("Canvas", typeof(Canvas));
            try
            {
                GameplayOverlay overlay = GameplayOverlay.Create(canvasObject.GetComponent<Canvas>(),
                    () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { });
                Button undo = overlay.transform.Find("Helpers/UndoButton").GetComponent<Button>();
                LogAssert.Expect(LogType.Log, new Regex(@"^\[Input\] button=Undo frame=\d+ t=\d+\.\d{3}$"));
                undo.onClick.Invoke();
            }
            finally
            {
                Object.DestroyImmediate(canvasObject);
            }
        }
    }
}
