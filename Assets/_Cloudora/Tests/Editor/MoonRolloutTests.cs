using System;
using System.Collections.Generic;
using System.Linq;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;
namespace Cloudora.Tests.Editor
{
    public sealed class MoonRolloutTests
    {
        private static IReadOnlyList<V3LevelResult> _baseline;
        private static IReadOnlyList<V3LevelResult> Baseline => _baseline ??= Enum.GetValues(typeof(V3Part)).Cast<V3Part>().SelectMany(V3LevelBuilder.BuildPart).OrderBy(r=>r.Level).ToArray();
        [Test]
        public void MoonIsTheFifthElementFrom60AndNoExistingElementDisappears()
        {
            for(int level=1;level<=100;level++)
            {
                var entry=V3LevelData.Find(level);var pieces=entry.CreateBoard().SelectMany(c=>c).ToArray();
                Assert.That(pieces.Contains(WeatherType.Moon),Is.EqualTo(level>=60),"Moon boundary at "+level);
                if(level<60)continue;
                Assert.That(pieces.Distinct(),Is.EquivalentTo(new[]{WeatherType.Sun,WeatherType.Rain,WeatherType.Snow,WeatherType.Wind,WeatherType.Moon}));
                foreach(var group in pieces.GroupBy(t=>t))Assert.That(group.Count(),Is.EqualTo(entry.Capacity),"conserved element quantity at "+level);
            }
        }
        [Test]
        public void Only60Through80ChangeAndDifficultyFramesAndTrapSlotsStayProtected()
        {
            var updated=CampaignMoonRollout.Apply(Baseline);
            foreach(var before in Baseline)
            {
                var after=updated[before.Level-1];var entry=V3LevelData.Find(before.Level);
                Assert.That(after.Band,Is.EqualTo(before.Band));Assert.That(after.Frame.Capacity,Is.EqualTo(before.Frame.Capacity));Assert.That(after.Frame.EmptyCount,Is.EqualTo(before.Frame.EmptyCount));Assert.That(after.Frame.OpenClouds,Is.EqualTo(before.Frame.OpenClouds));
                if(before.Level<60||before.Level>=81)
                {
                    Assert.That(entry.Board,Is.EqualTo(V3BoardText.Format(before.Primary.Board)),"protected board "+before.Level);Assert.That(entry.Seed,Is.EqualTo(before.Primary.Seed));Assert.That(entry.Score,Is.EqualTo(before.Primary.Score));Assert.That(entry.OptimalMoves,Is.EqualTo(before.Primary.Result.Metrics.OptimalMoves));
                }
                else
                {
                    Assert.That(Math.Abs(entry.Score-before.Primary.Score),Is.LessThanOrEqualTo(CampaignMoonRollout.ScoreTolerance));
                    Assert.That(entry.CreateBoard().Count(c=>c.Contains(WeatherType.Moon)),Is.GreaterThanOrEqualTo(2),"Moon must be playable, not already completed.");
                    Assert.That(entry.OptimalMoves,Is.LessThanOrEqualTo(before.Primary.Result.Metrics.OptimalMoves+4));
                    if(before.Level==60)Assert.That(entry.OptimalMoves,Is.GreaterThanOrEqualTo(before.Primary.Result.Metrics.OptimalMoves));
                    Assert.That(after.Primary.Result.Metrics.OptimalMoves,Is.LessThanOrEqualTo(V3LadderBuilder.LongMovesThreshold(after.Frame)));
                }
            }
            Assert.That(updated.Select(r=>r.Primary.CanonicalKey).Distinct().Count(),Is.EqualTo(100));
            var finale=updated[59].Primary.Result.Metrics;
            Assert.That(finale.NaiveFailureRate+finale.DeadStateRatio,Is.GreaterThan(updated.Where(r=>r.Level>=31&&r.Level<60).Max(r=>r.Primary.Result.Metrics.NaiveFailureRate+r.Primary.Result.Metrics.DeadStateRatio)));
            foreach(var part in updated.Where(r=>r.Level>=31).GroupBy(r=>V3LevelPlan.PartFor(r.Level)))
            {
                Assert.That(part.Count(r=>r.Primary.Result.Metrics.NoMovesDepth>=0),Is.InRange(V3ThemeBuilder.MinNoMoves(part.Key),V3ThemeBuilder.MaxNoMoves(part.Key)));
                Assert.That(part.Count(r=>r.Primary.Result.Metrics.NoMovesDepth==1),Is.LessThanOrEqualTo(part.Key==V3Part.Theme2?1:2));
            }
        }
        [Test]
        public void AllNewBoardsAreSolvableWithCompletedVesselsLockedAndLoadTheirFiveElements()
        {
            for(int level=60;level<=80;level++)
            {
                var entry=V3LevelData.Find(level);Assert.That(AuthoredLevelCatalog.TryGetBuilt(level,out var actual),Is.True);Assert.That(actual.capacity,Is.EqualTo(entry.Capacity));
                Assert.That(PuzzleSolver.CheckSolvable(entry.CreateBoard(),entry.Capacity,V3LevelBuilder.MaxStates),Is.EqualTo(SolveStatus.Solved),"runtime lock at "+level);
            }
        }
        [Test]
        public void PublishedCyclesKeepMediumBelowHardBelowVeryHard()
        {
            foreach(var cycle in Enumerable.Range(1,100).Select(V3LevelData.Find).GroupBy(e=>(e.Level-1)/5))
            {
                float medium=cycle.Where(e=>e.Band==DifficultyBand.Medium).Max(e=>e.Score),hard=cycle.Single(e=>e.Band==DifficultyBand.Hard).Score,veryHard=cycle.Single(e=>e.Band==DifficultyBand.VeryHard).Score;
                Assert.That(medium,Is.LessThan(hard));Assert.That(hard,Is.LessThan(veryHard));
            }
        }
    }
}
