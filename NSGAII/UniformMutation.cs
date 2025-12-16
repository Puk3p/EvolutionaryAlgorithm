using EvolutionaryAlgorithm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.NSGAII
{
    public class UniformMutation : IUniformMutation
    {
        private readonly OptimizationProblem _problem;
        private readonly Random _rnd;

        public UniformMutation(OptimizationProblem problem, Random rnd)
        {
            _problem = problem;
            _rnd = rnd;
        }

        public void Mutate(Individual individual)
        {
            for(int i = 0; i < individual.Genes.Length; i++)
            {
                if (_rnd.NextDouble() < _problem.MutationRate)
                {
                    //folosim proprietatile din OptimizationProblem
                    int min = (int)_problem.MinEmployeesPerHour;
                    int max=(int)_problem.MaxEmployeesPerHour;
                    //se genereaza un nr aleatoriu intre min si max si am pus +1 deoarece vrem sa exludem valoarea maxima
                    int newEmployeeCount = _rnd.Next(min, max + 1);
                    //inlocuim gena veche cu cea noua
                    individual.Genes[i] = newEmployeeCount;
                }
            }
        }
    }
}
