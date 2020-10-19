using GeneSharp.Extensions;
using System;

namespace GeneSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = new Population(30, 30,
                chromosome =>
                {
                    var random = new Random();
                    chromosome.FitnessScore = random.NextDouble(0.0, 1000.0);
                    return chromosome.FitnessScore;
                });

            Console.WriteLine("--Initial Generation--");
            Console.WriteLine(population.ToString());

            for (var i = 0; i < 100000; i++)
            {
                population.PopulationStep();
            }
            Console.WriteLine($"--Final Generation--");
            Console.WriteLine(population.ToString());
            Console.ReadKey();
        }
    }
}
