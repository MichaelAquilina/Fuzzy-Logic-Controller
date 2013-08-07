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
    /// <summary>
    /// Interaction logic for FunctionControl.xaml
    /// </summary>
    public partial class FunctionControl : UserControl
    {
        public double a
        {
            get { return Convert.ToDouble(AParameterTextBox.Text); }
            set { AParameterTextBox.Text = value.ToString(); }
        }
        public double b
        {
            get { return Convert.ToDouble(BParameterTextBox.Text); }
            set { BParameterTextBox.Text = value.ToString(); }
        }
        public double alpha
        {
            get { return Convert.ToDouble(AlphaParameterTextBox.Text); }
            set { AlphaParameterTextBox.Text = value.ToString(); }
        }
        public double beta
        {
            get { return Convert.ToDouble(BetaParameterTextBox.Text); }
            set { BetaParameterTextBox.Text = value.ToString(); }
        }

        public FunctionControl()
        {
            InitializeComponent();
        }

        public FunctionControl(MembershipFunction function)
        {
            InitializeComponent();
            SetValues(function);
        }

        public void SetValues(MembershipFunction function)
        {
            a = function.a;
            b = function.b;
            alpha = function.alpha;
            beta = function.beta;  
        }

        public MembershipFunction CreateMembershipFunction( int samples, double min, double max )
        {
            return new MembershipFunction("", a, b, alpha, beta, samples, min, max);
        }
    }
}
