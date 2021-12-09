using Day_5;
using System.Numerics;

string[] lines = await File.ReadAllLinesAsync("sample.txt");

DotGrid grid = new DotGrid(0, 0);
foreach (string line in lines)
{
    VentArea area = VentArea.Parse(line);
    foreach (Vector2 point in area.GetDiagonalLines())
    {
        grid.AddPoint(point);
    }
}

int intersections = grid.coverage.Where(x => x.Value > 1).Count();
Console.WriteLine(intersections);

grid.Visualize(Console.Out);