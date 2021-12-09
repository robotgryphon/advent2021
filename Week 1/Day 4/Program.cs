using Day_4;

var lines = await File.ReadAllLinesAsync("boards.txt");
var l = new Queue<string>(lines);
HashSet<BingoBoard> boards = new HashSet<BingoBoard>();

Queue<int> calls = new Queue<int>(l.Dequeue().Split(",").Select(x => int.Parse(x)));
l.Dequeue();

List<string> tmp = new List<string>(5);
while (l.Count > 0)
{
    if (string.IsNullOrWhiteSpace(l.Peek()))
    {
        boards.Add(new BingoBoard(tmp));
        l.Dequeue();
        tmp.Clear();
    }
    else
    {
        tmp.Add(l.Dequeue());
    }
}

if(tmp.Count > 0)
{
    boards.Add(new BingoBoard(tmp));
    tmp.Clear();
}

Console.WriteLine("Number boards: " + boards.Count);

var c = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.White;
var fb = boards.First();
fb.Write(Console.Out);
Console.ForegroundColor = c;

Stack<BingoBoard> winners = new Stack<BingoBoard>();
while (calls.Count > 0)
{
    int call = calls.Dequeue();

    // Console.WriteLine("Calling: " + call);

    var callWinners = makeCall(boards, call);
    if (callWinners.Count > 0)
    {
        foreach (var win in callWinners)
        {
            win.Write(Console.Out);
            winners.Push(win);
            boards.Remove(win);
        }
        
        Console.WriteLine("Remaining boards: " + boards.Count);
    }
}

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

c = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Cyan;

BingoBoard board = winners.Peek();

WriteBoardInfo(board);

Console.ForegroundColor = ConsoleColor.Gray;
board.Write(Console.Out);

Console.ForegroundColor = c;

static HashSet<BingoBoard> makeCall(HashSet<BingoBoard> boards, int call)
{
    HashSet<BingoBoard> winners = new HashSet<BingoBoard>();
    foreach (var board in boards)
    {
        bool matched = board.Call(call);
        // board.Write(Console.Out, false);
        // Console.WriteLine();
        if (matched) winners.Add(board); ;
    }

    return winners;
}

static void WriteBoardInfo(BingoBoard board)
{
    int winningCall = board.winningPositions.Last();
    var unmarkedFirst = board.positions.Keys.Except(board.winningPositions);

    Console.WriteLine("Got winner: " + board);
    Console.WriteLine("Won on call: " + winningCall);
    Console.WriteLine("Sum winning positions: " + board.winningPositions.Sum());
    Console.WriteLine("Sum winning unmatched positions: " + unmarkedFirst.Sum());
    Console.WriteLine("Unmarked * Called = " + (unmarkedFirst.Sum() * winningCall));
    Console.WriteLine();
}