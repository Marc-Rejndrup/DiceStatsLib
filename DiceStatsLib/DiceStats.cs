using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public static class ProbabilityUtils
    {
        public static Rational Average(ProbabilityDict probabilities)
        {
            var avg = new Rational(0, 1);

            foreach (var outcome in probabilities.Keys)
            {
                avg += outcome * probabilities[outcome];
            }

            return avg;
        }

        public static ProbabilityDict CalcImprovementOfReRoll(ProbabilityDict probabilities)
        {
            var improvementProb = new ProbabilityDict();

            for (var i = 1; i < probabilities.Keys.Max() - (probabilities.Keys.Min() - 1); i++)
            {
                improvementProb[i] = 0;
            }
            foreach (var a in probabilities.Keys)
            {
                foreach (var b in probabilities.Keys)
                {
                    if (b > a)
                        improvementProb[b - a] += probabilities[a] * probabilities[b];
                }
            }
            return improvementProb;
        }

        public static ProbabilityDict BestOf(ProbabilityDict one, ProbabilityDict two)
        {
            var probabilities = new ProbabilityDict();

            for (var i = Math.Max(one.Keys.Min(), two.Keys.Min()) ; i < Math.Max(one.Keys.Max(), two.Keys.Max()); i++)
            {
                probabilities[i] = 0;
            }

            foreach (var a in one.Keys)
            {
                foreach (var b in two.Keys)
                {
                    probabilities[Math.Max(a, b)] += probabilities[a] * probabilities[b];
                }
            }

            return probabilities;
        }

        public static ProbabilityDict Advantage(ProbabilityDict one)
        {
            return BestOf(one, one);
        }

        public static ProbabilityDict WorstOf(ProbabilityDict one, ProbabilityDict two)
        {
            var probabilities = new ProbabilityDict();

            for (var i = Math.Min(one.Keys.Min(), two.Keys.Min()); i < Math.Min(one.Keys.Max(), two.Keys.Max()); i++)
            {
                probabilities[i] = 0;
            }

            foreach (var a in one.Keys)
            {
                foreach (var b in two.Keys)
                {
                    probabilities[Math.Min(a, b)] += probabilities[a] * probabilities[b];
                }
            }

            return probabilities;
        }

        public static ProbabilityDict Disadvantage(ProbabilityDict one)
        {
            return WorstOf(one, one);
        }

        public static Rational SumProbabilities(ProbabilityDict probabilities)
        {
            var sum = new Rational(0, 1);

            foreach (var key in probabilities.Keys)
            {
                sum += probabilities[key];
            }
            return sum;
        }

        public static Rational AtLeast(this ProbabilityDict probabilities, int result)
        {
            var sum = new Rational(0, 1);

            foreach (var key in probabilities.Keys)
            {
                if (key >= result)
                    sum += probabilities[key];
            }
            return sum;
        }
    }
}