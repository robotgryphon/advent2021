using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day_5
{
    internal class DotGrid
    {
        public int Width;
        public int Height;
        public Dictionary<Vector2, int> coverage = new Dictionary<Vector2, int>();

        internal void AddPoint(Vector2 point)
        {
            if (!coverage.ContainsKey(point))
                coverage.Add(point, 0);

            coverage[point]++;
            if (point.X > Width)
                Width = (int) point.X;

            if (point.Y > Height)
                Height = (int) point.Y;
        }

        public DotGrid(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        internal void Visualize(TextWriter o)
        {
            var tmp = Console.ForegroundColor;

            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    Vector2 pnt = new Vector2(x, y);
                    if (coverage.ContainsKey(pnt))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        o.Write(coverage[pnt]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        o.Write('.');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                o.WriteLine();
            }

            Console.ForegroundColor = tmp;
        }
    }
}
