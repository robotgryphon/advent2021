using System.Collections;
using System.Threading.Tasks.Dataflow;

#if SAMPLE
var lines = await File.ReadAllLinesAsync("sample.txt");
#else
var lines = await File.ReadAllLinesAsync("input.txt");
#endif

int numBits = lines[0].Length;

BitArray gamma = new(numBits);
BitArray epsil = new(numBits);

var convertBitPos = new TransformBlock<int, Tuple<int, bool>>(pos =>
{
    Dictionary<bool, int> counts = new();
    counts.Add(true, 0); counts.Add(false, 0);

    foreach (string line in lines)
        counts[line[pos] == '1']++;

    return Tuple.Create(pos, counts[true] > counts[false]);
});

var broad = new BroadcastBlock<Tuple<int, bool>>(pos => Tuple.Create(pos.Item1, pos.Item2));
var setGamma = new ActionBlock<Tuple<int, bool>>(pair => gamma.Set(pair.Item1, pair.Item2));
var setEpsilon = new ActionBlock<Tuple<int, bool>>(pair => epsil.Set(pair.Item1, !pair.Item2));

var opts = new DataflowLinkOptions { PropagateCompletion = true };
convertBitPos.LinkTo(broad, opts);
broad.LinkTo(setGamma, opts);
broad.LinkTo(setEpsilon, opts);

for (int p = 0; p < numBits; p++)
    convertBitPos.Post(p);

convertBitPos.Complete();

await Task.WhenAll(setGamma.Completion, setEpsilon.Completion);

int gam = c(gamma);
int eps = c(epsil);

Console.WriteLine(gam * eps);

static int c(BitArray b)
{
    int res = 0;
    Span<char> t = new char[b.Count].AsSpan();
    t.Fill('0');

    for (int p = 0; p < b.Count; p++)
        t.Slice(p, 1).Fill(b.Get(p) ? '1' : '0');

    return Convert.ToInt32(new string(t.ToArray()), 2);
}
