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

        public Chromosome(int lengthSize)
        {
            ChromosomeList = Enumerable.Range(0, lengthSize)
                .OrderBy(item => Guid.NewGuid())
                .ToList();

            FitnessScore = 0;

            Probability = 1;
        }

        public Chromosome TwoPointCrossover(Chromosome secondParent)
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

        public Chromosome ShallowClone() => MemberwiseClone() as Chromosome;

        public override string ToString() =>
            $"Chromosome: [{string.Join(", ", ChromosomeList)}] Probability: {Probability} Fitness Score {FitnessScore}";

    }
}
