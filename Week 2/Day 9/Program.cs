using System.Numerics;

string file = "";
#if SAMPLE
file = "sample.txt";
#else
file = "vents.txt";
#endif

string[] lines = File.ReadAllLines(file);
Vector2 dims = new Vector2(lines[0].Length, lines.Length);

Dictionary<Vector2, int> points = new();
for(int z = 0; z < dims.Y; z++)
{
    for(int x = 0; x < dims.X; x++)
    {
        points.Add(new Vector2(x, z), int.Parse(lines[z][x].ToString()));
    }
}

List<Vector2> lowpoints = new();
foreach(var point in points.Keys)
{
    Vector2[] surroundings = new[]
    {
        new Vector2(point.X - 1, point.Y),
        new Vector2(point.X + 1, point.Y),
        new Vector2(point.X, point.Y - 1),
        new Vector2(point.X, point.Y + 1),
    };

    if(surroundings.All(s => points.ContainsKey(s) ? points[s] > points[point] : true))
        lowpoints.Add(point);
}

int danger = lowpoints.Sum(x => points[x] + 1);
Console.WriteLine(danger);
Console.WriteLine();

foreach(var p in points.GroupBy(p => p.Key.Y).OrderBy(x => x.Key))
{
    foreach (var point in p)
    {
        if (lowpoints.Contains(point.Key))
            Console.ForegroundColor = ConsoleColor.Red;
        else
            Console.ForegroundColor = ConsoleColor.White;

        Console.Write(point.Value);
    }

    Console.WriteLine();
}