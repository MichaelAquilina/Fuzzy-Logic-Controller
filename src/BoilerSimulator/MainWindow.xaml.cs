using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FuzzyLogicController;

namespace MachineLearningAssignment
{
    public partial class MainWindow : Window
    {
        public BoilerSimulator simulator = new BoilerSimulator(500, 450, 41, DefuzificationMethod.CoA);

        //properties retrevied from the gui
        public double InitialPressure
        {
            get
            {
                return Convert.ToDouble(InitialPressureTextBox.Text);
            }
            set
            {
                InitialPressureTextBox.Text = value.ToString();
            }
        }
        public double SetPoint
        {
            get
            {
                return Convert.ToDouble(SetPointTextBox.Text);
            }
            set
            {
                SetPointTextBox.Text = value.ToString();
            }
        }
        public int Samples
        {
            get
            {
                return Convert.ToInt32(SamplingPointsTextBox.Text);
            }
            set
            {
                SamplingPointsTextBox.Text = value.ToString();
            }
        }
        public int Iterations
        {
            get
            {
                return Convert.ToInt32(IterationsTextBox.Text);
            }
            set
            {
                IterationsTextBox.Text = value.ToString();
            }
        }
        public DefuzificationMethod Method
        {
            get
            {
                return (MethodComboBox.SelectedItem == MeanOfMaximaComboBoxItem) ? DefuzificationMethod.MoM : DefuzificationMethod.CoA;
            }
            set
            {
                if (value == DefuzificationMethod.MoM)
                    MethodComboBox.SelectedItem = MeanOfMaximaComboBoxItem;
                else
                    MethodComboBox.SelectedItem = CenterOfAreaComboBoxItem;
            }
        }

        public double Interval;
        public double Min;

        public MainWindow()
        {
            InitializeComponent();
            
            Interval = simulator.controller.Interval;
            Min = simulator.controller.Min;

            foreach (Rule rule in simulator.controller.rules)
                AddRule(rule);
            
            simulator = null;
        }

        #region Other Methods

        private List<String> GenerateHistory(List<Rule> RuleOutput)
        {
            List<String> Output = new List<String>();
            for (int i = 0; i < RuleOutput.Count; i++)
                Output.Add( (RuleOutput[i]).Label);
            return Output;
        }

        private void RemoveRule(int index)
        {
            RulesListBox.SelectedIndex = -1;
            RulesListBox.Items.RemoveAt(index);
        }

        private void AddRule(Rule rule)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.Content = rule.Label;
            checkbox.IsChecked = true;
            checkbox.Tag = rule;
            RulesListBox.Items.Add(checkbox);
        }

        private void EditRule(int index, Rule rule)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.Content = rule.Label;
            checkbox.IsChecked = true;
            checkbox.Tag = rule;
            RulesListBox.SelectedIndex = -1;
            RulesListBox.Items[index] = checkbox;
            RulesListBox.SelectedIndex = index;
        }

        private Rule GetRule(int index)
        {
            if (index == -1)
                return null;

            CheckBox checkbox = (CheckBox)RulesListBox.Items[index];
            Rule rule = (Rule)checkbox.Tag;
            return rule;
        }

        private Rule GetSelectedRule()
        {
            return GetRule(RulesListBox.SelectedIndex);
        }

        private List<Point> BuildGraph(double[] graph)
        {
            List<Point> Values = new List<Point>();
            for (int i = 0; i < graph.Length; i++)
                Values.Add(new Point(i * Interval + Min, graph[i]));

            return Values;
        }

        private List<Point> BuildGraph(MembershipFunction mf)
        {
            return BuildGraph(mf.values);
        }

        #endregion

        #region Control Event Handlers

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                simulator = new BoilerSimulator(SetPoint, InitialPressure, Samples, Method);
                simulator.controller.rules.Clear();

                for (int i = 0; i < RulesListBox.Items.Count; i++)
                {
                    CheckBox checkbox = (CheckBox)RulesListBox.Items[i];

                    if (checkbox.IsChecked.Value)
                        simulator.controller.rules.Add((Rule)checkbox.Tag);
                }

                List<int> indices = new List<int>();
                List<Point> Values = new List<Point>();
                MainPECPEGraph.ClearLines();
                for (int i = 0; i < Iterations; i++)
                {
                    simulator.Next();
                    indices.Add(i);
                    Values.Add(new Point(i, simulator.Pressure));
                    MainPECPEGraph.DrawLine(simulator.controller.OutputRuleList.Last());
                }

                OutputGraphLineSeries.ItemsSource = Values;
                IterationComboBox.ItemsSource = indices;
                //generate a history based on the points
                HistoryOutputRulesListBox.ItemsSource = GenerateHistory(simulator.controller.OutputRuleList);

                StartButton.Content = "Re-run Simulation";
            } catch( FormatException exception )
            {
                MessageBox.Show("The values you have given are invalid. Please recheck them\n");
                Console.Out.WriteLine(exception);
            }
        }

        private void RulesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rule rule = GetSelectedRule();

            if( rule == null )
                return;

            RulesF1GraphLineSeries.ItemsSource = BuildGraph(rule.F1);
            RulesF2GraphLineSeries.ItemsSource = BuildGraph(rule.F2);
            RulesOutputGraphLineSeries.ItemsSource = BuildGraph(rule.Output);
        }

        private void IterationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (int)IterationComboBox.SelectedItem;
            double value = simulator.controller.OutputValueList[index];

            HistoryOutputGraphLineSeries.ItemsSource = BuildGraph(simulator.controller.OutputGraphList[index]);
            HistoryOutputValueLineSeries.ItemsSource = new Point[] { new Point(value, 0), new Point(value, 1) };
            HistoryOutputValueTextBlock.Text = value.ToString();
            HistoryOutputRuleTextBlock.Text = (simulator.controller.OutputRuleList[index]).Label;
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            RuleWindow window = new RuleWindow();
            window.Closed += delegate
            {
                if (window.Result != null)
                    AddRule(window.Result);
            };
            window.Show();
        }

        private void RemoveRuleButton_Click(object sender, RoutedEventArgs e)
        {
            if (RulesListBox.SelectedIndex == -1)
                return;

            RemoveRule(RulesListBox.SelectedIndex);
        }

        private void EditRuleButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedRule() == null)
                return;

            int Index = RulesListBox.SelectedIndex;
            RuleWindow window = new RuleWindow((Rule)GetSelectedRule());
            window.Closed += delegate
            {
                if (window.Result != null)
                    EditRule(Index, window.Result);
            };
            window.Show();
        }

        private void HistoryOutputRulesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MainPECPEGraph.SetCurrent((int)HistoryOutputRulesListBox.SelectedItem);
        }

        #endregion
    }
}