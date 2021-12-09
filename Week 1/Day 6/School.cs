using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotGryphon.AdventOfCode2021.Day6
{
    public class School
    {
        public long Fish { get; set; }
        public int[] Lifespans;

        public School(int[] lifespans)
        {
            this.Lifespans = lifespans;
            Fish = lifespans.Length;
        }

        public School Simulate(int days)
        {
            List<long> fishCounts = Enumerable.Range(0, 8 + 1)
                .Select(days => (long) Lifespans.Count(f => f == days))
                .ToList();

            foreach (int day in Enumerable.Range(0, days))
            {
                // the number of fish with 0 days left is now the first element
                long spawning = fishCounts.First();

                // shift all fish down a day (rolling the 0-day fish off the front of the list)
                fishCounts = fishCounts.Skip(1).ToList();

                // all the fish that rolled off the front get added again with 6 days left
                fishCounts[6] += spawning;

                // and contribute that many new fish with 8 days left
                fishCounts.Add(spawning);
            }

            this.Fish = fishCounts.Sum();
            return this;
        }
    }
}
