using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    class Attacker
    {
        public List<TargetAttack> Attacks { get; set; }

        public int AC { get; set; }

        public int HP { get; set; }
    }
}
