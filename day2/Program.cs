var lines = await System.IO.File.ReadAllLinesAsync("input.txt");
int forw = 0;
int depth = 0;
int aim = 0;

foreach(string cmd in lines) {
    int amt = int.Parse(cmd.Split(" ")[1]);
    switch(cmd.Split(" ")[0].ToLower()){
        case "forward": 
            forw += amt; 
            depth += (aim * amt); 
            break;
        case "down": 
            aim += amt; 
            break;
        case "up": 
            aim -= amt; 
            break;
    }
}

Console.WriteLine($"Forward ({forw}) * Depth ({depth}) = {forw * depth}");
