using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceStatsLib
{
    public class Rational
    {
        public long Numerator { get; set; }

        public ulong Denominator { get; set; }

        public double Value
        {
            get
            {
                return ((double)this.Numerator) / this.Denominator;
            }
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public string ToStringPercent()
        {
            return $"{this.Value*100}%";
        }

        public Rational(long numerator, long denominator)
        {
            if (denominator < 0)
            {
                this.Numerator = numerator * -1;

                this.Denominator = (ulong)(denominator * -1);
            }
            else
            {
                this.Numerator = numerator;

                this.Denominator = (ulong)denominator;
            }
        }
        public Rational(long numerator, ulong denominator)
        {
            this.Numerator = numerator;

            this.Denominator = denominator;
        }

        public static implicit operator Rational(long num)
        {
            return new Rational(num, 1);
        }

        public static Rational operator *(Rational rat, Rational rat2)
        {
            var ret = new Rational(rat.Numerator * rat2.Numerator, rat.Denominator * rat2.Denominator);

            ret.Simplify();

            return ret;
        }

        public static Rational operator +(Rational rat, Rational rat2)
        {
            if (rat.Denominator == rat2.Denominator)
                return new Rational(rat.Numerator + rat2.Numerator, rat.Denominator);

            else
            {
                long newNumerator;

                short mod = 1;

                if (rat.Numerator < 0)
                    mod = -1;
                newNumerator = mod*(long)((ulong)rat.Numerator * rat2.Denominator);

                mod = 1;

                if (rat.Numerator < 0)
                    mod = -1;

                newNumerator += mod * (long)((ulong)rat2.Numerator * rat.Denominator);

                var ret = new Rational(newNumerator, rat.Denominator * rat2.Denominator);

                ret.Simplify();

                return ret;
            }
        }

        public static Rational operator -(Rational rat, Rational rat2)
        {
            rat2.Numerator = rat2.Numerator * -1;
            return rat + rat2;
        }

        public void Simplify()
        {
            var gcd = GCD((ulong)this.Numerator, this.Denominator);

            if (gcd == 0)
                return;

            var neg = Numerator < 0;

            this.Numerator = neg ? 1 * (long)(((ulong)this.Numerator) / gcd) : -1 * (long)(((ulong)this.Numerator) / gcd);

            this.Denominator = this.Denominator / gcd;
        }

        private static ulong GCD(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            if (a == 0)
                return b;
            else
                return a;
        }
    }
    public static class RationalEnumerableExtensions
    {
        public static Rational Sum(this IEnumerable<Rational> source)
        {
            return source.Aggregate((x, y) => x + y);
        }

        public static Rational Sum<T>(this IEnumerable<T> source, Func<T, Rational> selector)
        {
            return source.Select(selector).Aggregate((x, y) => x + y);
        }
    }
}
