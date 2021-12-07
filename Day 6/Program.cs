
using RobotGryphon.AdventOfCode2021.Day6;

string[] fishIn = await File.ReadAllLinesAsync("lanternfish.txt");
int[] fishInitial = fishIn[0].Split(",").Select(int.Parse).ToArray();

School school = new School(fishInitial);
school.Simulate(256);
Console.WriteLine("Amount of fish after 256 days: " + school.Fish);