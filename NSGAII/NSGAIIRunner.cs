using EvolutionaryAlgorithm.Domain;
using EvolutionaryAlgorithm.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithm.NSGAII
{
    class NSGAIIRunner
    {
        private readonly OptimizationProblem _problem;
        private readonly IFitnessEvaluator _evaluator;

        private readonly DominanceComparer _dominanceComparer;
        private readonly NonDominatedSorter _sorter;

        private readonly TournamentSelection _tournamentSelection;
        private readonly AritmeticCrossover _crossover;
        private readonly UniformMutation _mutation;

        private readonly Random _random;

        public NSGAIIRunner(OptimizationProblem problem, IFitnessEvaluator evaluator, Random random = null)
        {
            if (problem == null) throw new ArgumentNullException("problem");
            if (evaluator == null) throw new ArgumentNullException("evaluator");

            _problem = problem;
            _evaluator = evaluator;

            _random = random ?? new Random();

            _dominanceComparer = new DominanceComparer();
            _sorter = new NonDominatedSorter(_dominanceComparer);

            _tournamentSelection = new TournamentSelection(_random, 2);
            _crossover = new AritmeticCrossover(_problem, _random);
            _mutation = new UniformMutation(_problem, _random);
        }

        public List<Individual> Run()
        {
            var population = InitializePopulation(_problem.PopulationSize);

            EvaluatePopulation(population);
            AssignRankAndCrowding(population);

            for (int gen = 0; gen < _problem.MaxGeneration; gen++)
            {
                var copii = CreeazaCopii(population, _problem.PopulationSize);
                EvaluatePopulation(copii);

                var combined = population.Concat(copii).ToList();
                var fronts = _sorter.FastNonDominatedSort(combined);

                var nextPop = new List<Individual>(_problem.PopulationSize);

                int frontIndex = 0;
                while (frontIndex < fronts.Count &&
                       nextPop.Count + fronts[frontIndex].Count <= _problem.PopulationSize)
                {
                    var front = fronts[frontIndex];
                    _sorter.CrowdingDistanceAssignment(front);

                    nextPop.AddRange(front);
                    frontIndex++;
                }

                if (nextPop.Count < _problem.PopulationSize && frontIndex < fronts.Count)
                {
                    var lastFront = fronts[frontIndex];
                    _sorter.CrowdingDistanceAssignment(lastFront);

                    int remaining = _problem.PopulationSize - nextPop.Count;

                    nextPop.AddRange(
                        lastFront
                            .OrderByDescending(ind => ind.CrowdingDistance)
                            .Take(remaining)
                    );
                }

                population = nextPop;
                AssignRankAndCrowding(population);
            }

            var finalFronts = _sorter.FastNonDominatedSort(population);
            return finalFronts.Count > 0 ? finalFronts[0] : new List<Individual>();
        }

        private List<Individual> InitializePopulation(int size)
        {
            var pop = new List<Individual>(size);
            for (int i = 0; i < size; i++)
                pop.Add(_problem.MakeIndividual());
            return pop;
        }

        private void EvaluatePopulation(List<Individual> population)
        {
            foreach (var ind in population)
                _evaluator.Evaluate(ind, _problem);
        }

        private void AssignRankAndCrowding(List<Individual> population)
        {
            var fronts = _sorter.FastNonDominatedSort(population);
            foreach (var front in fronts)
                _sorter.CrowdingDistanceAssignment(front);
        }

        private List<Individual> CreeazaCopii(List<Individual> population, int targetSize)
        {
            var children = new List<Individual>(targetSize);

            while (children.Count < targetSize)
            {
                var parents = _tournamentSelection.Select(population, 2);
                var p1 = parents[0];
                var p2 = parents[1];

                var kids = _crossover.Crossover(p1, p2);
                var c1 = kids[0];
                var c2 = kids[1];

                _mutation.Mutate(c1);
                _mutation.Mutate(c2);

                children.Add(c1);
                if (children.Count < targetSize)
                    children.Add(c2);
            }

            return children;
        }
    }
}
