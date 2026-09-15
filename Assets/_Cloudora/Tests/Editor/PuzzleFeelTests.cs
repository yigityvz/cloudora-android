using Cloudora.Puzzle;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.Tests.Editor
{
    public sealed class PuzzleFeelTests
    {
        [Test]
        public void GroupMoveAndSnapshotRestoreRemainConsistent()
        {
            CloudContainerView source = CreateCloud("Source", WeatherType.Rain, WeatherType.Sun, WeatherType.Sun);
            CloudContainerView target = CreateCloud("Target");
            WeatherType[] snapshot = source.CaptureElements();

            Assert.That(source.TopGroupCount, Is.EqualTo(2));
            Assert.That(source.TryMoveTopGroupTo(target), Is.True);
            Assert.That(source.ElementCount, Is.EqualTo(1));
            Assert.That(target.ElementCount, Is.EqualTo(2));

            source.RestoreElements(snapshot);
            target.RestoreElements(System.Array.Empty<WeatherType>());
            Assert.That(source.CaptureElements(), Is.EqualTo(snapshot));
            Assert.That(target.IsEmpty, Is.True);

            Object.DestroyImmediate(source.gameObject);
            Object.DestroyImmediate(target.gameObject);
        }

        [Test]
        public void InvalidDestinationDoesNotMutateState()
        {
            CloudContainerView source = CreateCloud("Source", WeatherType.Sun);
            CloudContainerView target = CreateCloud("Target", WeatherType.Rain);

            Assert.That(source.TryMoveTopGroupTo(target), Is.False);
            Assert.That(source.CaptureElements(), Is.EqualTo(new[] { WeatherType.Sun }));
            Assert.That(target.CaptureElements(), Is.EqualTo(new[] { WeatherType.Rain }));

            Object.DestroyImmediate(source.gameObject);
            Object.DestroyImmediate(target.gameObject);
        }

        private static CloudContainerView CreateCloud(string name, params WeatherType[] elements)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            CloudContainerView view = go.AddComponent<CloudContainerView>();
            view.Initialize(4, elements, _ => { });
            return view;
        }
    }
}
