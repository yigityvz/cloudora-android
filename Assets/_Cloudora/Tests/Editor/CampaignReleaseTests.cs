using System.Linq;
using Cloudora.Progression;
using Cloudora.Level;
using Cloudora.Puzzle;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.Tests.Editor
{
    public sealed class CampaignReleaseTests
    {
        [Test]
        public void ExactlyThreeWorldsCoverAllPostIntroLevelsThrough100()
        {
            Assert.That(WorldCatalog.All.Count,Is.EqualTo(3));
            Assert.That(WorldCatalog.All.SelectMany(w=>Enumerable.Range(w.FirstLevel,w.LevelCount)),Is.EqualTo(Enumerable.Range(15,86)));
            Assert.That(WorldCatalog.ForLevel(100).Id,Is.EqualTo("frost-peaks"));
            for(int level=16;level<=100;level++){Assert.That(AuthoredLevelCatalog.TryGetBuilt(level,out var board),Is.True);Assert.That(board.worldId,Is.EqualTo(WorldCatalog.ForLevel(level).Id));}
        }
        [Test]
        public void EveryWorldProgressUsesOnlyItsOwnCompletedLevels()
        {
            foreach(var w in WorldCatalog.All)
            {
                Assert.That(RestorationManager.GetProgress(w,w.FirstLevel-1),Is.Zero);
                for(int n=1;n<=w.LevelCount;n++)Assert.That(RestorationManager.GetProgress(w,w.FirstLevel+n-1),Is.EqualTo(n/(float)w.LevelCount).Within(.00001));
                Assert.That(RestorationManager.GetProgress(w,999),Is.EqualTo(1));
            }
            Assert.That(RestorationManager.GetProgress(WorldCatalog.All[0],14),Is.Zero);
        }
        [TestCase(15,"green-valley")][TestCase(30,"green-valley")][TestCase(31,"rainy-coast")][TestCase(60,"rainy-coast")][TestCase(61,"frost-peaks")][TestCase(100,"frost-peaks")]
        public void WorldBoundariesMatchTheApprovedCampaign(int level,string world)=>Assert.That(WorldCatalog.ForLevel(level).Id,Is.EqualTo(world));
        [Test]
        public void FinishedCampaignNeverLoadsOrSavesLevel101()
        {
            var p=new ProgressionManager(100,99);p.CompleteCurrentLevel();p.Advance();
            Assert.That(p.CurrentLevel,Is.EqualTo(100));Assert.That(p.ResumeLevel(true),Is.EqualTo(100));
            var old=new ProgressionManager(400,399);Assert.That(old.CurrentLevel,Is.EqualTo(100));Assert.That(old.HighestCompletedLevel,Is.EqualTo(100));
        }
        [Test]
        public void WinHomeButtonCallsHomeWithoutAwardingOrContinuing()
        {
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));int home=0,next=0;
            try{var o=GameplayOverlay.Create(host.GetComponent<Canvas>(),null,()=>next++,null,null,null,()=>home++,null,null,null,null);o.SetLevelInfo(18,"green-valley");o.ShowComplete(true,10);
            o.GetComponentsInChildren<Button>().Single(b=>b.name=="WinHomeButton").onClick.Invoke();Assert.That(home,Is.EqualTo(1));Assert.That(next,Is.Zero);}
            finally{Object.DestroyImmediate(host);}
        }
        [Test]
        public void TransferPreviewDoesNotMutateBoardAndCanBeCancelled()
        {
            var host=new GameObject("Cloud",typeof(RectTransform),typeof(Image),typeof(Button),typeof(CloudContainerView));
            try{var c=host.GetComponent<CloudContainerView>();c.Initialize(4,new[]{WeatherType.Rain,WeatherType.Sun,WeatherType.Sun},null);
            var before=c.CaptureElements();c.SetTransferPreview(WeatherType.Sun,2,-.6f);Assert.That(c.CaptureElements(),Is.EqualTo(before));c.ClearTransferPreview();Assert.That(c.CaptureElements(),Is.EqualTo(before));
            c.SetTransferPreview(WeatherType.Sun,1,.5f);c.ClearTransferPreview();Assert.That(c.CaptureElements(),Is.EqualTo(before));}
            finally{Object.DestroyImmediate(host);}
        }
    }
}
