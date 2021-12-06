using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day_4
{
    internal class BingoBoard
    {
        private readonly Regex rowReg = new Regex(@"(?<input>[\d]+)");
        internal Dictionary<int, BingoPosition> positions = new Dictionary<int, BingoPosition>();
        internal Stack<int> called = new Stack<int>();
        internal HashSet<int> matchedPositions = new HashSet<int>();

        int firstWonCall = -1;
        internal HashSet<int> winningPositions = new HashSet<int>();

        public BingoBoard(IEnumerable<string> lines)
        {
            int row = 0;
            foreach (string line in lines)
            {
                MatchCollection inputs = rowReg.Matches(line.Trim());
                int[] vals = inputs.Select(m => int.Parse(m.Groups["input"].Value)).ToArray();
                for (int c = 0; c < vals.Length; c++)
                    positions.Add(vals[c], new BingoPosition(c, row));

                row++;
            }
        }

        public bool Call(int num)
        {
            called.Push(num);

            bool didWin = false;
            var matches = positions.Keys.Where(x => x == num).ToArray();
            if (matches.Length == 0)
                return false;

            matchedPositions.Add(num);
            if (matchedPositions.Count < 5) return false;

            for (int col = 0; col < 5; col++)
            {
                int numColMatch = matchedPositions.Where(call => positions[call].Col == col).Count();
                if (numColMatch == 5)
                {
                    if (firstWonCall == -1)
                    {
                        firstWonCall = num;
                        winningPositions = new HashSet<int>(matchedPositions);
                    }

                    didWin = true;
                }
            }

            for (int row = 0; row < 5; row++)
            {
                int numRowMatch = matchedPositions.Where(call => positions[call].Row == row).Count();
                if (numRowMatch == 5)
                {
                    if (firstWonCall == -1)
                    {
                        firstWonCall = num;
                        winningPositions = new HashSet<int>(matchedPositions);
                    }

                    didWin = true;
                }
            }

            return didWin;
        }

        public string Write(TextWriter o, bool showWinningCalls = true)
        {
            var t = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            StringBuilder sb = new();
            for (int r = 0; r < 5; r++) {
                foreach (int p in positions.Where(x => x.Value.Row == r).OrderBy(x => x.Value.Col).Select(x => x.Key))
                {
                    bool m = (showWinningCalls ? winningPositions : matchedPositions).Contains(p);
                    if (m) Console.ForegroundColor = ConsoleColor.Green;
                    if (firstWonCall == p) Console.ForegroundColor = ConsoleColor.Yellow;
                    o.Write(p);
                    o.Write('\t');
                    if (m) Console.ForegroundColor = ConsoleColor.White;

                    sb.Append(p).Append('\t');
                }

                sb.AppendLine();
                o.WriteLine();
            };

            Console.ForegroundColor = t;

            return sb.ToString();
        }

        public override string ToString()
        {
            return "Bingo Board {" + String.Join(",", matchedPositions) + "}";
        }
    }

    internal class BingoPosition
    {
        internal readonly int Col;
        internal readonly int Row;

        public BingoPosition(int col, int row)
        {
            this.Col = col;
            this.Row = row;
        }
    }
}
