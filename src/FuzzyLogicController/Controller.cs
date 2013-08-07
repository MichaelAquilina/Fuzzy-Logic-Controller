using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogicController
{
    //generic binary input fuzzy logic controller
    public class Controller
    {
        public List<Rule> rules = new List<Rule>();
        private double min;
        private double max;
        private int samples;
        private DefuzificationMethod method;       //change this into a delegate with prebuilt COA and MOM delegates

        //store the history in a list so that it can be iterated through
        public List<double> OutputValueList = new List<double>();       //output of the controller after performing defuzification
        public List<double[]> OutputGraphList = new List<double[]>();   //output graphs resulting from min max inference
        public List<Rule> OutputRuleList = new List<Rule>();              //value corresponds the index of the rule that fired

        public int RuleCount
        {
            get { return rules.Count; }
        }

        public double Interval
        {
            get { return (max - min) / samples; }
        }

        public double Min
        {
            get { return min; }
        }

        public double Max
        {
            get { return max; }
        }

        public Controller(double min, double max, int samples, DefuzificationMethod method)
        {
            this.min = min;
            this.max = max;
            this.samples = samples;
            this.method = method;
        }

        //gets a rule from the fuzzy logic controller
        public Rule GetRule(int index)
        {
            return rules[index];
        }

        //adds a new rule to the fuzzy logic controller
        public void AddRule(Rule rule)
        {
            if (rule.F1.Max != max || rule.F1.Min != min || rule.F1.Samples != samples)
                throw new InvalidRangeException("The membership functions must be defined on the same domain");

            if (rule.F2.Max != max || rule.F2.Min != min || rule.F2.Samples != samples)
                throw new InvalidRangeException("The membership functions must be defined on the same domain");

            rules.Add(rule);
        }

        //returns the resultant value, given a set of rules to work with
        public double Next( double value1, double value2 )
        {
            double[] outputs = new double[rules.Count];  //allows cropping
            double[] OutputGraph = new double[samples];  //output graph from min max inferencing
            int[] OutputRules = new int[samples];        //array storing which rule corresponds to a point

            //perform fuzzyfication
            for (int i = 0; i < rules.Count; i++)
                outputs[i] = getOutputValue(rules[i], value1, value2);

            //build output graph (min max inferencing)
            for (int i = 0; i < samples; i++)
            {
                double Max = Double.MinValue;
                int rule = -1;                         //shows which rule is firing
                
                for (int j = 0; j < rules.Count; j++)
                {
                    double value = Math.Min(outputs[j], rules[j].Output.values[i]);
                    if (value > Max)
                    {
                        rule = j;
                        Max = value;
                    }
                }

                OutputGraph[i] = Max;
                OutputRules[i] = rule;
            }

            //adds the output graph to the history list
            OutputGraphList.Add(OutputGraph);

            //perform defuzzification
            double OutputValue =  (method == DefuzificationMethod.CoA) ? CenterOfArea(OutputGraph,OutputRules) : MeanOfMaxima(OutputGraph,OutputRules);
            
            //add the output value to the history list
            OutputValueList.Add(OutputValue);
            return OutputValue;
        }

        //perfroms a center of area calculation on the given graph
        private double CenterOfArea(double[] graph, int[] OutputRules )
        {
            double[] ruleArea = new double[rules.Count];        //store areas that each rule contributes
            double totalArea = 0;
            double area = 0;

            //we can just take into consideration height
            for (int i = 0; i < graph.Length; i++)
            {
                totalArea += graph[i];
                ruleArea[OutputRules[i]] += graph[i];
            }

            //determine which rule fired using the rule areas calculated
            int rule = -1;
            double Max = Double.MinValue;
            for (int i = 0; i < ruleArea.Length; i++)
            {
                if (ruleArea[i] > Max)
                {
                    Max = ruleArea[i];
                    rule = i;
                }
            }
            OutputRuleList.Add(rules[rule]);

            //divide by two for comparasin
            totalArea /= 2;

            for (int i = 0; i < graph.Length; i++)
            {
                area += graph[i];
                if (area >= totalArea)
                    return GetX(i);
            }

            throw new InvalidRangeException("Center of Area was not succesful in calculating an output value");
        }

        //performs a mean of maxima calculation on the given graph
        private double MeanOfMaxima(double[] graph, int[] OutputRules )
        {
            int[] ruleCount = new int[rules.Count];        //store how many times a rule occurs in the maxima
            double max = Double.MinValue;
            for (int i = 0; i < graph.Length; i++)
            {
                if (graph[i] > max)
                    max = graph[i];
            }

            double total = 0;
            double count = 0;
            for (int i = 0; i < graph.Length; i++)
            {
                if (graph[i] == max)
                {
                    ruleCount[OutputRules[i]]++;
                    total += GetX(i);
                    count++;
                }
            }

            //check which rule fired by checking which has the largest count
            int rule = -1;
            int Max = Int32.MinValue;
            for (int i=0; i < ruleCount.Length; i++)
            {
                if (ruleCount[i] > Max)
                {
                    Max = ruleCount[i];
                    rule = i;
                }
            }
            OutputRuleList.Add(rules[rule]);

            return total / count;
        }

        //gets the minimum y value from the two input rules
        private double getOutputValue(Rule rule, double value1, double value2)
        {
            return Math.Min(rule.F1.getValue(value1), rule.F2.getValue(value2));
        }

        //returns the corresponding x value from an index
        private double GetX(int index)
        {
            return index * Interval + min;
        }
    }
}
