using System;
using System.Collections.Generic;
using EvolutionaryAlgorithm.Domain;
using EvolutionaryAlgorithm.Infrastructure;
using EvolutionaryAlgorithm.NSGAII;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            OptimizationProblem problem = new OptimizationProblem();

            // Date simulate pentru cererea de clienti --> 24 ore
            // 0-6 (Noapte), 6-10 (Dimineata), 10-14 (Pranz), 14-18 (Dupa-amiaza), 18-24 (Seara)
            double[] customerDemand = new double[]
            {
                0, 0, 0, 0, 0, 2,        // 00-06
                5, 10, 15, 20,           // 06-10
                25, 30, 28, 25,          // 10-14
                20, 18, 20, 22,          // 14-18
                25, 20, 15, 10, 5, 2     // 18-24
            };


            IFitnessEvaluator evaluator = new FastFoodFitnessEvaluator(problem, customerDemand);

            DominanceComparer comparer = new DominanceComparer();
            INonDominatedSorterer sorter = new NonDominatedSorter(comparer);


            ISelectionOperator selection = new TournamentSelection(rnd, 2);
            IAritmeticCrossover crossover = new AritmeticCrossover(problem, rnd);
            IUniformMutation mutation = new UniformMutation(problem, rnd);

            NSGAIIRunner runner = new NSGAIIRunner(
                problem,
                rnd,
                selection,
                crossover,
                mutation,
                sorter,
                evaluator
            );

            Console.WriteLine($"Start Optimizare cu: {problem.MaxGeneration} generatii:");
            List<Individual> finalFront = runner.Run();

            IResult resultPrinter = new Result(finalFront);
            resultPrinter.Display();

            Console.WriteLine("\nExecutie finalizata.");
        }
    }
}