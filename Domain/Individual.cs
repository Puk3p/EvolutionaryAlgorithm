using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithm.Domain
{
    //clasa entitate
    class Individual
    {
        //proprietati preluate din codul din laborator
        public int NoGenes { get; set; } //numarul de gene
        public double[] Genes { get; set; } //vectorul de gene --> planul de angajati
        public double[] MinValues { get; set; }
        public double[] MaxValues { get; set; }

        //proprietati multi-obiect --> functiile de cost/performanta
        public double Costs { get; set; }
        public double WaitingTime { get; set; }

        //proprietati specifice NSGA II
        public int Rank { get; set; }
        public double CrowdingDistance { get; set; }
        //numarul de indivizi ce domina individul curent p ( in functie de frontul pe care il are)
        public int DominationCount { get; set; }
        //setul de indivizi pe care individul curent p ii domina
        public List<Individual> DominatedSet { get; set; } = new List<Individual>();

        private static Random _rand = new Random();

        public Individual(int noGenes, double[] minValues, double[] maxValues)
        {
            NoGenes = noGenes;
            Genes = new double[noGenes];
            MinValues = new double[noGenes];
            MaxValues = new double[noGenes];

            for (int i=0; i< noGenes; i++)
            {
                MinValues[i] = minValues[i];
                MaxValues[i] = maxValues[i];

                double randomValue = minValues[i] + _rand.NextDouble() * (maxValues[i] - minValues[i]);
                Genes[i] = Math.Round(randomValue);
            }

        }

        public Individual (Individual c)
        {
            NoGenes = c.NoGenes;

            Costs = c.Costs;
            WaitingTime = c.WaitingTime;

            Rank = c.Rank;
            CrowdingDistance = c.CrowdingDistance;
            DominationCount = c.DominationCount;
            //dominatedSet nu trebuie copiat deoarece se recalculeaza la fiecare sortare

            Genes = new double[c.NoGenes];
            MinValues = new double[c.NoGenes];
            MaxValues = new double[c.NoGenes];

            for (int i = 0; i < c.Genes.Length; i++)
            {
                Genes[i] = c.Genes[i];
                MinValues[i] = c.MinValues[i];
                MaxValues[i] = c.MaxValues[i];
            }


        }
        public static Individual Clone(Individual source)
        {
            return new Individual(source);
        }



    }
}
