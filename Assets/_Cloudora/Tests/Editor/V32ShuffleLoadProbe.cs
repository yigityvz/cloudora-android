using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Cloudora.Level;
using Cloudora.Puzzle;
using NUnit.Framework;

namespace Cloudora.Tests.Editor
{
    // Faz 2.6 Ã¶lÃ§Ã¼mÃ¼ (2026-10-15 20:36, telefonda 83 + 2 Extra + Shuffle donmasÄ±): oyundaki Shuffle'Ä±n
    // (CloudShuffler + 200.000 durum sÄ±nÄ±rÄ±) 81â€“100 tahtalarÄ±nda 0/1/2 Extra bulutla sÃ¼resi. YalnÄ±zca elle Ã§alÄ±ÅŸtÄ±rÄ±lÄ±r.
    [Explicit("Ã–lÃ§Ã¼m; elle Ã§alÄ±ÅŸtÄ±rÄ±lÄ±r")]
    public sealed class V32ShuffleLoadProbe
    {
        private const int SolverStateLimit = 200000;   // PuzzleController.SolverStateLimit ile aynÄ±

        [Test]
        public void MeasureShuffleOnLargeBoards()
        {
            var csv = new StringBuilder("level,extras,cloud,result,ms,board_solve_status,board_states,board_ms\n");
            foreach (int level in new[] { 81, 83, 87, 90, 100 })
            {
                V3LevelEntry entry = V3LevelData.Find(level);
                for (int extras = 0; extras <= 2; extras++)
                {
                    WeatherType[][] board = entry.CreateBoard().Select(c => c.ToArray())
                        .Concat(Enumerable.Range(0, extras).Select(_ => new WeatherType[0])).ToArray();
                    var sw = Stopwatch.StartNew();
                    PuzzleSolveResult whole = PuzzleSolver.Solve(board, entry.Capacity, SolverStateLimit);
                    long boardMs = sw.ElapsedMilliseconds;
                    for (int cloud = 0; cloud < board.Length; cloud++)
                    {
                        if (!CloudShuffler.CanChange(board[cloud])) continue;
                        sw.Restart();
                        CloudShuffleResult result = CloudShuffler.Shuffle(board, entry.Capacity, cloud, entry.Seed + 997, SolverStateLimit);
                        csv.Append($"{level},{extras},{cloud},{(result.Success ? "ok" : result.Rejection.ToString())},{sw.ElapsedMilliseconds},{whole.Status},{whole.Metrics.ReachableStates},{boardMs}\n");
                        if (sw.ElapsedMilliseconds > 120000) break;   // Ã¶lÃ§Ã¼m sonsuza uzamasÄ±n
                    }
                }
            }
            File.WriteAllText(Path.Combine("Logs", "v32-shuffle-load.csv"), csv.ToString());
        }
    }
}
