using DiceStatsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiceStatsLib.ProbabilityUtils;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Die d12 = new Die(12);
            var nprob = d12.Probabilities;

            DieRoll d12r2 = new DieRoll(d12);
            d12r2.ReRoll = 2;
            var rprob = d12r2.Probabilities;

            double check = 0;

            foreach(var key in rprob.Keys)
            {
                check += rprob[key].Value;//approach 1
            }
            Console.WriteLine($"Should be 1: {check}\n");

            var result = ProbabilityUtils.CalcImprovementOfReRoll(rprob);
            var avg = Average(result);
            var oddsHelpful = SumProbabilities(result);

            Console.WriteLine($"Extra Damage:\n{avg.Value}\n{avg.Numerator}/{avg.Denominator}\nOdds of benefit:\n{oddsHelpful.Value*100}% \n:{oddsHelpful.Numerator}/{oddsHelpful.Denominator}");
            var s  = Console.ReadLine();
        }
    }
}
