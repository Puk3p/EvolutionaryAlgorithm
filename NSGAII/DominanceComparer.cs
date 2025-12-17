using EvolutionaryAlgorithm.Domain;

namespace EvolutionaryAlgorithm.NSGAII
{
    // minimizare pe ambele Costs si WaitingTime
    public class DominanceComparer
    {
        public int Dominates(Individual p, Individual q)
        {
            if (p == null && q == null) return 0;
            if (p == null) return -1;
            if (q == null) return 1;

            bool pNotWorse = (p.Costs <= q.Costs) && (p.WaitingTime <= q.WaitingTime);
            bool qNotWorse = (q.Costs <= p.Costs) && (q.WaitingTime <= p.WaitingTime);

            bool pStrictBetter = (p.Costs < q.Costs) || (p.WaitingTime < q.WaitingTime);
            bool qStrictBetter = (q.Costs < p.Costs) || (q.WaitingTime < p.WaitingTime);

            if (pNotWorse && pStrictBetter) return 1;
            if (qNotWorse && qStrictBetter) return -1;

            return 0;
        }
    }
}
