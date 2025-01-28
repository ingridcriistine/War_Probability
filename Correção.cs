using System.Diagnostics;
using System.Collections.Concurrent;

var sw = new Stopwatch();
sw.Start();
var result = monteCarlo(1_000_000, 1707, 1000);
sw.Stop();
Console.WriteLine($"Result: {result} in {sw.ElapsedMilliseconds} ms");

float monteCarlo(int N, int atk, int def)
{
    const int SizeLimit = 1000;
    if (N < SizeLimit)
    {
        var sim = new WarSimulation(atk, def, (int)DateTime.Now.Ticks);
        return sim.MonteCarlo(N);
    }
    
    int threads = N / SizeLimit;
    List<WarSimulation> simulations = [];
    for (int i = 0; i < threads; i++)
        simulations.Add(new WarSimulation(atk, def, Random.Shared.Next()));

    ConcurrentQueue<float> probs = [];
    Parallel.For(0, threads, i =>
    {
        var sim = simulations[i];
        var prob = sim.MonteCarlo(SizeLimit);
        probs.Enqueue(prob);
    });

    return probs.Average();
}

public class WarSimulation(int atk, int def, int seed)
{
    readonly Random random = new(seed);
    readonly int baseAtk = atk;
    readonly int baseDef = def;

    /// <summary>
    /// Use MonteCarlo algorithm to evaluate attack win
    /// probability.
    /// </summary>
    public float MonteCarlo(int N)
    {
        int atkWin = 0;
        for (int i = 0; i < N; i++)
        {
            if (Simulate())
                atkWin++;
        }
        return atkWin / (float)N;
    }
    
    /// <summary>
    /// Make a simulation of a war and returns true if
    /// attack wins.
    /// </summary>
    public bool Simulate()
    {
        int atk = baseAtk;
        int def = baseDef;
        while (atk > 1 && def > 0)
            Battle(ref atk, ref def);
        
        return atk > 1;
    }

    /// <summary>
    /// Make a subbattle.
    /// </summary>
    void Battle(ref int atk, ref int def)
    {
        int atkSize = int.Min(baseAtk - 1, 3);
        int defSize = int.Min(baseDef, 3);

        var atkDices = Roll(atkSize);
        var defDices = Roll(defSize);

        int battleSize = int.Min(atkSize, defSize);
        for (int i = 0; i < battleSize; i++)
        {
            var atkDice = atkDices[i];
            var defDice = defDices[i];

            if (atkDice > defDice) def--;
            else atk--;
        }
    }

    /// <summary>
    /// Roll and order 'n' dices with six sides.
    /// </summary>
    int[] Roll(int n)
    {
        var result = new int[n];
        for (int i = 0; i < result.Length; i++)
            result[i] = Roll();
        Array.Sort(result, (a, b) => b - a);
        return result;
    }
    
    /// <summary>
    /// Roll a dice with six sides.
    /// </summary>
    int Roll() => random.Next(6);
}