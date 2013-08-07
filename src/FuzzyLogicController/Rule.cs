using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogicController
{
    public class Rule
    {
        private String label;

        public MembershipFunction F1;
        public MembershipFunction F2;
        public MembershipFunction Output;
        public String Label
        {
            get { return label; }
            set { label = value; }
        }

        public Rule(String Label, MembershipFunction F1, MembershipFunction F2, MembershipFunction Output)
        {
            if (F1.Min != F2.Min || F1.Max != F2.Max)
                throw new InvalidRangeException("The membership functions must be defined on the same range");

            this.label = Label;
            this.F1 = F1;
            this.F2 = F2;
            this.Output = Output;
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
