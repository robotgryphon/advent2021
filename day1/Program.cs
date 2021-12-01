// See https://aka.ms/new-console-template for more information
var lines = await System.IO.File.ReadAllLinesAsync("input.txt");
int lastValue = int.Parse(lines[0]);
int more = 0;
for(int l = 1; l < lines.Length; l++) {
    if(int.Parse(lines[l]) > lastValue) more++;
    lastValue = int.Parse(lines[l]);
}

Console.WriteLine(more);
