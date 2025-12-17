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
        public int Compare(Individual i1, Individual i2)
        {
            if (i1 == null && i2 == null) return 0;
            if (i1 == null) return 1;
            if (i2 == null) return -1;

            bool i1NuEsteMaiRau =
                i1.Costs <= i2.Costs && i1.WaitingTime <= i2.WaitingTime;

            bool i2NuEsteMaiRau =
                i2.Costs <= i1.Costs && i2.WaitingTime <= i1.WaitingTime;

            bool i1EsteStrictMaiBun =
                i1.Costs < i2.Costs || i1.WaitingTime < i2.WaitingTime;

            bool i2EsteStrictMaiBun =
                i2.Costs < i1.Costs || i2.WaitingTime < i1.WaitingTime;

            if (isNuEsteMaiRau && i1EsteStrictMaiBun) return -1; //i1 domina i2
            if (i2NuEsteMaiRau && i2EsteStrictMaiBun) return 1;  //i2 domina i1

            return 0;
        }
    }
}
