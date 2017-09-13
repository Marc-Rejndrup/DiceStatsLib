using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class Attack
    {
        public DiceRoll BaseActivator { get { return BaseActivator; } set { this.BaseActivator = value; CalcProbabilities(); } }
        //while needed, this needs to represent bonuses that don't contribute to "range results"(crits), like bless.

        public List<DiceRange> DicePool { get { return DicePool; } set { this.DicePool = value; CalcProbabilities(); } }

        public Attack(int normalDamageDie)
        {
            BaseActivator = (LoneDiceRoll)new DieRoll(new Die(20));

            DicePool = new List<DiceRange>()
            {
                new DiceRange(20, 20, new SumDiceRoll((LoneDiceRoll)new DieRoll(new Die(normalDamageDie)))),//crit

                new DiceRange(new SumDiceRoll((LoneDiceRoll)new DieRoll(new Die(normalDamageDie))))//normal damage
            };
        }

        public Dictionary<int, Rational> Probabilities { get; private set; }

        private void CalcProbabilities()
        {
            Probabilities = new Dictionary<int, Rational>();

            var activatorOdds = BaseActivator.Probabilities;

            foreach (var diceRange in DicePool)
            {
                foreach (var activatorValue in activatorOdds.Keys)
                {
                    if (diceRange.In(activatorValue))
                    {
                        foreach (var diceResult in diceRange.Dice.Probabilities)
                        {
                            if (Probabilities.ContainsKey(diceResult.Key))
                                Probabilities[diceResult.Key] += (diceResult.Value * activatorOdds[activatorValue]);

                            else
                                Probabilities[diceResult.Key] = (diceResult.Value * activatorOdds[activatorValue]);
                        }
                    }
                }
            }
        }

        public int Roll()
        {
            int activatorResult = BaseActivator.Roll();

            int value = 0;

            foreach (var die in DicePool)
            {
                if (activatorResult >= die.MinActivate && activatorResult <= die.MaxActivate)
                {
                    foreach (var curDie in die.Dice.Rolls)
                    {
                        value += curDie.Roll();
                    }
                }
            }

            return value;
        }
    }
}
