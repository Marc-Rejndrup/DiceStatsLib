using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class DieRoll
    {
        private Die die;

        public Die Die { get { return die; } set { die = value; CalcProbabilities(); } }

        private int reRoll;

        public int ReRoll { get { return reRoll; } set { reRoll = value; CalcProbabilities(); } }

        private int minRoll;

        public int MinRoll { get { return minRoll; } set { minRoll = value; CalcProbabilities(); } }

        public int MaxValue { get { return Die.MaxValue; } }

        public int MinValue { get { return Die.MinValue; } }

        public int Roll()
        {
            var res = Die.Roll();

            if (res <= ReRoll)
                res = Die.Roll();

            if (res > MinRoll)
                return MinRoll;

            return res;
        }

        public DieRoll(Die die)
        {
            this.Die = die;
        }

        public Dictionary<int, Rational> Probabilities { get; private set; }

        private void CalcProbabilities()
        {
            var probs = Die.Probabilities;

            var quantityReRoll = this.ReRoll - (Die.MinValue - 1);

            if (quantityReRoll > 0)
            {
                var quantityTotal = Die.NumValues;

                Rational probReRoll = new Rational(quantityReRoll, quantityTotal);

                for (var i = Die.MinValue; i <= Die.MaxValue; i++)
                {
                    if (i <= this.ReRoll)
                        probs[i] = probs[i] * probReRoll;

                    else
                    {
                        var res = probs[i] + (probReRoll * probs[i]);

                        probs[i] = res;
                    }
                }
            }
            foreach (var key in probs.Keys)
            {
                if (key < MinRoll)
                {
                    probs[MinRoll] = probs[key];

                    probs.Remove(key);
                }
            }
            this.Probabilities = probs;
        }
    }
}
