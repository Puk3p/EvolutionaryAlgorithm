using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.NSGAII
{
    class AritmeticCrossover: IAritmeticCrossover
    {
        private OptimizationProblem _problem;
        private Random _random;


        public AritmeticCrossover(OptimizationProblem problem, Random random)
        {
            _problem = problem;
            _random = random;
        }

        public List<Individual> Crossover(Individual parent1, Individual parent2)
        {
            var children = new List<Individual>();

            if (_random.NextDouble() >= _problem.CrossoverRate)
            {
                children.Add(Individual.Clone(parent1));
                children.Add(Individual.Clone(parent2));

                return children;
            }

            double alpha = _random.NextDouble();
            int GenesLength = parent1.Genes.Length;

            var child1 = new Individual(GenesLength, parent1.MinValues, parent1.MaxValues);
            var child2 = new Individual(GenesLength, parent2.MinValues, parent2.MaxValues);

            for (int i = 0; i < GenesLength; i++)
            {
                double value1 = alpha * parent1.Genes[i] + (1 - alpha) * parent2.Genes[i];
                double value2 = alpha * parent2.Genes[i] + (1 - alpha) * parent1.Genes[i];

                int min = (int)_problem.MinEmployeesPerHour;
                int max = (int)_problem.MaxEmployeesPerHour;

                //child1.Genes[i] = Math.Clamp((int)Math.Round(value1), min, max);
                //child2.Genes[i] = Math.Clamp((int)Math.Round(value2), min, max);

                //ne asiguram child1 este intre min si max
                int result1 = (int)Math.Round(value1);
                result1 = Math.Min(result1, max);
                result1 = Math.Max(result1, min);
                child1.Genes[i] = result1;

                //ne asiguram child2 este intre min si max
                int result2 = (int)Math.Round(value2);
                result2 = Math.Min(result2, max);
                result2 = Math.Max(result2, min); 
                child2.Genes[i] = result2;
            }

            children.Add(child1);
            children.Add(child2);

            return children;
        }

    }
}
