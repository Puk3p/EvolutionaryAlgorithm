using EvolutionaryAlgorithm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.NSGAII
{
    public class TournamentSelection : ITournamentSelection
    {
        private readonly Random _random;

        public TournamentSelection(Random? random = null)
        {
            _random = random ?? new Random();
        }

        public Individual RunTournament(List<Individual> populatie, int marimeTurneu)
        {
            if (populatie == null || populatie.Count == 0)
                throw new ArgumentException("Populatia nu poate fi nula sau goala.");

            if (marimeTurneu <= 0) marimeTurneu = 1;

            Individual celMaiTop = null;

            for (int i = 0; i < marimeTurneu; ++i)
            {
                var candidat = populatie[_random.Next(0, populatie.Count)];

                if (candidat == null) continue;

                if (celMaiTop == null || EsteMaiBun(candidat, celMaiTop))
                {
                    celMaiTop = candidat;
                }
            }

            return celMaiTop ?? populatie[0];
        }

        public bool EsteMaiBun(Individual i1, Individual i2)
        {
            if (i2 == null) return true;
            if (i1 == null) return false;

            if (i1.Rank < i2.Rank) return true;
            if (i1.Rank > i2.Rank) return false;

            return i1.CrowdingDistance > i2.CrowdingDistance;
        }
    }
}
