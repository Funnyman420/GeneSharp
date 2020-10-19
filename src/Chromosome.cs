using GeneSharp.Extensions;
using GeneSharp.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneSharp
{
    public class Chromosome : IShallowClonable<Chromosome>
    {
        public List<int> ChromosomeList { get; private set; }
        public double FitnessScore { get; set; }
        public double Probability { get; set; }
        public double MutationRate { get; set; }

        private readonly Random _random = new Random();

        /// <summary>
        /// The constructor of the Chromosome object. It creates a
        /// list of <paramref name="lengthSize"/> length and orders it
        /// randomly.
        /// </summary>
        /// <param name="lengthSize">The chromosome's length</param>
        public Chromosome(int lengthSize)
        {
            ChromosomeList = Enumerable.Range(0, lengthSize)
                .OrderBy(item => Guid.NewGuid())
                .ToList();

            FitnessScore = 0;

            Probability = 1;
        }

        /// <summary>
        /// In Genetic Algorithms crossover is a genetic operator used to combine the genetic information
        /// of two parents to generate an new offspring (source: https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm))
        /// 
        /// This is exactly what the function does.
        /// </summary>
        /// <param name="secondParent">The parent that this chromosome is going to "mate" with</param>
        /// <returns></returns>
        public Chromosome TwoPointCrossoverWith(Chromosome secondParent)
        {
            int firstPointCrossover;
            int secondPointCrossover;
            var child = ShallowClone();

            while (true)
            {
                firstPointCrossover = _random.Next(1, child.ChromosomeList.Count);
                secondPointCrossover = _random.Next(1, child.ChromosomeList.Count);

                if (firstPointCrossover < secondPointCrossover)
                    break;
            }

            for (var i = firstPointCrossover; i < secondPointCrossover; i++)
            {
                var indexOfDuplicateGene = FindGene(child, secondParent.ChromosomeList[i]);

                if (i != indexOfDuplicateGene)
                    child.ChromosomeList = child.ChromosomeList.Swap(i, indexOfDuplicateGene).ToList();
            }

            MutationChange(child);

            return child;
        }

        /// <summary>
        /// In Evolutionary Theory, there is a chance that the offspring
        /// of two parents is going to mutate randomly. This simulates that 
        /// behavior by having the <paramref name="subject"/> swap two elements inside its list.
        /// </summary>
        /// <param name="subject">The chromosome to *potentially* be mutated</param>
        public void MutationChange(Chromosome subject)
        {
            int firstPoint;
            int secondPoint;

            if (_random.NextDouble() < MutationRate)
            {
                while (true)
                {
                    firstPoint = _random.Next(0, subject.ChromosomeList.Count - 1);
                    secondPoint = _random.Next(0, subject.ChromosomeList.Count - 1);

                    if (firstPoint != secondPoint)
                        break;
                }

                subject.ChromosomeList = subject.ChromosomeList.Swap(firstPoint, secondPoint).ToList();
            }
        }

        public int FindGene(Chromosome firstParent, int geneValue) =>
             firstParent.ChromosomeList.FindIndex(e => e == geneValue);

        /// <summary>
        /// Shallow Clone behavior
        /// </summary>
        /// <returns>An exact copy of the chromosome, packed with a new pointer</returns>
        public Chromosome ShallowClone() => MemberwiseClone() as Chromosome;

        public override string ToString() =>
            $"Chromosome: [{string.Join(", ", ChromosomeList)}] \nProbability: {Probability} \nFitnessScore: {FitnessScore}\n";

    }
}
