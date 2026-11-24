using System.Linq;
using Cloudora.Core;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;
namespace Cloudora.Tests.Editor
{
    public class ReachabilityRegressionTests
    {
        private static SolveStatus Finish(PuzzleSolver.ReachabilitySearch search)
        {
            for(int i=0;i<10000;i++){var status=search.Step(512,1000);if(status!=SolveStatus.LimitExceeded)return status;}
            Assert.Fail("Search failed to finish within the generous test budget.");return SolveStatus.LimitExceeded;
        }
        [Test]
        public void ScreenshotYellowCycleIsALossDespiteLegalMoves()
        {
            var board=V3BoardText.Parse("RRR/NNN/NRSSS/SRNS");var state=new PuzzleState(board.Select(c=>new CloudState(5,c)));
            Assert.That(PuzzleRules.TryApply(state,new Move(2,3,1,WeatherType.Sun)),Is.True);
            Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(board,5)),Is.EqualTo(SolveStatus.Unsolvable));
            Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(state.Clouds.Select(c=>c.Elements.ToArray()).ToArray(),5)),Is.EqualTo(SolveStatus.Unsolvable));
        }
        [Test]
        public void ExtraVesselRescuesTheScreenshotAndCannotReuseADeadCacheEntry()
        {
            var dead=V3BoardText.Parse("RRR/NNN/NRSSS/SRNS");
            var rescued=dead.Concat(new[]{new WeatherType[0]}).ToArray();
            Assert.That(PuzzleSolver.ReachabilityKey(dead,5),Is.Not.EqualTo(PuzzleSolver.ReachabilityKey(rescued,5)));
            Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(dead,5)),Is.EqualTo(SolveStatus.Unsolvable));
            Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(rescued,5)),Is.EqualTo(SolveStatus.Solved));
        }
        [Test]
        public void ExhaustedFrameBudgetNeverMeansLost()
        {
            var board=AuthoredLevelCatalog.Get(1);var search=new PuzzleSolver.ReachabilitySearch(board.CreateBoard(),board.capacity);
            Assert.That(search.Step(0),Is.EqualTo(SolveStatus.LimitExceeded));Assert.That(Finish(search),Is.EqualTo(SolveStatus.Solved));
        }
        [Test]
        public void All100StartingBoardsHaveSolutionsUnderRuntimeRules()
        {
            for(int i=1;i<=100;i++)
            {
                LevelDefinition level;if(i<=AuthoredLevelCatalog.Count)level=AuthoredLevelCatalog.Get(i);else{Assert.That(AuthoredLevelCatalog.TryGetBuilt(i,out level),Is.True);}
                Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(level.CreateBoard(),level.capacity)),Is.EqualTo(SolveStatus.Solved),"level "+i);
            }
        }
        [Test]
        public void IncrementalSearchAgreesWithIndependentFullGraphAndPermutation()
        {
            var frame=new BoardFrame(4,3,1);
            for(int seed=1;seed<=12;seed++)
            {
                var board=LevelCandidateFactory.Deal(frame,seed);var exact=PuzzleSolver.Solve(board,4,300000).Status;
                Assert.That(exact,Is.Not.EqualTo(SolveStatus.LimitExceeded));
                Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(board,4)),Is.EqualTo(exact));
                Assert.That(Finish(new PuzzleSolver.ReachabilitySearch(board.Reverse().ToArray(),4)),Is.EqualTo(exact));
            }
        }
    }
}
