using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;

string input;

#if SAMPLE
input = "sample.txt";
#else
input = "crabs.txt";
#endif

int[] positions = File.ReadAllLines(input).First().Split(",").Select(x => int.Parse(x)).ToArray();

ConcurrentBag<Tuple<int, int>> results = new ConcurrentBag<Tuple<int, int>>();

var posToFuelConverter = new TransformBlock<int, Tuple<int, int[]>>(ConvertToFuel);
var fuelToTotal = new TransformBlock<Tuple<int, int[]>, Tuple<int, int>>((fuel) => Tuple.Create(fuel.Item1, fuel.Item2.Sum()));
var collect = new ActionBlock<Tuple<int, int>>(results.Add);

posToFuelConverter.LinkTo(fuelToTotal, new DataflowLinkOptions { PropagateCompletion = true });
fuelToTotal.LinkTo(collect, new DataflowLinkOptions { PropagateCompletion = true });

foreach (int target in positions.Distinct())
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

Tuple<int, int[]> ConvertToFuel(int target)
{
    // Console.WriteLine("Converting to fuel values: " + target);

    int[] fuel = new int[positions.Length];
    for (int crab = 0; crab < positions.Length; crab++)
        fuel[crab] = Math.Abs(positions[crab] - target);

    return Tuple.Create(target, fuel);
}