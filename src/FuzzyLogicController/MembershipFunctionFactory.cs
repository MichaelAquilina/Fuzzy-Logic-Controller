using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogicController
{
    public class MembershipFunctionFactory
    {
        private int samples;
        private double max;
        private double min;
        private double m = 1;

        //quick and easy way of multiplying the values being inserted to create factories
        public double MultiplyBy
        {
            get { return m; }
            set { m = value; }
        }

        public MembershipFunctionFactory(double min, double max, int samples)
        {
            this.samples = samples;
            this.max = max;
            this.min = min;
        }

        public MembershipFunction Create( String label, double a, double b, double alpha, double beta )
        {
            return new MembershipFunction( label, a*m, b*m, alpha*m, beta*m, samples, min, max );
        }
    }
}
