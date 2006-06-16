#region MathNet Numerics, Copyright �2006 Christoph Ruegg

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004-2006,	Christoph Ruegg, http://www.cdrnet.net
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Numerics
{
    /// <summary>
    /// Static DoublePrecision Special Functions Helper Class
    /// </summary>
    public static class Fn
    {
        /// <summary> Returns <code>sqrt(a<sup>2</sup> + b<sup>2</sup>)</code> 
        /// without underflow/overlow.</summary>
        public static double Hypot(double a, double b)
        {
            double r;

            if(Math.Abs(a) > Math.Abs(b))
            {
                r = b / a;
                r = Math.Abs(a) * Math.Sqrt(1 + r * r);
            }
            else if(b != 0)
            {
                r = a / b;
                r = Math.Abs(b) * Math.Sqrt(1 + r * r);
            }
            else r = 0.0;

            return r;
        }

        /// <summary>
        /// Returns the greatest common divisor of two integers using euclids algorithm.
        /// </summary>
        /// <returns>gcd(a,b)</returns>
        public static long Gcd(long a, long b)
        {                        	
            long rem;                	
            while(b != 0)
            {       	
                rem = a % b;
                a = b;
                b = rem;
            }
            return Math.Abs(a);
        }

        /// <summary>
        /// Computes the extended greatest common divisor, such that a*x + b*y = gcd(a,b).
        /// </summary>
        /// <returns>gcd(a,b)</returns>
        /// <example>
        /// <code>
        /// long x,y,d;
        /// d = Fn.Gcd(45,18,out x, out y);
        /// -> d == 9 && x == 1 && y == -2
        /// </code>
        /// The gcd of 45 and 18 is 9: 18 = 2*9, 45 = 5*9. 9 = 1*45 -2*18, therefore x=1 and y=-2.
        /// </example>
        public static long Gcd(long a, long b, out long x, out long y)
        {
            long rem, quot, tmp;
            long mp = 1, np = 0, m = 0, n = 1;

            while(b != 0)
            {
                quot = a / b;
                rem = a % b;
                a = b;
                b = rem;

                tmp = m;
                m = mp - quot * m;
                mp = tmp;

                tmp = n;
                n = np - quot * n;
                np = tmp;
            }

            if(a >= 0)
            {
                x = mp;
                y = np;
                return a;
            }
            else
            {
                x = -mp;
                y = -np;
                return -a;
            }
        }

        /// <summary>
        /// Returns the least common multiple of two integers using euclids algorithm.
        /// </summary>
        /// <returns>lcm(a,b)</returns>
        public static long Lcm(long a, long b)
        {
            // TODO: Direct Implementation for preventing overflows.
            if(a == 0 && b == 0)
                return 0;
            return Math.Abs(a * b) / Gcd(a, b);
        }

        /// <summary>
        /// Returns the natural logarithm of Gamma for a real value > 0
        /// </summary>
        /// <param name="xx">A real value for Gamma calculation</param>
        /// <returns>A value ln|Gamma(xx))| for xx > 0</returns>
        public static double GammaLn(double xx)
        {
            // TODO: check
            double x, y, ser, temp;
            double[] coefficient = new double[]{76.18009172947146,-86.50535032941677,
												   24.01409824083091,-1.231739572450155,0.1208650973866179e-2,-0.5395239384953e-5};
            int j;
            y = x = xx;
            temp = x + 5.5;
            temp -= ((x + 0.5) * Math.Log(temp));
            ser = 1.000000000190015;
            for(j = 0; j <= 5; j++)
                ser += (coefficient[j] / ++y);
            return -temp + Math.Log(2.50662827463100005 * ser / x);
        }

        /// <summary>
        /// Returns a factorial of an integer number (n!)
        /// </summary>
        /// <param name="n">The value to be factorialized</param>
        /// <returns>The double precision result</returns>
        public static double Factorial(int n)
        {
            // TODO: check
            int ntop = 4;
            double[] a = new double[32];
            a[0] = 1.0; a[1] = 1.0; a[2] = 2.0; a[3] = 6.0; a[4] = 24.0;
            int j;
            if(n < 0)
                throw new ArgumentException("Factorial expects a positive argument", "n");
            if(n > 32)
                return Math.Exp(GammaLn(n + 1.0));
            while(ntop < n)
            {
                j = ntop++;
                a[ntop] = a[j] * ntop;
            }
            return a[n];
        }

        /// <summary>
        /// Returns a binomial coefficient of n and k as a double precision number
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double BinomialCoefficient(int n, int k)
        {
            if(k < 0 || k > n)
                return 0;
            return Math.Floor(0.5 + Math.Exp(FactorialLn(n) - FactorialLn(k) - FactorialLn(n - k)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double FactorialLn(int n)
        {
            // TODO: check
            double[] a = new double[101];
            if(n < 0)
                throw new ArgumentException("Factorial expects a positive argument", "n");
            if(n <= 1)
                return 0.0d;
            if(n <= 100)
            {
                a[n] = GammaLn(n + 1.0d);
                return (a[n] == 0.0d) ? a[n] : (a[n]); // TODO: historic hulk?
            }
            else
            {
                return GammaLn(n + 1.0d);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Beta(double z, double w)
        {
            return Math.Exp(GammaLn(z) + GammaLn(w) - GammaLn(z + w));
        }
    }
}