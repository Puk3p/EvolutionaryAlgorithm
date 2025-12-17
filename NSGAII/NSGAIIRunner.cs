using System;
using System.Collections.Generic;
using System.Linq;
using EvolutionaryAlgorithm.Domain;
using EvolutionaryAlgorithm.Infrastructure;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.NSGAII
{
    public class NSGAIIRunner
    {
        private readonly OptimizationProblem _problem;
        private readonly Random _random;

        private readonly ISelectionOperator _selection;
        private readonly IAritmeticCrossover _crossover;
        private readonly IUniformMutation _mutation;
        private readonly INonDominatedSorterer _sorter;
        private readonly IFitnessEvaluator _evaluator;

        public NSGAIIRunner(OptimizationProblem problem, Random random,
                            ISelectionOperator selection, IAritmeticCrossover crossover,
                            IUniformMutation mutation, INonDominatedSorterer sorter,
                            IFitnessEvaluator evaluator)
        {
            _problem = problem;
            _random = random;
            _selection = selection;
            _crossover = crossover;
            _mutation = mutation;
            _sorter = sorter;
            _evaluator = evaluator;
        }

        public List<Individual> Run()
        {
            //initializam populatia
            List<Individual> population = new List<Individual>();
            for (int i = 0; i < _problem.PopulationSize; i++)
            {
                var ind = _problem.MakeIndividual();
                _evaluator.Evaluate(ind);
                population.Add(ind);
            }

            for (int gen = 0; gen < _problem.MaxGeneration; gen++)
            {
                //facem copii
                List<Individual> offspring = new List<Individual>();

                while (offspring.Count < _problem.PopulationSize)
                { //aplicam functiile specifice algoritmului evolutiv
                    var parents = _selection.Select(population, 2);

                    var children = _crossover.Crossover(parents[0], parents[1]);

                    foreach (var child in children)
                    {
                        _mutation.Mutate(child);
                        _evaluator.Evaluate(child);
                        offspring.Add(child);
                    }
                }

                //populatie = parinti + copii
                var combinedPopulation = new List<Individual>(population);
                combinedPopulation.AddRange(offspring);

                //fronturile pareto
                var fronts = _sorter.FastNonDominatedSort(combinedPopulation);

                //construirea populatiei rezultat
                population.Clear();
                foreach (var front in fronts)
                {
                    _sorter.CrowdingDistanceAssignment(front);

                    if (population.Count + front.Count <= _problem.PopulationSize)
                    {
                        population.AddRange(front);
                    }
                    else
                    {
                        // Daca nu incape frontul, facem o sortare dupa Crowding Distance  si ii luam pe cei mai buni
                        var sortedFront = front.OrderByDescending(x => x.CrowdingDistance).ToList();
                        int spotsLeft = _problem.PopulationSize - population.Count;
                        population.AddRange(sortedFront.Take(spotsLeft));
                        break;
                    }
                }
            }

            // Returnăm primul front in care se afla cele mai bune solutii
            return _sorter.FastNonDominatedSort(population)[0];
        }
    }
}