using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class Die
    {
        private static Random rand = new Random();

        private int minValue;

        public int MinValue {
            get { return minValue; }
            set {
                minValue = value;
                CalcProbabilities();
            }
        }

        private int maxValue;

        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                CalcProbabilities();
            }
        }

        public int NumValues { get { return this.MaxValue - this.MinValue + 1; } }

        public ProbabilityDict Probabilities { get; private set; }

        private void CalcProbabilities()
        {
            Probabilities = new ProbabilityDict();

            for (var i = this.MinValue; i <= this.MaxValue; i++)
            {
                var denom = MaxValue + 1 - MinValue;

                Probabilities[i] = new Rational(1, denom);
            }
        }

        public Die(int min, int max)
        {
            MinValue = min;

            MaxValue = max;
        }

        public Die(int max) : this(1, max) { }

        public Die() : this(20) { }

        public int Roll()
        {
            return rand.Next(MinValue, MaxValue);
        }
    }
}
