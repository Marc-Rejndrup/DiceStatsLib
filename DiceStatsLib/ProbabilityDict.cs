using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class ProbabilityDict : Dictionary<int, Rational>
    {
        public static ProbabilityDict operator +(ProbabilityDict pd1, ProbabilityDict pd2)
        {
            return OperaterHelper((x, y) => x + y, pd1, pd2);
        }
        public static ProbabilityDict operator -(ProbabilityDict pd1, ProbabilityDict pd2)
        {
            return OperaterHelper((x, y) => x - y, pd1, pd2);
        }
        public static ProbabilityDict operator *(ProbabilityDict pd1, ProbabilityDict pd2)
        {
            return OperaterHelper((x, y) => x * y, pd1, pd2);
        }
        public static ProbabilityDict operator /(ProbabilityDict pd1, ProbabilityDict pd2)
        {
            return OperaterHelper((x, y) => x / y, pd1, pd2);
        }
        public static ProbabilityDict operator %(ProbabilityDict pd1, ProbabilityDict pd2)
        {
            return OperaterHelper((x, y) => x % y, pd1, pd2);
        }
        private static ProbabilityDict OperaterHelper(Func<int, int, int> calculation, ProbabilityDict pd1, ProbabilityDict pd2)
        {
            var modDict = new ProbabilityDict();

            foreach (var key1 in pd1.Keys)
            {
                foreach (var key2 in pd2.Keys)
                {
                    if (modDict.ContainsKey(calculation(key1, key2)))
                    {
                        modDict[calculation(key1, key2)] += pd1[key1] * pd2[key2];
                    }
                    else
                        modDict[calculation(key1, key2)] = pd1[key1] * pd2[key2];
                }
            }
            return modDict;
        }
    }
}
