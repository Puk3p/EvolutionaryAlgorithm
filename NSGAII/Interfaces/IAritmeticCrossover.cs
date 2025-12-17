using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EvolutionaryAlgorithm.Domain;


namespace EvolutionaryAlgorithm.NSGAII
{
    public interface IAritmeticCrossover
    {
        //va primi 2 parinti si returneaza o lista cu 2 descendenti
        List<Individual> Crossover(Individual parent1, Individual parent2);
    }
}
