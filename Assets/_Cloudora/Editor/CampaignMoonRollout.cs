using System;
using System.Collections.Generic;
using System.Linq;
namespace Cloudora.Level
{
    // Publishing rule only. The tested historical builders, RNG, solver and score formula
    // remain untouched. Refit the 21 affected slots against their original measured scores.
    public static class CampaignMoonRollout
    {
        public const int FirstMoonLevel=60, HistoricalFirstMoonLevel=81;
        public const float ScoreTolerance=V3LadderBuilder.TargetWindow;
        public const int ExpandedPoolCandidates=12000;
        public static IReadOnlyList<V3LevelResult> Apply(IEnumerable<V3LevelResult> original)
        {
            var source=original.OrderBy(r=>r.Level).ToArray();
            if(source.Length!=100||!source.Select(r=>r.Level).SequenceEqual(Enumerable.Range(1,100)))throw new ArgumentException("Publish a complete 100-level campaign.",nameof(original));
            var reserved=new HashSet<string>(source.Where(r=>r.Level<FirstMoonLevel||r.Level>=HistoricalFirstMoonLevel).Select(r=>r.Primary.CanonicalKey));
            var published=new List<V3LevelResult>(100);
            foreach(var slot in source)
            {
                if(slot.Level<FirstMoonLevel||slot.Level>=HistoricalFirstMoonLevel)
                {
                    // The old novelty marker is historical: the board at 81 is unchanged.
                    published.Add(slot.Level==HistoricalFirstMoonLevel?new V3LevelResult(slot.Level,slot.Band,slot.Frame,slot.Window,slot.Primary,slot.Target,V3Novelty.None):slot);
                    continue;
                }
                var frame=new BoardFrame(slot.Frame.Capacity,5,slot.Frame.EmptyCount,slot.Frame.OpenClouds);
                bool finale=slot.Level==60;
                var pool=V3LadderBuilder.Pool(frame);
                // Preserve the already-tested finale's length. A new palette's median would
                // force a longer level despite the user's request to keep its difficulty.
                int longest=Math.Min(V3LadderBuilder.LongMovesThreshold(frame),slot.Primary.Result.Metrics.OptimalMoves+4),minimum=finale?slot.Primary.Result.Metrics.OptimalMoves:0;
                var preceding=published.Where(r=>V3LevelPlan.PartFor(r.Level)==V3LevelPlan.PartFor(slot.Level)).ToArray();
                float previousBand=preceding.Where(r=>r.Band==slot.Band).Select(r=>r.Primary.Score).DefaultIfEmpty(float.MinValue).Max();
                float previousTrap=preceding.Select(r=>r.Primary.Result.Metrics.NaiveFailureRate+r.Primary.Result.Metrics.DeadStateRatio).DefaultIfEmpty(0).Max();
                int cycle=(slot.Level-1)/5;
                var fixedCycle=source.Where(r=>(r.Level-1)/5==cycle&&(r.Level<60||r.Level>=81)).ToArray();
                var sameCycle=preceding.Where(r=>(r.Level-1)/5==cycle).Concat(fixedCycle).ToArray();
                var old=slot.Primary.Result.Metrics;
                var part=V3LevelPlan.PartFor(slot.Level);
                var projected=source.Where(r=>V3LevelPlan.PartFor(r.Level)==part&&r.Level>slot.Level).Concat(preceding).ToArray();
                int projectedDeadEnds=projected.Count(r=>r.Primary.Result.Metrics.NoMovesDepth>=0),projectedImmediate=projected.Count(r=>r.Primary.Result.Metrics.NoMovesDepth==1);
                int projectedBands=0;foreach(var r in projected)if(r.Primary.Result.Metrics.NoMovesDepth>=0)projectedBands|=1<<(int)r.Band;
                bool AllowedDeadEnds(LevelCandidate candidate)
                {
                    var m=candidate.Result.Metrics;int noMoves=projectedDeadEnds+(m.NoMovesDepth>=0?1:0),immediate=projectedImmediate+(m.NoMovesDepth==1?1:0),bands=projectedBands|(m.NoMovesDepth>=0?1<<(int)slot.Band:0);
                    return noMoves>=V3ThemeBuilder.MinNoMoves(part)&&noMoves<=V3ThemeBuilder.MaxNoMoves(part)&&(bands&(bands-1))!=0&&immediate<=(part==V3Part.Theme2?1:2);
                }
                bool Suitable(LevelCandidate c)
                {
                    var m=c.Result.Metrics;
                    if(reserved.Contains(c.CanonicalKey)||Math.Abs(c.Score-slot.Primary.Score)>ScoreTolerance||c.Score<previousBand)return false;
                    if(!AllowedDeadEnds(c))return false;
                    if(m.OptimalMoves>longest||m.OptimalMoves<minimum)return false;
                    if(c.Board.Count(cloud=>cloud.Contains(Cloudora.Puzzle.WeatherType.Moon))<2)return false;
                    if(!finale&&m.NaiveFailureRate>=V3LadderBuilder.MaxNaiveFailureBeforeFinale)return false;
                    if(finale&&m.NaiveFailureRate+m.DeadStateRatio<=previousTrap)return false;
                    if(slot.Band!=DifficultyBand.Medium&&old.DeadStateRatio>.1f&&m.DeadStateRatio<=.1f)return false;
                    foreach(var other in sameCycle)
                    {
                        if(slot.Band>other.Band&&c.Score<=other.Primary.Score)return false;
                        if(slot.Band<other.Band&&c.Score>=other.Primary.Score)return false;
                    }
                    return true;
                }
                LevelCandidate pick=null;
                for(int pass=0;pass<2&&pick==null;pass++)
                {
                    var choices=pass==0?pool:V3LadderBuilder.Pool(frame,ExpandedPoolCandidates);
                    var candidates=choices.Where(Suitable).OrderBy(c=>Math.Abs(c.Score-slot.Primary.Score)).ThenBy(c=>Math.Abs(c.Result.Metrics.OptimalMoves-old.OptimalMoves)).ThenBy(c=>c.Seed);
                    foreach(var candidate in candidates)
                    {
                        // Runtime rules lock completed vessels. Historical scoring never bypasses
                        // this separate publish-time reachability check.
                        if(PuzzleSolver.CheckSolvable(candidate.Board,frame.Capacity,V3LevelBuilder.MaxStates)!=SolveStatus.Solved)continue;
                        pick=candidate;break;
                    }
                }
                if(pick==null)
                {
                    var diagnostics=new List<string>{"projected="+string.Join(";",projected.Select(r=>r.Level+":"+r.Primary.Result.Metrics.NoMovesDepth)),"median="+minimum+",long="+longest+",previousTrap="+previousTrap,"seed,score,moves,noMoves,trap,quota"};
                    foreach(var c in V3LadderBuilder.Pool(frame,ExpandedPoolCandidates).Where(c=>Math.Abs(c.Score-slot.Primary.Score)<=ScoreTolerance))diagnostics.Add($"{c.Seed},{c.Score:R},{c.Result.Metrics.OptimalMoves},{c.Result.Metrics.NoMovesDepth},{c.Result.Metrics.DeadStateRatio+c.Result.Metrics.NaiveFailureRate:R},{AllowedDeadEnds(c)}");
                    System.IO.File.WriteAllLines($"Logs/moon-rollout-20261019/reject-{slot.Level}.csv",diagnostics);
                }
                if(pick==null)throw new InvalidOperationException($"Moon rollout L{slot.Level}: no verified {frame} within {ScoreTolerance} of score {slot.Primary.Score:R}; refusing to change the difficulty curve.");
                reserved.Add(pick.CanonicalKey);
                published.Add(new V3LevelResult(slot.Level,slot.Band,frame,new ScoreWindow(slot.Primary.Score-ScoreTolerance,slot.Primary.Score+ScoreTolerance),pick,slot.Target,slot.Level==FirstMoonLevel?V3Novelty.NewColor:slot.Novelty));
            }
            Validate(source,published);
            return published;
        }
        private static void Validate(V3LevelResult[] original,List<V3LevelResult> published)
        {
            if(published.Select(r=>r.Primary.CanonicalKey).Distinct().Count()!=100)throw new InvalidOperationException("Moon campaign contains duplicate boards.");
            foreach(var row in published)
            {
                var before=original[row.Level-1];
                if(row.Band!=before.Band||row.Frame.Capacity!=before.Frame.Capacity||row.Frame.EmptyCount!=before.Frame.EmptyCount||row.Frame.OpenClouds!=before.Frame.OpenClouds)throw new InvalidOperationException("Moon rollout changed a protected frame.");
                if(row.Level<60||row.Level>=81)
                    if(row.Primary.Seed!=before.Primary.Seed||row.Primary.CanonicalKey!=before.Primary.CanonicalKey||row.Primary.Score!=before.Primary.Score)throw new InvalidOperationException("Moon rollout changed an unaffected level.");
            }
            foreach(var cycle in published.GroupBy(r=>(r.Level-1)/5))
            {
                float medium=cycle.Where(r=>r.Band==DifficultyBand.Medium).Max(r=>r.Primary.Score),hard=cycle.Single(r=>r.Band==DifficultyBand.Hard).Primary.Score,veryHard=cycle.Single(r=>r.Band==DifficultyBand.VeryHard).Primary.Score;
                if(!(medium<hard&&hard<veryHard))throw new InvalidOperationException("Moon rollout violated a difficulty cycle.");
            }
            foreach(var part in published.Where(r=>r.Level>=31).GroupBy(r=>V3LevelPlan.PartFor(r.Level)))
            {
                int deadEnds=part.Count(r=>r.Primary.Result.Metrics.NoMovesDepth>=0),immediate=part.Count(r=>r.Primary.Result.Metrics.NoMovesDepth==1);
                if(deadEnds<V3ThemeBuilder.MinNoMoves(part.Key)||deadEnds>V3ThemeBuilder.MaxNoMoves(part.Key)||immediate>(part.Key==V3Part.Theme2?1:2)||part.Where(r=>r.Primary.Result.Metrics.NoMovesDepth>=0).Select(r=>r.Band).Distinct().Count()<2)throw new InvalidOperationException("Moon rollout violated existing No Moves quotas.");
            }
            foreach(var part in published.GroupBy(r=>V3LevelPlan.PartFor(r.Level)))foreach(var band in part.GroupBy(r=>r.Band))
            {
                float last=float.MinValue;
                foreach(var row in band.OrderBy(r=>r.Level)){if(row.Primary.Score<last)throw new InvalidOperationException("Moon rollout made a later band easier.");last=row.Primary.Score;}
            }
        }
    }
}
