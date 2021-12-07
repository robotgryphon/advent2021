using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

var timer = Stopwatch.StartNew();

string input;

#if SAMPLE
input = "sample.txt";
#else
input = "crabs.txt";
#endif

int[] positions = File.ReadAllLines(input).First().Split(",").Select(x => int.Parse(x)).ToArray();

ConcurrentBag<Tuple<int, int>> results = new ConcurrentBag<Tuple<int, int>>();

var posToFuelConverter = new TransformBlock<int, Tuple<int, int[]>>(ConvertToFuel);
var fuelToTotal = new TransformBlock<Tuple<int, int[]>, Tuple<int, int>>((fuel) =>
{
    Console.WriteLine($"Movement to {fuel.Item1} = {string.Join(',', fuel.Item2)}");
    return Tuple.Create(fuel.Item1, fuel.Item2.Sum());
});

var collect = new ActionBlock<Tuple<int, int>>(results.Add);

posToFuelConverter.LinkTo(fuelToTotal, new DataflowLinkOptions { PropagateCompletion = true });
fuelToTotal.LinkTo(collect, new DataflowLinkOptions { PropagateCompletion = true });

foreach (int target in Enumerable.Range(0, positions.Max()))
    posToFuelConverter.Post(target);

posToFuelConverter.Complete();

await collect.Completion;

Console.WriteLine();
Console.WriteLine();

var res = results.ToArray().OrderBy(r => r.Item2).Take(5);
foreach (var r in res)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(r.Item1);
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(" takes ");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(r.Item2);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(" fuel.");
}

timer.Stop();

Console.WriteLine($"Done in {timer.ElapsedMilliseconds}ms. ({timer.Elapsed.TotalSeconds}s)");

Tuple<int, int[]> ConvertToFuel(int target)
{
    // Console.WriteLine("Converting to fuel values: " + target);

    int[] fuel = new int[positions.Length];
    for (int crab = 0; crab < positions.Length; crab++)
    {
        int numMoves = Math.Abs(positions[crab] - target);
        if (numMoves == 0) fuel[crab] = 0;
        else fuel[crab] = Enumerable.Range(0, numMoves + 1).Sum();
    }

    return Tuple.Create(target, fuel);
}