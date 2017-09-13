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

        public DiceRoll Dice { get; set; }

        public DiceRange(int start, int end, DiceRoll dice)
        {
            MinActivate = start;

            MaxActivate = end;

            Dice = dice;
        }

        public DiceRange(DiceRoll dice) : this(1, 20, dice) { }

        public bool In (int i)
        {
            return i <= MaxActivate && i >= MinActivate;
        }
    }
}
