using RobotGryphon.AdventOfCode2021.Day8;
using System.Numerics;

string file = "";
#if SAMPLE
file = "sample.txt";
#else
file = "vents.txt";
#endif

string[] lines = File.ReadAllLines(file);
Vector2 dims = new Vector2(lines[0].Length, lines.Length);

PointMap map = new PointMap(dims);
map.Build(lines);
map.GrowBasins();

var largest3 = map.Basins.OrderByDescending(x => x.NumPoints).Take(3);
int total = 1;
foreach (var basin in largest3)
{
    total *= basin.NumPoints;
    Console.WriteLine(basin.NumPoints);
}

Console.WriteLine();
Console.WriteLine(total);

// int danger = map.ElevationMap.Sum(x => points[x] + 1);
// Console.WriteLine(danger);
Console.WriteLine();

map.Display();