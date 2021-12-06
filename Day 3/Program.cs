using System.Collections;

var lines = await File.ReadAllLinesAsync("input.txt");
int numBits = lines[0].Length;

BitArray[] bits = new BitArray[lines.Length];
BitArray gamma = new(numBits);
BitArray epsil = new(numBits);
for (int lineIndex = 0; lineIndex < bits.Length; lineIndex++)
{
    var ss = lines[lineIndex];
    var arr = new BitArray(numBits);
    for (int bit = 0; bit < numBits; bit++) {
        var v = Convert.ToInt16(ss[bit]) & 1;
        arr.Set(bit, v == 1);
    }

    bits[lineIndex] = arr;
}

for (int bitIndex = 0; bitIndex < numBits; bitIndex++) {
    var gammaLine = bits.Select(x => x.Get(bitIndex)).GroupBy(x => x).OrderByDescending(x => x.Count()).First();
    var epsilLine = bits.Select(x => x.Get(bitIndex)).GroupBy(x => x).OrderBy(x => x.Count()).First();
    gamma.Set(bitIndex, gammaLine.Key);
    epsil.Set(bitIndex, epsilLine.Key);
}

int[] t = new int[1];
gamma.CopyTo(t, 0);
int gam = t[0];

epsil.CopyTo(t, 0);
int eps = t[0];

Console.WriteLine(gam * eps);
