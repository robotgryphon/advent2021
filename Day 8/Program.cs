using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

var timer = Stopwatch.StartNew();

string input;

#if SAMPLE
input = "sample.txt";
#else
input = "digits.txt";
#endif

string[] digits = await File.ReadAllLinesAsync(input);

var DigitSegments = new Dictionary<int, char[]>
{
    { 0, new char[] { 'a', 'b', 'c', 'e', 'f', 'g' } },
    { 1, new char[] { 'c', 'f' } },
    { 2, new char[] { 'a', 'c', 'd', 'e', 'g' } },
    { 3, new char[] { 'a', 'c', 'd', 'f', 'g' } },
    { 4, new char[] { 'b', 'c', 'd', 'f' } },
    { 5, new char[] { 'a', 'b', 'd', 'f', 'g' } },
    { 6, new char[] { 'a', 'b', 'd', 'e', 'f', 'g' } },
    { 7, new char[] { 'a', 'c', 'f' } },
    { 8, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
    { 9, new char[] { 'a', 'b', 'c', 'd', 'f', 'g' } },
};

Dictionary<int, int[]> determined = new Dictionary<int, int[]>();
int currLine = 0;
foreach (string entry in digits)
{
    char[][] segments = entry.Split('|')[0].Split(' ').Select(x => x.ToCharArray()).ToArray();
    string[] outputs = entry.Split('|')[1].Split(' ').Select(x => string.Join("", x.ToCharArray().OrderBy(x => x))).ToArray();

    Dictionary<int, List<int>> guesses = new Dictionary<int, List<int>>();
    for (int gind = 0; gind < segments.Length; gind++)
    {
        char[] group = segments[gind];
        guesses.Add(gind, DigitSegments.Where(seg => seg.Value.Length == group.Length).Select(x => x.Key).ToList());
    }

    Dictionary<int, string> known = new Dictionary<int, string>();
    foreach (var guessGroup in guesses)
    {
        if (guessGroup.Value.Count == 1)
        {
            known.Add(guessGroup.Value[0], string.Join("", segments[guessGroup.Key].OrderBy(x => x)));
        }
    }

    Dictionary<int, int> determinedEntry = new();
    foreach (var k in known.Keys)
    {
        int numAppearances = outputs.Count(o => o == known[k]);
        determinedEntry.Add(k, numAppearances);
    }

    var found = string.Join(',', determinedEntry.Where(x => x.Value > 0));
    Console.WriteLine(entry + " = " + found + " = " + determinedEntry.Values.Sum());
    List<int> entries = new List<int>();
    foreach (var d in determinedEntry.Keys)
        for (int i = 0; i < determinedEntry[d]; i++) entries.Add(d);

    determined.Add(currLine, entries.ToArray());
    currLine++;
}

var counts = determined.Select(x => x.Value).Sum(x => x.Length);
Console.WriteLine("All determined: " + counts);