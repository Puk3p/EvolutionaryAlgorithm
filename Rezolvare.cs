/**************************************************************************
 *                                                                        *
 *  Copyright:   (c) 2016-2020, Florin Leon                               *
 *  E-mail:      florin.leon@academic.tuiasi.ro                           *
 *  Website:     http://florinleon.byethost24.com/lab_ia.html             *
 *  Description: Evolutionary Algorithms                                  *
 *               (Artificial Intelligence lab 8)                          *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System;

namespace EvolutionaryAlgorithm
{
    /// <summary>
    /// Clasa care reprezinta operatia de selectie
    /// </summary>
    public class Selection
    {
        private static Random _rand = new Random();

        public static Chromosome Tournament(Chromosome[] population)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            int dimensiune = population.Length;

            int index1 = _rand.Next(dimensiune);
            int index2 = _rand.Next(dimensiune);

            while (index2 == index1)
            {
                index2 = _rand.Next(dimensiune);
            }

            Chromosome cromozon1 = population[index1];
            Chromosome cromozon2 = population[index2];


            if (cromozon1.Fitness >= cromozon2.Fitness)
            {
                return cromozon1;
            } else
            {
                return cromozon2;
            }
        }

        public static Chromosome GetBest(Chromosome[] population)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            Chromosome celMaiTopDintreToti = population[0];

            foreach (Chromosome cromozonulNecuviincios in population)
            {
                if (cromozonulNecuviincios.Fitness > celMaiTopDintreToti.Fitness)
                {
                    celMaiTopDintreToti = cromozonulNecuviincios;
                }
            }
            return celMaiTopDintreToti;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de incrucisare
    /// </summary>
    public class Crossover
    {
        private static Random _rand = new Random();

        public static double IaUnNumarRandomTipDouble(double minim, double maxim)
        {
            return _rand.NextDouble() * (maxim - minim) + minim;
        }

        public static Chromosome Arithmetic(Chromosome mother, Chromosome father, double rate)
        {
            Chromosome copil = new Chromosome(mother.Genes.Length, mother.MinValues, mother.MaxValues);
            int numar = _rand.Next(0, 11);
            Chromosome baza;

            if (numar > 5)
            {
                baza = father;
            } else
            {
                baza = mother;
            }

            for (int i = 0; i < copil.Genes.Length; ++i)
            {
                copil.Genes[i] = baza.Genes[i];
            }

            double numarAleator = _rand.NextDouble();

            if (numarAleator < rate)
            {
                double coeficientIncrucisere = IaUnNumarRandomTipDouble(-0.25, 1.25); //asa sxrue


                for (int i = 0; i < copil.Genes.Length; ++i)
                {
                    double genaMama = mother.Genes[i];
                    double genaTata = father.Genes[i];

                    double genaNouNouta = coeficientIncrucisere * genaMama +
                        (1 - coeficientIncrucisere) * genaTata;

                    copil.Genes[i] = genaNouNouta;
                }
            }
            return copil;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta operatia de mutatie
    /// </summary>
    public class Mutation
    {
        private static Random _rand = new Random();

        public static void Reset(Chromosome child, double rate)
        {
            //throw new Exception("Aceasta metoda trebuie implementata");
            for (int i = 0; i < child.Genes.Length; ++i)
            {
                double numarRANDOM = _rand.NextDouble();

                if (numarRANDOM < rate)
                {
                    double minimi = child.MinValues[i];
                    double maximi = child.MaxValues[i];

                    double valNouta = minimi + _rand.NextDouble() * (maximi - minimi);

                    child.Genes[i] = valNouta;
                }
            }
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care implementeaza algoritmul evolutiv pentru optimizare
    /// </summary>
    public class EvolutionaryAlgorithm
    {
        /// <summary>
        /// Metoda de optimizare care gaseste solutia problemei
        /// </summary>
        public Chromosome Solve(IOptimizationProblem p, int populationSize, int maxGenerations, double crossoverRate, double mutationRate)
        {
            //throw new Exception("Aceasta metoda trebuie completata");

            Chromosome[] population = new Chromosome[populationSize];
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = p.MakeChromosome();
                p.ComputeFitness(population[i]);
            }

            for (int gen = 0; gen < maxGenerations; gen++)
            {
                Chromosome[] newPopulation = new Chromosome[populationSize];
                newPopulation[0] = Selection.GetBest(population); // elitism

                for (int i = 1; i < populationSize; i++)
                {
                    // selectare 2 parinti: Selection.Tournament

                    Chromosome parinte1 = Selection.Tournament(population);
                    Chromosome parinte2 = Selection.Tournament(population);

                    // generarea unui copil prin aplicare crossover: Crossover.Arithmetic

                    Chromosome copilas = Crossover.Arithmetic(parinte1, parinte2, mutationRate);

                    // aplicare mutatie asupra copilului: Mutation.Reset

                    Mutation.Reset(copilas, mutationRate);

                    // calculare fitness pentru copil: ComputeFitness din problema p

                    p.ComputeFitness(copilas);

                    // introducere copil in newPopulation

                    newPopulation[i] = copilas;

                }

                for (int i = 0; i < populationSize; i++)
                    population[i] = newPopulation[i];
            }

            return Selection.GetBest(population);
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta problema din prima aplicatie: rezolvarea ecuatiei
    /// </summary>
    public class Equation : IOptimizationProblem
    {
        public Chromosome MakeChromosome()
        {
            // un cromozom are o gena (x) care poate lua valori in intervalul (-5, 5)
            return new Chromosome(1, new double[] { -5 }, new double[] { 5 });
        }

        public void ComputeFitness(Chromosome c)
        {
            //throw new Exception("Aceasta metoda trebuie completata");

            double x = c.Genes[0];
            double valoareEcuatiei = Math.Pow(x, 5) - 5 * x + 5;

            double fitness = -Math.Abs(valoareEcuatiei);

            c.Fitness = fitness;
        }
    }

    //==================================================================================

    /// <summary>
    /// Clasa care reprezinta problema din a doua aplicatie: maximizarea ariei terenului
    /// </summary>
    public class Fence : IOptimizationProblem
    {
        public Chromosome MakeChromosome()
        {
            // un cromozom are doua gene (x si y) care pot lua valori in intervalul (0, 100)
            return new Chromosome(2, new double[] { 0, 0 }, new double[] { 100, 100 });
        }

        public void ComputeFitness(Chromosome c)
        {
            //throw new Exception("Aceasta metoda trebuie completata");

            double x = c.Genes[0];
            double y = c.Genes[1];

            if (2 * x + y <= 0)
            {
                c.Fitness = 0;
                return;
            }

            double r = 100.0 / (2 * x + y);
            x *= r;
            y *= r;

            c.Genes[0] = x;
            c.Genes[1] = y;
            c.Fitness = x * y;


            // c.Fitness = functia care va fi maximizata
        }
    }

    //==================================================================================

    /// <summary>
    /// Programul principal care apeleaza algoritmul
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
            //throw new Exception("Aceasta metoda trebuie completata");

            EvolutionaryAlgorithm ea = new EvolutionaryAlgorithm();
            int dimensiuneaPopulatiei = 30;
            int numarMaximGeneratii = 80;
            double rataIncrucisare = 0.7;
            double rataMutatie = 0.1;



            Chromosome solution = ea.Solve(new Equation(),
                dimensiuneaPopulatiei,
                numarMaximGeneratii,
                rataIncrucisare,
                rataMutatie);



            double x = solution.Genes[0];
            double valEc = Math.Pow(x, 5) - 5 * x + 5;
            double eroare = Math.Abs(valEc);



            // de completat parametrii algoritmului
            //se foloseste -solution.Fitness pentru ca algoritmul evolutiv maximizeaza, iar aici avem o problema de minimizare
            Console.WriteLine("{0:F6} -> {1:F6}", solution.Genes[0], -solution.Fitness);

            solution = ea.Solve(new Fence(), dimensiuneaPopulatiei, numarMaximGeneratii,
                rataIncrucisare, rataIncrucisare); // de completat parametrii algoritmului
            Console.WriteLine("{0:F2} {1:F2} -> {2:F4}", solution.Genes[0], solution.Genes[1], solution.Fitness);
        }
    }
}