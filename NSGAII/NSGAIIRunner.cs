using EvolutionaryAlgorithm.Domain;
using EvolutionaryAlgorithm.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithm.NSGAII
{
    //coordonator de gestionare Parento
    class NSGAIIRunner
    {
        private readonly OptimizationProblem _problem;
        private readonly IFitnessEvaluator _evaluator;

        private readonly NonDominatedSorter _sorter;
        private readonly DominanceComparer _dominanceComparer;

        private readonly TournamentSelection _tournamentSelection;
        private readonly AritmeticCrossover _crossover;
        private readonly UniformMutation _mutation;

        private readonly Random _random;

        public NSGAIIRunner(
            OptimizationProblem problem,
            IFitnessEvaluator evaluator,

            Random? random = null)
        {
            _problem = problem ?? throw new ArgumentNullException(nameof(problem));
            _evaluator = evaluator ?? throw new ArgumentNullException(nameof(evaluator));

            _random = random ?? new Random();

            _dominanceComparer = new DominanceComparer();
            _sorter = new NonDominatedSorter();
            _tournamentSelection = new TournamentSelection(_random);
            _crossover = new AritmeticCrossover(_random);
            _mutation = new UniformMutation(_random);
        }

        public List<Individual> Run()
        {

            //initailziare
            var population = InitializePopulation(_problem.PopulationSize);

            //evaloare rank+crowding
            EvaluatePopulation(population);
            AssignRankAndCrowding(population);

            //generatii
            for (int gen = 0; gen < _problem.MaxGeneration; ++gen)
            {
                var copilasi = CreeazaCopilasi(population, _problem.PopulationSize);

                EvaluatePopulation(copilasi);

                var combinatii = population.Concat(copilasi).ToList();
                var fronturi = _sorter.FastNonDominatedSort(combinatii, _dominanceComparer);

                var urmPopulatie = new List<Individual>(_problem.PopulationSize);

                int frontIndex = 0;

                while (frontIndex < fronturi.Count && urmPopulatie.Count + fronturi[frontIndex].Count
                    <= _problem.PopulationSize)
                {
                    var front = fronturi[frontIndex];
                    _sorter.CrowdingDistanceAssignment(front);
                    ++frontIndex;
                }

                if (urmPopulatie.Count < _problem.PopulationSize && frontIndex < fronturi.Count)
                {
                    var ultimulFront = fronturi[frontIndex];
                    _sorter.CrowdingDistanceAssignment(ultimulFront);

                    int ramase = _problem.PopulationSize - urmPopulatie.Count;

                    urmPopulatie.AddRange(
                        ultimulFront
                        .OrderByDescending(ind => ind.CrowdingDistance)
                        .Take(ramase)
                        );
                }
                population = urmPopulatie;

                AssignRankAndCrowding(population);
            }

            var frontFinal = _sorter.FastNonDominatedSort(population, _dominanceComparer);

            return frontFinal.Count > 0 ? frontFinal[0] : new List<Individual>();
        }

        private List<Individual> InitializePopulation(int marimeaPopulatiei)
        {
            var populatie = new List<Individual>(marimeaPopulatiei);
            for (int i = 0; i < marimeaPopulatiei; ++i)
            {
                populatie.Add(_problem.MakeIndividual());
            }

            return populatie;
        }

        private List<Individual> EvaluatePopulation(List<Individual> population)
        {
            foreach(var individ in population)
            {
                _evaluator.Evaluate(individ, _problem);
            }
        }

        private void AssignRankAndCrowding(List<Individual> population)
        {
            var fronturi = _sorter.FastNonDominatedSort(population, _dominanceComparer);
            foreach (var front in fronturi)
            {
                _sorter.CrowdingDistanceAssignment(front);
            }
        }

        private List<Individual> CreeazaCopilasi(List<Individual> population, int targetSize)
        {
            var children = new List<Individual>(targetSize);

            int tournamentSize = 2;

            while (children.Count < targetSize)
            {
                var p1 = _tournamentSelection.RunTournament(population, tournamentSize);
                var p2 = _tournamentSelection.RunTournament(population, tournamentSize);

                var (c1, c2) = _crossover.Crossover(p1, p2);

                _mutation.Mutate(c1);
                _mutation.Mutate(c2);

                children.Add(c1);

                if (children.Count < targetSize)
                {
                    children.Add(c2);
                }
            }

            return children;
        }
    }
}
