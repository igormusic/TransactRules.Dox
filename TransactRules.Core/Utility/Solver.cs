using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


public class Solver
{
    public delegate decimal SolverFunction(decimal Parameter);

    //Maximum allowed number of iterations.
    const int ITMAX = 100;
    //Machine floating-point precision.
    const decimal EPS = 0.000000003M;

    public static decimal FindFunctionZero(SolverFunction func, decimal lowerBound, decimal upperBound, decimal tolerance)
    {
        //Using Brent’s method, find the root of a function func known to lie between x1 and x2. The
        //root, returned as zbrent, will be refined until its accuracy is tol.

        int iter = 0;
        decimal a = 0;
        decimal b = 0;
        decimal c = 0;
        decimal d = 0;
        decimal e = 0;
        decimal min1 = 0;
        decimal min2 = 0;

        a = lowerBound;
        b = upperBound;
        c = upperBound;

        decimal fa = 0;
        decimal fb = 0;
        decimal fc = 0;
        decimal p = 0;
        decimal q = 0;
        decimal r = 0;
        decimal s = 0;
        decimal tol1 = 0;
        decimal xm = 0;

        fa = func(a);
        fb = func(b);

        if ((fa > 0 && fb > 0) || (fa < 0 && fb < 0))
        {
            throw new ApplicationException("Root must be bracketed");
        }

        fc = fb;


        for (iter = 1; iter <= ITMAX; iter++)
        {
            if ((fb > 0 && fc > 0) || (fb < 0 && fc < 0))
            {
                c = a;
                //Rename a, b, c and adjust bounding interval d
                fc = fa;
                d = b - a;
                e = d;
            }

            if (Math.Abs(fc) < Math.Abs(fb))
            {
                a = b;
                b = c;
                c = a;
                fa = fb;
                fb = fc;
                fc = fa;
            }

            tol1 = 2 * EPS * Math.Abs(b) + 0.5M * tolerance;
            //Convergence check.
            xm = 0.5M * (c - b);

            if (Math.Abs(xm) <= tol1 || fb == 0.0M)
            {
                return b;
            }

            if ((Math.Abs(e) >= tol1 & Math.Abs(fa) > Math.Abs(fb)))
            {
                s = fb / fa;
                //Attempt inverse quadratic interpolation.

                if ((a == c))
                {
                    p = 2 * xm * s;
                    q = 1 - s;
                }
                else
                {
                    q = fa / fc;
                    r = fb / fc;
                    p = s * (2 * xm * q * (q - r) - (b - a) * (r - 1));
                    q = (q - 1) * (r - 1) * (s - 1);
                }

                if ((p > 0))
                    q = -q;
                //Check whether in bounds.

                p = Math.Abs(p);

                min1 = 3 * xm * q - Math.Abs(tol1 * q);
                min2 = Math.Abs(e * q);

                if (2 * p < (decimal)(min1 < min2 ? min1 : min2))
                {
                    e = d;
                    //Accept interpolation.
                    d = p / q;
                }
                else
                {
                    d = xm;
                    //Interpolation failed, use bisection.
                    e = d;
                }

            }
            else
            {
                //Bounds decreasing too slowly, use bisection.
                d = xm;
                e = d;
            }

            a = b;
            //Move last best guess to a.
            fa = fb;

            //Evaluate new trial root.
            if (Math.Abs(d) > tol1)
            {
                b = b + d;
            }
            else
            {
                b = b + MSIGN(tol1, xm);
            }

            fb = func(b);
            Trace.WriteLine(string.Format("{0}:f({1})={2}", iter, b, fb));
        }

        throw new ApplicationException("Maximum number of iterations exceeded");
    }

    private static decimal MSIGN(decimal a, decimal b)
    {
        return (decimal)(b >= 0 ? Math.Abs(a) : -Math.Abs(a));
    }

}
