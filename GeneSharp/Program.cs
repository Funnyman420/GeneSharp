using GeneSharp.Extensions;
using System;

namespace GeneSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = new Population(5, 5,
                chromosome =>
                {
                    var random = new Random();
                    chromosome.FitnessScore = random.NextDouble(0.0, 1000.0);
                    return chromosome.FitnessScore;
                });

            Console.WriteLine("--Initial Generation--");
            Console.WriteLine(population.ToString());

            for (var i = 0; i < 1000; i++)
            {
                population.PopulationStep();
                Console.WriteLine($"--Generation {i}--");
                Console.WriteLine(population.ToString());
            }
            Console.ReadKey();
        }
    }
}
