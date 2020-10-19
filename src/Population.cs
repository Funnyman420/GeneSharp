using GeneSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GeneSharp
{
    public class Population
    {
        public double BornRate { get; set; } = 0.3;
        public double ParentRate { get; set; } = 0.8;
        public double FitnessSum { get; private set; } = 0;
        public List<Chromosome> PopulationList { get; private set; }
        public Func<Chromosome, double> FitnessFunction { get; private set; }

        private readonly Random _random = new Random();


        public Population(
            int populationSize,
            int lengthOfChromosome,
            Func<Chromosome, double> fitnessFuntion,
            double mutationRate = 0.03)
        {
            FitnessFunction = fitnessFuntion;

            PopulationList = Enumerable
                .Range(0, populationSize)
                .Select(e =>
                {
                    var toBeAdded = new Chromosome(lengthOfChromosome)
                    {
                        MutationRate = mutationRate
                    };

                    ApplyFitness(toBeAdded);

                    FitnessSum += toBeAdded.FitnessScore;

                    return toBeAdded;
                })
                .OrderByDescending(c => c.FitnessScore)
                .ToList();

        }

        private Chromosome ParentSelection()
        {
            var populationSelection = ParentRate * FitnessSum;
            var sumTillThreshold = 0.0;
            var threshold = _random.NextDouble(0, populationSelection);
            Chromosome selectedParent = null;

            foreach (var chromosome in PopulationList)
            {
                if (sumTillThreshold <= threshold)
                    sumTillThreshold += chromosome.FitnessScore;
                else
                {
                    selectedParent = chromosome;
                    break;
                }
            }

            return selectedParent ?? throw new Exception("Parent not found");
        }

        public void PopulationStep()
        {
            Chromosome firstParent;
            Chromosome secondParent;
            Chromosome child;
            var recycledChromosomes = Convert.ToInt32(PopulationList.Count * BornRate);

            for (var i = 0; i < recycledChromosomes; i++)
            {
                firstParent = ParentSelection();
                secondParent = ParentSelection();
                child = firstParent.TwoPointCrossover(secondParent);
                AddToPopulation(child);
            }

            for (var i = 0; i < recycledChromosomes; i++)
                KillChromosome();
        }

        private int BinarySearch(Chromosome subject)
        {
            int mid;
            var low = 0;
            var high = PopulationList.Count;

            while (low < high)
            {
                mid = (low + high) / 2;
                if (PopulationList[mid].FitnessScore > subject.FitnessScore)
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }

        private void AddToPopulation(Chromosome subject)
        {
            ApplyFitness(subject);
            FitnessSum += subject.FitnessScore;
            PopulationList.Insert(BinarySearch(subject), subject);
        }

        private void ApplyFitness(Chromosome subject) =>
            subject.FitnessScore = FitnessFunction(subject);


        private void ComputeProbabilities()
        {
            foreach (var chromosome in PopulationList)
            {
                chromosome.Probability = chromosome.FitnessScore / FitnessSum;
            }
        }


        private void KillChromosome()
        {
            var killedChromosome = PopulationList.Last();
            FitnessSum -= killedChromosome.FitnessScore;
            PopulationList.Remove(killedChromosome);
        }

        public override string ToString()
        {
            ComputeProbabilities();
            return string.Join("\n", PopulationList.Select(chromosome => chromosome.ToString()));
        }
    }
}
