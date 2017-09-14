using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public interface DiceRoll
    {
        ProbabilityDict Probabilities { get; }

        List<DiceRoll> Rolls { get; set; }

        int Roll();

        int MinValue { get; }

        int MaxValue { get; }
    }

    public abstract class DiceRollBase
    {
        public ProbabilityDict Probabilities { get; protected set; }

        protected List<DiceRoll> rolls;

        public List<DiceRoll> Rolls { get { return rolls; } set { rolls = value; CalcProbabilities(); } }

        public int Roll()
        {
            var result = 0;

            foreach (var die in Rolls)
            {
                result += die.Roll();
            }

            return result;
        }

        protected abstract void CalcProbabilities();

        public int MinValue
        {
            get {
                if(this.GetType() == typeof(LoneDiceRoll))
                {
                    return (rolls.First() as LoneDiceRoll).LoneRoll.MinValue;
                }
                else if (this.GetType() == typeof(WorstDiceRoll))
                {
                    return Enumerable.Min(rolls.Select(x => x.MinValue));
                }
                else if (this.GetType() == typeof(BestDiceRoll))
                {
                    return Enumerable.Max(rolls.Select(x => x.MinValue));
                }
                else //SumDiceRoll
                {
                    return Enumerable.Sum(rolls.Select(x => x.MinValue));
                }
            }
        }

        public int MaxValue
        {
            get
            {
                if (this.GetType() == typeof(LoneDiceRoll))
                {
                    return (rolls.First() as LoneDiceRoll).LoneRoll.MaxValue;
                }
                else if (this.GetType() == typeof(WorstDiceRoll))
                {
                    return Enumerable.Min(rolls.Select(x => x.MaxValue));
                }
                else if (this.GetType() == typeof(BestDiceRoll))
                {
                    return Enumerable.Max(rolls.Select(x => x.MaxValue));
                }
                else //SumDiceRoll
                {
                    return Enumerable.Sum(rolls.Select(x => x.MaxValue));
                }
            }
        }

        protected ProbabilityDict RollCalc(Func<int, int, int> calculation, List<DiceRoll> diceRolls)
        {
            ProbabilityDict tempProbs = null;

            foreach (var roll in diceRolls)
            {
                if (tempProbs == null)
                {
                    tempProbs = roll.Probabilities;
                }
                else
                {
                    var tempestProbs = new ProbabilityDict();

                    var curMaxValue = roll.MaxValue + tempProbs.Keys.Max();
                    var curMinValue = roll.MinValue + tempProbs.Keys.Min();

                    for (var i = curMinValue; i <= curMaxValue; i++)
                    {
                        tempestProbs[i] = 0;
                    }
                    foreach (var previousOutcome in tempProbs.Keys)
                    {
                        foreach (var outcomeToAdd in roll.Probabilities.Keys)
                        {
                            tempestProbs[calculation(previousOutcome, outcomeToAdd)] += tempProbs[previousOutcome] * roll.Probabilities[outcomeToAdd];
                        }
                    }
                    tempProbs = tempestProbs;
                }
            }
            return tempProbs;
        }
    }

    public class LoneDiceRoll : DiceRollBase, DiceRoll
    {
        private LoneDiceRoll(DieRoll roll)
        {
            this.Rolls = new List<DiceRoll>();
            LoneRoll = roll;
        }

        private DieRoll loneRoll;
        public DieRoll LoneRoll
        {
            get
            {
                return loneRoll;
            }
            private set
            {
                loneRoll = value;
                CalcProbabilities();
            }
        }

        protected override void CalcProbabilities()
        {
            Probabilities = loneRoll.Probabilities;
        }

        public static implicit operator LoneDiceRoll(DieRoll die)
        {
            return new LoneDiceRoll(die);
        }
    }

    public class WorstDiceRoll : DiceRollBase, DiceRoll
    {
        public WorstDiceRoll(params DiceRoll[] rolls)
        {
            this.Rolls = rolls.ToList();
        }

        protected override void CalcProbabilities()
        {
            Probabilities = RollCalc((x, y) => Math.Min(x, y), Rolls);
        }
    }

    public class BestDiceRoll : DiceRollBase, DiceRoll
    {
        public BestDiceRoll(params DiceRoll[] rolls)
        {
            this.Rolls = rolls.ToList();
        }

        protected override void CalcProbabilities()
        {
            Probabilities = RollCalc((x, y) => Math.Max(x, y), Rolls);
        }
    }

    public class SumDiceRoll : DiceRollBase, DiceRoll
    {
        public SumDiceRoll(params DiceRoll[] rolls)
        {
            this.Rolls = rolls.ToList();
        }

        protected override void CalcProbabilities()
        {
            Probabilities = RollCalc((x, y) => (x + y), Rolls);
        }
    }
}
