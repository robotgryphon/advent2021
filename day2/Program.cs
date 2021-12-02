var lines = await System.IO.File.ReadAllLinesAsync("input.txt");
int forw = 0;
int depth = 0;

foreach(string cmd in lines) {
    int amt = int.Parse(cmd.Split(" ")[1]);
    switch(cmd.Split(" ")[0].ToLower()){
        case "forward": forw += amt; break;
        case "down": depth += amt; break;
        case "up": depth -= amt; break;
    }
}

Console.WriteLine($"Forward ({forw}) * Depth ({depth}) = {forw * depth}");
