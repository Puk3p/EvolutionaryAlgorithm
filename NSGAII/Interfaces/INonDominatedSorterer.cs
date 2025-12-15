using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EvolutionaryAlgorithm.Domain;


namespace EvolutionaryAlgorithm.NSGAII
{
    interface INonDominatedSorterer
    {
        //sortarea populatiei in Fronturi Pareto (F1, F2...)
        List<List<Individual>> FastNonDominatedSort(List<Individual> population);

        //calcul CrowdingDistance in cadrul unui singur front
        void CrowdingDistanceAssignment(List<Individual> front);
    }
}
