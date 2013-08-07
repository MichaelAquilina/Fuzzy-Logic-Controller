using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzyLogicController;

namespace MachineLearningAssignment
{
    public class BoilerSimulator
    {
        private const double K = 0.7;

        public Controller controller;
        public double Heat;
        public double Pressure;
        public double SetPoint;
        public double DeltaPressure;
        public double DeltaHeat;

        public double PressureError = 0;
        public double ChangePressureError = 0;

        public DefuzificationMethod Method;
        public int Samples = 41;
        public double MinX = -5;
        public double MaxX = 5;

        public BoilerSimulator( double SetPoint, double InitialPressure, int Samples, DefuzificationMethod Method )
        {
            this.SetPoint = SetPoint;
            this.Pressure = InitialPressure;
            this.Samples = Samples;
            this.Method = Method;

            controller = new Controller(MinX, MaxX, Samples, Method);  //use center of area fuzzy classification

            MembershipFunctionFactory factory = new MembershipFunctionFactory(MinX, MaxX, Samples);
            factory.MultiplyBy = 5;

            //membership functions to use in the controller
            MembershipFunction nb = factory.Create( "nb", -1, -0.7, 0, 0.2);
            MembershipFunction nm = factory.Create( "nm", -0.65, -0.35, 0.2, 0.2);
            MembershipFunction ns = factory.Create( "ns", -0.3, 0, 0.2, 0);
            MembershipFunction nz = factory.Create( "nz", -0.05, 0, 0.05, 0);
            MembershipFunction ze = factory.Create( "ze", -0.05, 0.05, 0.05, 0.05);
            MembershipFunction pz = factory.Create( "pz", 0, 0.05, 0, 0.05);
            MembershipFunction ps = factory.Create( "ps", 0, 0.3, 0, 0.2);
            MembershipFunction pm = factory.Create( "ps", 0.35, 0.65, 0.2, 0.2);
            MembershipFunction pb = factory.Create( "pn", 0.7, 1, 0.2, 0);

            //need to double check these values
            controller.AddRule(new Rule( "Rule 1",   nb,      ns + pb,    pb  ) );  //1
            controller.AddRule(new Rule( "Rule 2", nb + nm, ns, pm));  //2
            controller.AddRule(new Rule( "Rule 3", ns, nz + ps, pm));  //3
            controller.AddRule(new Rule( "Rule 4", nz, pm + pb, pm));  //4
            controller.AddRule(new Rule( "Rule 5", nz, nb + nm, nm));  //5
            controller.AddRule(new Rule( "Rule 6", nz + pz, nz, nz));  //6
            controller.AddRule(new Rule( "Rule 7", pz, nb + nm, pm));  //7
            controller.AddRule(new Rule( "Rule 8", pz, pm + pb, nm));  //8
            controller.AddRule(new Rule( "Rule 9", ps, nz + ps, nm));  //9
            controller.AddRule(new Rule( "Rule 10", pm + pb, ns, nm));  //10
            controller.AddRule(new Rule( "Rule 11", pb, ns + pb, nb));  //11
            controller.AddRule(new Rule( "Rule 12", nz, ps, ps));  //12
            controller.AddRule(new Rule( "Rule 13", nz, ns, ns));  //13
            controller.AddRule(new Rule( "Rule 14", pz, ns, ps));  //14
            controller.AddRule(new Rule( "Rule 15", pz, ps, ns));  //15
            //controller.AddRule(new Rule( "Rule 16", nb + nm, nb + nm, pb));  //16
            controller.AddRule(new Rule( "Rule 17", pm + pb, nb + nm, nb));  //17
        }

        public void Next()
        {
            double PE = Pressure - SetPoint;
            double CPE = PE - PressureError;

            PressureError = PE;
            ChangePressureError = CPE;

            DeltaHeat = controller.Next( PE, CPE );
            DeltaPressure = getDeltaPressure(DeltaHeat);
            Pressure += DeltaPressure;
            Heat += DeltaHeat;
        }

        //plant function
        private double getDeltaPressure(double DeltaHeat)
        {
            return Math.Sign(DeltaHeat) * K * Math.Pow(Math.Abs(DeltaHeat), 0.75);
        }
    }
}
