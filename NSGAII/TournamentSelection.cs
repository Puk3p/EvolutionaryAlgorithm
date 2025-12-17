using EvolutionaryAlgorithm.Domain;
using System;
using System.Collections.Generic;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.NSGAII
{
    public class TournamentSelection : ITournamentSelection
    {
        private readonly Random _rnd;
        private readonly int _tournamentSize;

        public TournamentSelection(Random rnd, int tournamentSize = 2)
        {
            _rnd = rnd ?? new Random();
            _tournamentSize = tournamentSize <= 0 ? 2 : tournamentSize;
        }

        public List<Individual> Select(List<Individual> population, int count)
        {
            if (population == null || population.Count == 0)
                throw new ArgumentException("Population is null or empty.");

            if (count <= 0) count = 1;

            var selected = new List<Individual>(count);
            for (int k = 0; k < count; k++)
                selected.Add(RunTournament(population));

            return selected;
        }

        private Individual RunTournament(List<Individual> population)
        {
            Individual best = null;

            for (int i = 0; i < _tournamentSize; i++)
            {
                var candidate = population[_rnd.Next(population.Count)];
                if (candidate == null) continue;

                if (best == null || EsteMaiBun(candidate, best))
                    best = candidate;
            }

            return best ?? population[0];
        }

        private bool EsteMaiBun(Individual a, Individual b)
        {
            if (a.Rank != b.Rank) return a.Rank < b.Rank;
            return a.CrowdingDistance > b.CrowdingDistance;
        }
    }
}
