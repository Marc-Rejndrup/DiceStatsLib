using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class TargetAttack
    {
        public int BaseDamage { get; set; }

        public int AttackBonus { get; set; }

        public DiceRoll AttackBonusDice { get; set; }

        public Attack BaseAttack { get; set; }

        public int AutoHitBy { get; set; }

        public TargetAttack(int baseDamage, int attackBonus, Attack baseAttack)
        {
            this.BaseDamage = baseDamage;

            this.AttackBonus = attackBonus;

            this.BaseAttack = baseAttack;

            this.AutoHitBy = 20;
        }
        public TargetAttack(int baseDamage, int attackBonus, int baseDie) : this(baseDamage, attackBonus, new Attack(baseDie)) { }

        public ProbabilityDict Probabilities (int OpposingAC)
        {
            {
                var mod = new ProbabilityDict();

                var stopped = OpposingAC - AttackBonus;

                var baseProbabilities = BaseAttack.Probabilities;

                var bonusProbabilities = AttackBonusDice.Probabilities;

                var combinedProbabilities = baseProbabilities + bonusProbabilities;

                List<int> keysToRemove = new List<int>();

                foreach (var element in baseProbabilities.Keys)
                {
                    if(element <= stopped)
                    {
                        keysToRemove.Add(stopped);
                    }
                }

                foreach(var key in keysToRemove)
                {
                    baseProbabilities.Remove(key);
                }

                throw new NotImplementedException();
            }
        }

        public int Roll(int OpposingAC)
        {
            var activatorResult = BaseAttack.BaseActivator.Roll() + AttackBonus;

            var toHit = activatorResult + AttackBonus;

            if (toHit > OpposingAC)
                return 0;

            var damage = BaseDamage;

            foreach (var die in BaseAttack.DicePool)
            {
                if (activatorResult >= die.MinActivate && activatorResult <= die.MaxActivate)
                    foreach (var curDie in die.Dice.Rolls)
                    {
                        damage += curDie.Roll();
                    }
            }
            return damage;
        }
    }
}
