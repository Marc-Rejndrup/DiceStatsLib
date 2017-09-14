using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class DiceRange
    {
        //Add base activator

        public int MinActivate { get; set; }

        public int MaxActivate { get; set; }

        public int StartActivate { get; set; }

        public int StopActivate { get; set; }

        public DiceRoll Dice { get; set; }

        public DiceRange(DiceRoll dice) : this(dice, 1, 20) { }

        public DiceRange(DiceRoll dice, int start, int stop) : this(dice, start, stop, 1, 20) { }

        public DiceRange(DiceRoll dice, int start, int stop, int min, int max)
        {
            MinActivate = min;

            MaxActivate = max;

            StartActivate = start;

            StopActivate = stop;

            Dice = dice;
        }

        public ProbabilityDict Result(int i)
        {
            return Result(i, i);
        }
        public ProbabilityDict Result(int i, int j)
        {
            var result = new ProbabilityDict();

            Rational prob = new Rational(1, MaxActivate - MinActivate + 1);

            foreach (var el in Enumerable.Range(i, j - i + 1))
            {
                if (el >= MinActivate && el <= MaxActivate)
                {
                    foreach (var key in Dice.Probabilities.Keys)
                    {
                        result[key] = prob * Dice.Probabilities[key];
                    }
                }
            }

            Rational zeroResult = 1 - result.Values.Sum();

            result[0] = zeroResult;

            return result;
        }

        public bool In (int i)
        {
            return i <= MaxActivate && i >= MinActivate;
        }
    }
}
