using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.NSGAII
{
    //Clasa verifica logica pe baza celor 2 functii obiectiv: F1 --> Costuri si F2 --> Timp de asteptare
    //  1.Se testeaza daca indivisul i1 este cel putin la fel de bun ca i2 la toate obiectivele
    //  2.Se testeaza daca i1 este strict mai bun decat i2 la cel putin unul din obiective
    //  3.Daca ambele conditii 1 si 2 sunt indeplinite, atunci i1 domina pe i2. Daca nu se domina reciproc, ei sunt considerati non-dominanti
    class DominanceComparer
    {
        //in cazul nostru, ambele functii obiectiv sunt de minimizare

        public int Dominates(Individual i1, Individual i2)
        {
            //presupunem initial ca ambii domina
            bool i1_dominates = true;
            bool i2_dominates = true;

            if (i1.Costs > i2.Costs || i1.WaitingTime > i2.WaitingTime)
            {
                i1_dominates = false;

            }
            if (i2.Costs > i1.Costs || i2.WaitingTime > i2.WaitingTime)
            {
                i2_dominates = false;
            }

            bool i1_strictlyBetter = false;
            bool i2_strictlyBetter = false;

            if (i1.Costs < i2.Costs || i1.WaitingTime < i2.WaitingTime)
            {
                i1_strictlyBetter = true;
            }

            if (i2.Costs < i1.Costs || i2.WaitingTime < i2.WaitingTime)
            {
                i2_strictlyBetter = true;
            }

            if (i1_dominates && i1_strictlyBetter)
            {
                return 1; //i1 domina i1
            }

            if (i2_dominates && i2_strictlyBetter)
            {
                return -1; //i1 domina i2
            }
            return 0; //non-dominated


        }

    }
}
