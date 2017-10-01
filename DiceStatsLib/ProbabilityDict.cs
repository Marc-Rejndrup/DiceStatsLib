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
        public static ProbabilityDict Combine(Func<int, int, int> keyTransformation, Func<Rational, Rational, Rational> valueTransformation, params ProbabilityDict[] pds)
        {
            var pdsList = pds.ToList();

            ProbabilityDict result = pdsList[0];

            pdsList.RemoveAt(0);

            while (pds.Length > 0)
            {
                result = Combine(keyTransformation, valueTransformation, result, pds[0]);
                pdsList.RemoveAt(0);
            }
            return pds[0];
        }
        public static ProbabilityDict Combine(Func<int, int, int> keyTransformation, Func<Rational, Rational, Rational> valueTransformation, ProbabilityDict pd1, ProbabilityDict pd2)
        {
            var modDict = new ProbabilityDict();

            foreach (var key1 in pd1.Keys)
            {
                foreach (var key2 in pd2.Keys)
                {
                    if (modDict.ContainsKey(keyTransformation(key1, key2)))
                    {
                        modDict[keyTransformation(key1, key2)] += valueTransformation(pd1[key1], pd2[key2]);
                    }
                    else
                        modDict[keyTransformation(key1, key2)] = valueTransformation(pd1[key1], pd2[key2]);
                }
            }
            return modDict;
        }
        public static ProbabilityDict Transform(Func<int, int> keyTransformation, Func<Rational, Rational> valueTransformation, ProbabilityDict pd)
        {
            var modDict = new ProbabilityDict();

            foreach (var key in pd.Keys)
            {
                if (modDict.ContainsKey(keyTransformation(key)))
                {
                    modDict[keyTransformation(key)] += valueTransformation(pd[key]);
                }
                else
                    modDict[keyTransformation(key)] = valueTransformation(pd[key]);

            }
            return modDict;
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
