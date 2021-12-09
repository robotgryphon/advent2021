int window = 3;
var lines = (await File.ReadAllLinesAsync("input.txt")).Select(x => int.Parse(x)).ToArray();
int windowStart = 1;
int more = 0;

while(windowStart < lines.Length) {
    if(lines.Skip(windowStart).Take(window).Sum() > lines.Skip(windowStart-1).Take(window).Sum())
        more++;
    windowStart++;
}

Console.WriteLine(more);