using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogicController
{
    //defines a membership function to be used in a Rule for a fuzzy logic controller
    public class MembershipFunction
    {
        public double[] values;
        public double a;
        public double b;
        public double alpha;
        public double beta;

        private double min;         //min domain value
        private double max;         //max domain value
        private double interval;    //interval width

        //label assosciated with the membership function
        public String Label;

        public double Min
        {
            get { return min; }
        }

        public double Max
        {
            get { return max; }
        }

        public int Samples
        {
            get{ return values.Length; }
        }

        //creates a membership function from the given parameters
        //will sample the function with the given number of sampling points
        //the range is defined by the min and max values
        public MembershipFunction( String label, double a, double b, double alpha, double beta, int samples, double min, double max)
        {
            values = new double[samples];
            this.Label = label;
            this.a = a;
            this.b = b;
            this.alpha = alpha;
            this.beta = beta;
            this.min = min;
            this.max = max;

            double alpha_inverse = 1 / alpha;
            double beta_inverse = 1 / beta;

            //perform sampling
            double range = max - min;
            interval = range / samples;
            double x;

            for (int i = 0; i < samples; i++)
            {
                x = min + i * interval;
                if ( x > b + beta || x < a - alpha)
                    values[i] = 0;

                if ( x >= a && x <= b)
                    values[i] = 1;

                if (x > a - alpha && x < a)
                    values[i] = (x - (a - alpha)) * alpha_inverse;

                if (x < b + beta && x > b)
                    values[i] = 1 - (x-b) * beta_inverse;
            }
        }

        //returns the corresponding y value of this membership function
        public double getValue(double x)
        {
            if (x > max)
                x = max;

            if (x < min)
                x = min;

            int index;
            if (x == min)
                index = 0;
            else if( x== max )
                index = Samples - 1;
            else index = (int)Math.Round((x - min) / interval);    //should check function value

            return values[index];
        }

        //composition operator. Takes two membership functions and joins them. Equivalant to from_to_fn
        //the number of sampling points of the output mem. function will be the largest from the two input
        //if the two membership functions are not defined on the same range an error will be thrown
        public static MembershipFunction operator +(MembershipFunction f1, MembershipFunction f2)
        {
            if (f1.min != f2.min || f1.max != f2.max )
                throw new InvalidRangeException("The two membership functions are not defined on the same range");

            return new MembershipFunction( "from_to_fn "+f1.Label+" "+f2.Label, 
                                            f1.a, f2.b, f1.alpha, f2.beta, 
                                            Math.Max(f1.values.Length,f2.values.Length), 
                                            f1.min, f1.max );
        }
    }
}
