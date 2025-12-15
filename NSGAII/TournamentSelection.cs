using EvolutionaryAlgorithm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionaryAlgorithm.NSGAII.Interfaces;

namespace EvolutionaryAlgorithm.NSGAII
{
    public class TournamentSelection : ISelectionOperator
    {
        private readonly Random _rnd;
        private readonly int _tournamentSize;

        public TournamentSelection(Random rnd,int tournamentSize = 2)
        {
            _rnd = rnd;
            _tournamentSize = tournamentSize;
        }
        
        public List<Individual> Select(List<Individual> population, int count)
        {
            List<Individual> selectedParents = new List<Individual>();
            for(int k = 0; k < count; k++)
            {
                selectedParents.Add(RunTournament(population));
            }
            return selectedParents;
        }
        private Individual RunTournament(List<Individual> population)
        {
            Individual bestInd = null;
            for (int i = 0; i < _tournamentSize; i++)
            {
                int randomIndex = _rnd.Next(population.Count);
                Individual currentCandidate = population[randomIndex];

                if (currentCandidate == null)
                {
                    bestInd = currentCandidate;
                    continue;
                }
                if (CrowdedComparison(currentCandidate, bestInd))
                {
                    bestInd = currentCandidate;
                }
            }
            return bestInd;
        }
        private bool CrowdedComparison(Individual i1, Individual i2)
        {
            if (i1.Rank != i2.Rank)
            {
                return i1.Rank < i2.Rank;
            }
            else
            {
                return i1.CrowdingDistance > i2.CrowdingDistance;
            }
        }
    }
}
