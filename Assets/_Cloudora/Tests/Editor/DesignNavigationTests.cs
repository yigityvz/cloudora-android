using System.Linq;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.Tests.Editor
{
    public class DesignNavigationTests
    {
        [TestCase(14)] [TestCase(30)] [TestCase(60)]
        public void MilestoneDoesNotAdvanceUntilPlayIsPressed(int level)
        {
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));
            int advances=0,worlds=0;
            try
            {
                var screen=MilestoneScreen.Create(host.transform,level,()=>advances++,()=>worlds++,()=>{});
                screen.SetResources("1.250","5/5","DOLU");
                var discover=screen.GetComponentsInChildren<Button>().Single(b=>b.GetComponentInChildren<Text>()?.text.Contains("Keşfet")==true);
                Assert.That(advances,Is.Zero);
                discover.onClick.Invoke();
                Assert.That(advances,Is.Zero,"A visual transition must not advance or award a second completion.");
                var play=screen.GetComponentsInChildren<Button>().Single(b=>b.GetComponentInChildren<Text>()?.text.StartsWith("Oyna")==true);
                play.onClick.Invoke();Assert.That(advances,Is.EqualTo(1));Assert.That(worlds,Is.Zero);
            }
            finally{Object.DestroyImmediate(host);}
        }
    }
}
