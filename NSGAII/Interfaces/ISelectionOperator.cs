using EvolutionaryAlgorithm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithm.NSGAII.Interfaces
{
    public interface ISelectionOperator
    {
        List<Individual> Select(List<Individual> population, int count);
    }
}
