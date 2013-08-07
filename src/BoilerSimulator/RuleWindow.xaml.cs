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
using System.Windows.Shapes;
using FuzzyLogicController;

namespace MachineLearningAssignment
{
    /// <summary>
    /// Interaction logic for NewRuleWindow.xaml
    /// </summary>
    public partial class RuleWindow : Window
    {
        public Rule Result;

        public RuleWindow()
        {
            InitializeComponent();
        }

        public RuleWindow( Rule rule )
        {
            InitializeComponent();
            PEFunctionControl.SetValues(rule.F1);
            CPEFunctionControl.SetValues(rule.F2);
            DHFunctionControl.SetValues(rule.Output);
            FunctionNameTextBox.Text = rule.Label;
            TitleTextBlock.Text = "Edit Rule";
            CreateButton.Content = "Apply Changes";
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result = new Rule(FunctionNameTextBox.Text, PEFunctionControl.CreateMembershipFunction(41, -5, 5),
                                                            CPEFunctionControl.CreateMembershipFunction(41, -5, 5),
                                                            DHFunctionControl.CreateMembershipFunction(41, -5, 5));
                this.Close();
            }
            catch (FormatException exception)
            {
                MessageBox.Show("The values you have given are invalid. Plese recheck them");
                Console.Out.WriteLine(exception);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
