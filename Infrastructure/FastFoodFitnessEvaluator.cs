using System;
using System.Linq;
using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.Infrastructure
{
    public class FastFoodFitnessEvaluator : IFitnessEvaluator
    {
        private readonly OptimizationProblem _problem;
        private readonly double[] _customerDemand;

        public FastFoodFitnessEvaluator(OptimizationProblem problem, double[] customerDemand)
        {
            _problem = problem;
            _customerDemand = customerDemand;
        }

        public void Evaluate(Individual individual)
        {
            //calcul costuri: Suma Angajatilor * Cost Orar
            individual.Costs = individual.Genes.Sum() * _problem.HourlyCost;

            //calcul Timp Asteptare
            double totalPenalty = 0;
            // pp ca 1 angajat poate servi 3 clienti pe oră
            double serviceRate = 3.0;

            for (int i = 0; i < individual.Genes.Length; i++)
            {
                double demand = _customerDemand[i];
                double allocatedEmployees = individual.Genes[i];
                double capacity = allocatedEmployees * serviceRate;

                //daca capacitatea e mai mica decat cererea, se acumulează penalizare
                if (capacity < demand)
                {
                    double deficit = demand - capacity;
                    //penalizare patratica pentru a descuraja deficitele mari
                    totalPenalty += deficit * deficit;
                }
            }
            individual.WaitingTime = totalPenalty;
        }
    }
}