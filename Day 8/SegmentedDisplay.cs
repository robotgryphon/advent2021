using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_8
{
    public class SegmentedDisplay
    {
        internal static readonly Dictionary<int, char[]> DigitSegments = new()
        {
            { 0, new char[] { 'a', 'b', 'c', 'e', 'f', 'g' } },
            { 1, new char[] { 'c', 'f' } },
            { 2, new char[] { 'a', 'c', 'd', 'e', 'g' } },
            { 3, new char[] { 'a', 'c', 'd', 'f', 'g' } },
            { 4, new char[] { 'b', 'c', 'd', 'f' } },
            { 5, new char[] { 'a', 'b', 'd', 'f', 'g' } },
            { 6, new char[] { 'a', 'b', 'd', 'e', 'f', 'g' } },
            { 7, new char[] { 'a', 'c', 'f' } },
            { 8, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' } },
            { 9, new char[] { 'a', 'b', 'c', 'd', 'f', 'g' } },
        };

        public static char[] AllSegments => DigitSegments[8];

        public static char[] Unscramble(char[] input)
        {
            switch(input.Length)
            {
                case 2: return DigitSegments[1];
                case 4: return DigitSegments[4];
                case 3: return DigitSegments[7];
                case 8: return DigitSegments[8];
            }

            int[] possible = DigitSegments.Where(ds => ds.Value.Length == input.Length).Select(ds => ds.Key).ToArray();
            return DigitSegments[possible[0]];
        }

        public static void Display(int digit, char[]? remap = null)
        {
            Dictionary<char, char> segments = new();
            if (remap != null)
            {
                for(int i = 0; i < DigitSegments[digit].Length; i++)
                    segments.Add(DigitSegments[digit][i], remap[i]);
            } else
            {
                segments = DigitSegments[digit].ToDictionary(d => d);
            }

            int startX = Console.CursorLeft;
            int startY = Console.CursorTop;

            var o = Console.Out;

            if (segments.ContainsKey('a'))
            {
                WriteHorizontal(o, segments['a']);
                Console.SetCursorPosition(startX, startY);
            }

            foreach(var v1 in Enumerable.Range(startY + 1, 3)) {
                Console.SetCursorPosition(startX, v1);
                o.Write(segments.ContainsKey('b') ? segments['b'] : ' ');
                o.Write("   ");
                o.Write(segments.ContainsKey('c') ? segments['c'] : ' ');
                Console.SetCursorPosition(startX, startY);
            }

            if (segments.ContainsKey('d'))
            {
                Console.SetCursorPosition(startX, startY + 4);
                WriteHorizontal(o, segments['d']);
                Console.SetCursorPosition(startX, startY);
            }

            foreach (var v2 in Enumerable.Range(startY + 5, 3))
            {
                Console.SetCursorPosition(startX, v2);
                o.Write(segments.ContainsKey('e') ? segments['e'] : ' ');
                o.Write("   ");
                o.Write(segments.ContainsKey('f') ? segments['f'] : ' ');
                Console.SetCursorPosition(startX, startY);
            }

            if (segments.ContainsKey('g'))
            {
                Console.SetCursorPosition(startX, startY + 8);
                WriteHorizontal(o, segments['g']);
                Console.SetCursorPosition(startX, startY);
            }
        }

        private static void WriteHorizontal(TextWriter o, char digit)
        {
            o.Write(' ');
            o.Write("".PadLeft(3, digit));
            o.Write(' ');
        }

        internal static char[]? Remap(int digit, Dictionary<char, char> remapped)
        {
            char[] original = DigitSegments[digit];
            char[] map = new char[original.Length];
            for(int op = 0; op < original.Length; op++)
            {
                if (remapped.ContainsKey(original[op]))
                    map[op] = remapped[original[op]];
                else
                    map[op] = ' ';
            }

            return map;
        }
    }
}
