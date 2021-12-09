using Day_8;
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

//int top = Console.CursorTop;
//int start = Console.CursorLeft;
//foreach (int digit in Enumerable.Range(8, 1))
//{
//    SegmentedDisplay.Display(digit, new char[] { 'd', 'e', 'a', 'f', 'g', 'b', 'c' });
//    Console.CursorLeft = start + ((digit + 1) * 7);
//    Console.CursorTop = top;
//}

//Console.CursorTop = start + 9;
//return 0;


int currLine = 0;
foreach (string entry in digits.Take(1))
{
    Dictionary<string, List<int>> guesses = new();
    Dictionary<char, List<char>> remapGuesses = new();
    Dictionary<char, char> remapped = new();

    foreach (char seg in SegmentedDisplay.AllSegments)
        remapGuesses.Add(seg, new List<char>(SegmentedDisplay.AllSegments));

    char[][] segments = entry.Split('|')[0].Split(' ').Select(x => x.ToCharArray()).ToArray();
    string[] outputs = entry.Split('|')[1].Split(' ').Select(x => string.Join("", x.ToCharArray().OrderBy(x => x))).ToArray();

    GetGuessesFromLine(guesses, segments, outputs);

    Dictionary<int, string> known = guesses.Where(x => x.Value.Count == 1).ToDictionary(x => x.Value[0], x => x.Key);

    foreach (string kno in known.Values)
        guesses.Remove(kno);

    foreach (var k in known.Keys)
    {
        var removeableGuesses = guesses.Where(g => g.Value.Contains(k)).Select(k => k.Key);
        foreach (string gind in removeableGuesses)
            guesses[gind].Remove(k);
    }

    if(known.Count < 4)
        Console.WriteLine("Known #:" + known.Count);

    // calculate 2,3,5
    char[][] guesses235 = guesses.Where(x => x.Key.Length == 5).Select(x => x.Key.ToCharArray()).ToArray();
    char[][] guesses069 = guesses.Where(x => x.Key.Length == 6).Select(x => x.Key.ToCharArray()).ToArray();

    // if we don't have 1 we can determine by diffing 4 and 7
    if (!known.ContainsKey(1) && (known.ContainsKey(4) && known.ContainsKey(7)))
    {
        known[1] = known[7].Replace(remapped['a'].ToString(), "");
    }

    // remove all guesses from known right segments, provided by 1
    if (known.ContainsKey(1) && known.ContainsKey(4))
    {
        remapGuesses['b'].RemoveAll(ch => known[1].Contains(ch));
        remapGuesses['d'].RemoveAll(ch => known[1].Contains(ch));
    }

    // top segment
    if (known.ContainsKey(1) && known.ContainsKey(7))
    {
        remapped['a'] = known[7].Except(known[1]).First();
        remapGuesses.Remove('a');
    }

    // determine 9
    if (known.ContainsKey(4))
    {
        var known9 = guesses.Where(x => x.Key.Length == 6).First(x => known[4].All(c => x.Key.Contains(c)));
        known[9] = known9.Key;

        // remove all of 4's segments from 9's segment guesses
        foreach (char s9 in SegmentedDisplay.DigitSegments[9])
            if (remapGuesses.ContainsKey(s9))
                remapGuesses[s9].RemoveAll(guess => known[4].Contains(guess));
    }

    if (known.ContainsKey(4) && known.ContainsKey(7))
    {
        remapped['a'] = known[7].Except(known[4]).First();
        remapGuesses.Remove('a');

        var digitsUsingA = remapGuesses.Where(g => g.Value.Contains('a')).Select(x => x.Key);
        foreach (var b in digitsUsingA)
            remapGuesses[b].Remove(remapped['a']);
    }



    //foreach (int digit in SegmentedDisplay.DigitSegments.Keys.Except(known.Keys))
    //{
    //    remapGuesses[digit].RemoveAll(c => known.Where(x => x.Key != 8).SelectMany(x => x.Value).Contains(c));
    //}

    currLine++;

    //Console.WriteLine();
    //WriteDigitLine(known, remapped);

    Console.WriteLine();
    Console.WriteLine();
    WriteDigitLine(known, remapped);
}

timer.Stop();

static void GetGuessesFromLine(Dictionary<string, List<int>> guesses, char[][] segments, string[] outputs)
{
    for (int gind = 0; gind < segments.Length; gind++)
    {
        char[] group = segments[gind].OrderBy(x => x).ToArray();
        if (group.Length == 0) continue;
        guesses.Add(String.Join("", group), SegmentedDisplay.DigitSegments.Where(seg => seg.Value.Length == group.Length).Select(x => x.Key).ToList());
    }

    for (int gind = 0; gind < outputs.Length; gind++)
    {
        char[] group = outputs[gind].OrderBy(x => x).ToArray();
        if (group.Length == 0) continue;
        string found = String.Join("", group);
        if (!guesses.ContainsKey(found))
            guesses.Add(found, SegmentedDisplay.DigitSegments.Where(seg => seg.Value.Length == group.Length).Select(x => x.Key).ToList());
    }
}

static void WriteDigitLine(Dictionary<int, string> known, Dictionary<char, char> remapped)
{
    int top = Console.CursorTop;
    int start = Console.CursorLeft;
    foreach (int digit in Enumerable.Range(0, 10))
    {
        if (known.ContainsKey(digit))
            SegmentedDisplay.Display(digit, SegmentedDisplay.Remap(digit, remapped));

        Console.CursorLeft = start + ((digit + 1) * 7);
        Console.CursorTop = top;
    }

    Console.CursorTop = top + 9;
    Console.CursorLeft = start;
}