using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

#if SAMPLE
var inputLines = await File.ReadAllLinesAsync("sample.txt");
#else
var inputLines = await File.ReadAllLinesAsync("input.txt");
#endif

int numBits = inputLines[0].Length;

BitArray gamma = new(numBits);
BitArray epsil = new(numBits);
BitArray oxy = new(numBits);
BitArray co2 = new(numBits);

var convertBitPos = new TransformBlock<int, Tuple<int, bool>>(pos =>
{
    var counts = GetPosCounts(pos, inputLines);
    return Tuple.Create(pos, counts[true] >= counts[false]);
});

var broad = new BroadcastBlock<Tuple<int, bool>>(pos => Tuple.Create(pos.Item1, pos.Item2));
var setGamma = new ActionBlock<Tuple<int, bool>>(pair => gamma.Set(pair.Item1, pair.Item2));
var setEpsilon = new ActionBlock<Tuple<int, bool>>(pair => epsil.Set(pair.Item1, !pair.Item2));

var makeOxy = new ActionBlock<Tuple<int, bool>>((pair) => CondenseByPosition(oxy, true));
var makeCO2 = new ActionBlock<Tuple<int, bool>>((pair) => CondenseByPosition(co2, false));


var opts = new DataflowLinkOptions { PropagateCompletion = true };
convertBitPos.LinkTo(broad, opts);
broad.LinkTo(setGamma, opts);
broad.LinkTo(setEpsilon, opts);
broad.LinkTo(makeOxy, opts, pair => pair.Item1 == 0);
broad.LinkTo(makeCO2, opts, pair => pair.Item1 == 0);

for (int p = 0; p < numBits; p++)
    convertBitPos.Post(p);

convertBitPos.Complete();

await Task.WhenAll(setGamma.Completion, setEpsilon.Completion, makeOxy.Completion, makeCO2.Completion);

int gam = c(gamma);
int eps = c(epsil);
int oxyint = c(oxy);
int co2int = c(co2);

Console.WriteLine("Part 1: " + gam * eps);
Console.WriteLine("Oxy: " + oxyint);
Console.WriteLine("CO2: " + co2int);

static Dictionary<bool, int> GetPosCounts(int pos, string[] lines)
{
    Dictionary<bool, int> counts = new();
    foreach (string line in lines)
    {
        bool set = line[pos] == '1';
        if (!counts.ContainsKey(set)) counts.Add(set, 0);
        counts[set]++;
    }

    return counts;
}

void CondenseByPosition(BitArray target, bool oxyMode)
{
    var survived = inputLines;
    for (int i = 0; i < target.Count; i++)
    {
        // todo - fix counts here, ugh

        Dictionary<bool, int> bitCount = GetPosCounts(i, survived);
        bool countsMatch = bitCount.Count == 1 || bitCount[true] == bitCount[false];
        bool setBit;

        if (oxyMode)
            setBit = countsMatch ? true : bitCount.MaxBy(x => x.Value).Key;
        else
        {
            if (countsMatch && bitCount.Count == 1)
                setBit = bitCount.First().Key;
            else
                setBit = countsMatch ? false : bitCount.MinBy(x => x.Value).Key;
        }

        (oxyMode ? oxy : co2).Set(i, setBit);

        if (survived.Length == 1)
            continue;

        survived = survived.Where(x => x[i] == (setBit ? '1' : '0')).ToArray();
    }
}

static int c(BitArray b)
{
    Span<char> t = new char[b.Count].AsSpan();
    t.Fill('0');

    for (int p = 0; p < b.Count; p++)
        t.Slice(p, 1).Fill(b.Get(p) ? '1' : '0');

    return Convert.ToInt32(new string(t.ToArray()), 2);
}
