using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day_5
{
    /// <summary>
    /// Represents "0,9 -> 5,9"
    /// </summary>
    internal class VentArea
    {
        static Regex matcher = new Regex(@"(?<p1>[\d]+,[\d]+)\s+->\s+(?<p2>[\d]+,[\d]+)");

        internal Vector2 start;
        internal Vector2 end;
        internal int width;
        internal int height;

        public static VentArea? Parse(string input)
        {
            if (!matcher.IsMatch(input))
                return null;

            var match = matcher.Match(input);
            int[] p1 = match.Groups["p1"].Value.Split(",").Select(int.Parse).ToArray();
            int[] p2 = match.Groups["p2"].Value.Split(",").Select(int.Parse).ToArray();

            VentArea result = new VentArea
            {
                start = new Vector2(p1[0], p1[1]),
                end = new Vector2(p2[0], p2[1])
            };

            result.width = (int)(Math.Max(result.start.X, result.end.X) - Math.Min(result.start.X, result.end.X));
            result.height = (int)(Math.Max(result.start.Y, result.end.Y) - Math.Min(result.start.Y, result.end.Y));

            return result;
        }

        internal IEnumerable<Vector2> GetCrossLines()
        {
            List<Vector2> points = new List<Vector2>();

            if (start.X == end.X || start.Y == end.Y)
            {
                if (start.X == end.X)
                    for (float y = Math.Min(start.Y, end.Y); y <= Math.Max(start.Y, end.Y); y++)
                        points.Add(new Vector2(start.X, y));

                if (start.Y == end.Y)
                    for (float x = Math.Min(start.X, end.X); x <= Math.Max(start.X, end.X); x++)
                        points.Add(new Vector2(x, start.Y));
            }

            return points;
        }

        internal IEnumerable<Vector2> GetDiagonalLines()
        {
            List<Vector2> points = new List<Vector2>();

            // points.AddRange(GetCrossLines());

            int xDelta = Math.Sign(end.X - start.X);
            int yDelta = Math.Sign(end.Y - start.Y);

            for (float y = start.Y; y != end.Y; y += yDelta)
            {
                for (float x = start.X; x != end.X; x += xDelta)
                {
                    points.Add(new Vector2(x, y));
                }
            }

            return points;
        }

        internal IEnumerable<Vector2> GetContainedPoints()
        {
            List<Vector2> points = new List<Vector2>();
            for (float x = Math.Min(start.X, end.X); x <= width; x++)
            {
                for (float y = Math.Min(start.Y, end.Y); y <= height; y++)
                {
                    points.Add(new Vector2(x, y));
                }
            }

            return points;
        }
    }
}
