using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.NSGAII
{
    class NonDominatedSorter: INonDominatedSorterer
    {
        //folosim DominanceComparer pentru a decide relatia dintre indivizi
        private DominanceComparer _comparer;

        public NonDominatedSorter(DominanceComparer comparer)
        {
            _comparer = comparer;
        }

        //folosita pentru a clasifica populatia pe diferite fronturi Pareto
        //frontul 1: solutii nedeominante : cele mai bune
        //frontul 2: dominate doar de solutiile fin F1
        //frontul 3: dominate de F2 si F1 samd.

        public List<List<Individual>> FastNonDominatedSort(List<Individual> population)
        {
            List<List<Individual>> fronts = new List<List<Individual>>();

            //frontul 1 va fi cel mai bun
            List<Individual> F1 = new List<Individual>();

            //daca p domina q --> adaugam q in DominatedSet (pe cine domina p)
            //daca q domina p --> DominatedCount++ (cati inedivizi il domina pe p)

            foreach(var p in population)
            {
                p.DominationCount = 0;
                p.DominatedSet.Clear();

                foreach ( var q in population)
                {
                    if (p == q) continue; //individul nu e comparat cu el insusi

                    int result = _comparer.Dominates(p, q);

                    if(result==1) //p domina q
                    {
                        p.DominatedSet.Add(q);
                    }else if ( result == -1) //q domina p
                    {
                        p.DominationCount++;
                    }
                }

                //indivizii nedominati de nimeni vor fi pusi in Frontul 1
                if (p.DominationCount == 0)
                {
                    p.Rank = 1;
                    F1.Add(p);
                }
            }

            fronts.Add(F1);

            //gasim fronturile urmatoare
            int i = 0; //indexul fontului curent ( 0 pt F1)
            while( fronts[i].Any())
            {
                List<Individual> Q = new List<Individual>(); //Q va fi urmatorul front
                foreach( var p in fronts[i])
                {
                    foreach( var q in p.DominatedSet)
                    {
                        //decrementam DominationCount al lui q
                        q.DominationCount--;

                        //daca q nu mai e dominat de nimeni => se adauga in frontul curent
                        if ( q.DominationCount == 0)
                        {
                            q.Rank = i + 2;
                            Q.Add(q);
                        }
                    }
                }

                i++;
                if (Q.Any())
                    fronts.Add(Q);
                else
                    break;
            }
            return fronts;

            
        }

        public void CrowdingDistanceAssignment(List<Individual> front)
        {
            if(front.Count <= 2)
            {
                foreach( var individual in front)
                {
                    individual.CrowdingDistance = double.PositiveInfinity;
                   
                }
                return;
            }

            foreach( var individual in front)
            {
                individual.CrowdingDistance = 0;

            }

            //calcul pt prima functie obiectiv
            var sortedByCost = front.OrderBy(i => i.Costs).ToList();

            //capetele vor avea avea infinit pt crowdingDistance
            sortedByCost.First().CrowdingDistance = double.PositiveInfinity;
            sortedByCost.Last().CrowdingDistance = double.PositiveInfinity;

            double costMin = sortedByCost.First().Costs;
            double costMax = sortedByCost.Last().Costs;
            double costRange = costMax - costMin;

            if( costRange > 0)
            {
                for(int i= 1; i < sortedByCost.Count-1; i++)
                {
                    sortedByCost[i].CrowdingDistance +=
                        (sortedByCost[i + 1].Costs - sortedByCost[i - 1].Costs) / costRange;
                }
            }

            //calcul pt a doua functie obiectiv
            var sortedByTime = front.OrderBy(i => i.WaitingTime).ToList();

            //setam capetele la infinit (doar daca nu sunt deja setate de mai sus
            if (sortedByTime.First().CrowdingDistance != double.PositiveInfinity)
                sortedByTime.First().CrowdingDistance = double.PositiveInfinity;
            if (sortedByTime.Last().CrowdingDistance != double.PositiveInfinity)
                sortedByTime.Last().CrowdingDistance = double.PositiveInfinity;

            double timeMin = sortedByTime.First().WaitingTime;
            double timeMax = sortedByTime.Last().WaitingTime;
            double timeRange = timeMax - timeMin;

            if (timeRange > 0)
            {
                for ( int i = 1; i< sortedByTime.Count-1; i++)
                {
                    sortedByTime[i].CrowdingDistance +=
                        (sortedByTime[i + 1].WaitingTime - sortedByTime[i - 1].WaitingTime) / timeRange;
                }
            }
        }
    }
}
