using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.Infrastructure
{
    public interface IFitnessEvaluator
    {
        void Evaluate(Individual individual);
    }
}