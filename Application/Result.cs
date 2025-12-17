using System;
using System.Collections.Generic;
using System.Linq;
using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.Application
{
    public class Result : IResult
    {
        private readonly List<Individual> _paretoFront;

        public Result(List<Individual> paretoFront)
        {
            _paretoFront = paretoFront;
        }

        public void Display()
        {
            Console.WriteLine("****REZULTATE FINALE NSGA-II");

            if (_paretoFront == null || _paretoFront.Count == 0)
            {
                Console.WriteLine("Nicio soluție găsită.");
                return;
            }

            //solutiile vor fi afisate sortate dupa costuri
            var sortedSolutions = _paretoFront.OrderBy(x => x.Costs).ToList();

            foreach (var ind in sortedSolutions)
            {
                Console.WriteLine($"Cost: {ind.Costs:F2} RON | Penalizare (Asteptare): {ind.WaitingTime:F2}");
                Console.Write("Plan orar: [");
                for (int i = 0; i < ind.Genes.Length; i++)
                {
                    Console.Write($"{(int)ind.Genes[i]}" + (i < ind.Genes.Length - 1 ? "," : ""));
                }
                Console.WriteLine("]");
                Console.WriteLine("----------------------------------------------");
            }
        }
    }
}