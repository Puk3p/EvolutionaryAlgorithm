using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithm.Domain
{
    class OptimizationProblem
    {
        public int PopulationSize { get; set; } = 100;
        public int MaxGeneration { get; set; } = 200;
        public double CrossoverRate { get; set; } = 0.9; //rata de incrucisare
        public double MutationRate { get; set; } = 0.1; //rata de mutatie

        //parametri pt problema de Fast-Food
        public int NumTimeIntervals { get; set; } = 24;
        public double HourlyCost { get; set; } = 15.0;
        public double MaxEmployeesPerHour { get; set; } = 12.0;
        public double MinEmployeesPerHour { get; set; } = 1.0;

        public OptimizationProblem()
        {
            MinValues = new double[NumTimeIntervals];
            MaxValues = new double[NumTimeIntervals];

            for (int i = 0; i < NumTimeIntervals; i++)
            {
                MinValues[i] = MinEmployeesPerHour;
                MaxValues[i] = MaxEmployeesPerHour;
            }

        }
        public double[] MinValues { get; private set; }
        public double[] MaxValues { get; private set; }

        public Individual MakeIndividual()
        {
            return new Individual(NumTimeIntervals, MinValues, MaxValues);
        }
    }
}
