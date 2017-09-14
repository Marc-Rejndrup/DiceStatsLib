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

        public List<DiceRange> DicePool { get { return DicePool; } set { this.DicePool = value; CalcProbabilities(); } }

        public Attack(int normalDamageDie)
        {
            BaseActivator = (LoneDiceRoll)new DieRoll(new Die(20));

            DicePool = new List<DiceRange>()
            {
                new DiceRange(new SumDiceRoll((LoneDiceRoll)new DieRoll(new Die(normalDamageDie))), 20, 20),//crit

                new DiceRange(new SumDiceRoll((LoneDiceRoll)new DieRoll(new Die(normalDamageDie))))//normal damage
            };
        }

        public ProbabilityDict Probabilities { get; private set; }

        private void CalcProbabilities()
        {
            Probabilities = new ProbabilityDict();

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

        public int Roll(int activatorResult)
        {
            int value = 0;

            foreach (var die in DicePool)
            {
                if (activatorResult >= die.StartActivate && activatorResult <= die.StopActivate)
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
