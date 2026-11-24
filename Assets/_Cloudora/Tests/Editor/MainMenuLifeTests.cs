using System.Linq;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
namespace Cloudora.Tests.Editor
{
    public class MainMenuLifeTests
    {
        [Test]
        public void ZeroLivesOffersARefillAndRegenerationRestoresThePlayAction()
        {
            var host=new GameObject("Canvas",typeof(RectTransform),typeof(Canvas));int refills=0;
            try
            {
                var menu=MainMenuController.Create(host.GetComponent<Canvas>(),"green-valley",21,null,()=>true,_=>{},()=>true,_=>{},()=>refills++);
                menu.SetResources(215,0,"30:00",20);
                var refill=menu.GetComponentsInChildren<Button>().Single(b=>b.GetComponentInChildren<Text>()?.text=="Can Kazan · +1");
                Assert.That(refill.interactable,Is.True);refill.onClick.Invoke();Assert.That(refills,Is.EqualTo(1));
                menu.Show();menu.SetResources(215,1,"29:59",20);
                Assert.That(menu.GetComponentsInChildren<Button>().Any(b=>b.interactable&&b.GetComponentInChildren<Text>()?.text=="Oyna · Bölüm 21"),Is.True);
            }
            finally{Object.DestroyImmediate(host);foreach(var splash in Object.FindObjectsByType<OpeningSplash>(FindObjectsSortMode.None))Object.DestroyImmediate(splash.gameObject);}
        }
    }
}
