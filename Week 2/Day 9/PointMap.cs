using System.Collections.Concurrent;
using System.Numerics;

namespace RobotGryphon.AdventOfCode2021.Day8
{
    public class PointMap
    {
        private readonly Vector2 Dimensions;
        public Dictionary<Vector2, int> ElevationMap = new();

        public HashSet<Basin> Basins = new();

        public PointMap(Vector2 dims)
        {
            this.Dimensions = dims;
        }
        public void Build(string[] lines)
        {
            ElevationMap.Clear();

            for (int z = 0; z < Dimensions.Y; z++)
            {
                for (int x = 0; x < Dimensions.X; x++)
                {
                    ElevationMap.Add(new Vector2(x, z), int.Parse(lines[z][x].ToString()));
                }
            }

            List<Vector2> lowpoints = new();
            foreach (var point in ElevationMap.Keys)
            {
                var surroundings = GetSurroundingPoints(point);
                if (surroundings.All(s => ElevationMap[s] > ElevationMap[point]))
                    Basins.Add(new Basin(this, point));
            }
        }

        public IEnumerable<Vector2> GetSurroundingPoints(Vector2 point)
        {
            var points = new[] {
                    new Vector2(point.X - 1, point.Y),
                    new Vector2(point.X + 1, point.Y),
                    new Vector2(point.X, point.Y - 1),
                    new Vector2(point.X, point.Y + 1),
                };

            return points.Where(p => ElevationMap.ContainsKey(p));
        }

        public void GrowBasins()
        {
            ConcurrentBag<Basin> b = new(Basins);
            Parallel.ForEach(Basins, basin =>
            {
                bool canStillGrow = true;
                while(canStillGrow)
                    canStillGrow = !basin.Grow();
            });
        }

        public void Display()
        {
            var c = Console.ForegroundColor;
            for(int y = 0; y < Dimensions.Y; y++)
            {
                for(int x = 0; x < Dimensions.X;x++)
                {
                    Vector2 point = new(x, y);
                    if (Basins.Any(b => b.Containment.Contains(point)))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(ElevationMap[point]);
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = c;
        }
    }
}