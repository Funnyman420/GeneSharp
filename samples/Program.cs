using GeneSharp.Extensions;
using System;

namespace GeneSharp
{
    class Program
    {
        /// <summary>
        /// This is the most basic test for any genetic algorithm.
        /// 
        /// You create a population and as a fitness function you have
        /// a random number generator setting fitness scores to chromosomes
        /// between 0 and 1000. 
        /// 
        /// If the Genetic Algorithm works the last generation must be approaching
        /// the 1000 number. (e.x. 999.9970231345)
        /// </summary>
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
