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
    public partial class PECPEGraph : UserControl
    {
        public Rectangle[] Rules = new Rectangle[17];
        public double ColumnWidth;
        public double RowHeight;

        public PECPEGraph()
        {
            InitializeComponent();

            List<TextBlock> textblocks = new List<TextBlock>();

            foreach (Rectangle rect in RootGrid.Children)
            {
                int Index = getIndex(rect.Name);
                Rules[Index] = rect;

                TextBlock text = new TextBlock();
                Grid.SetRow(text, Grid.GetRow(rect));
                Grid.SetColumn(text, Grid.GetColumn(rect));
                Grid.SetRowSpan(text, Grid.GetRowSpan(rect));
                Grid.SetColumnSpan(text, Grid.GetColumnSpan(rect));
                text.Text = "Rule "+(Index+1);
                text.Style = (Style)this.Resources["ContentTextBlockStyle"];
                textblocks.Add(text);
            }

            //makes sure textblocks are added after all calculations
            foreach (TextBlock text in textblocks)
                RootGrid.Children.Add(text);

            ColumnWidth = 400 / RootGrid.ColumnDefinitions.Count;
            RowHeight = 400 / RootGrid.RowDefinitions.Count;

            ClearLines();
        }

        private int getIndex(String Name)
        {
            String buffer = "";
            foreach (char c in Name)
            {
                if (c >= '0' && c <= '9')
                    buffer += c;
            }

            return Convert.ToInt32(buffer) - 1;
        }

        public void ClearLines()
        {
            RulePolygon.Points.Clear();
        }

        public void SetCurrent(int index)
        {
            if (index >= Rules.Length)
                return;

            Rectangle toRect = Rules[index];
            double multX = ColumnWidth;
            double multY = RowHeight;

            Point point = new Point();
            point.X = Grid.GetColumn(toRect) * multX + Grid.GetColumnSpan(toRect) * multX / 2;
            point.Y = Grid.GetRow(toRect) * multY + Grid.GetRowSpan(toRect) * multY / 2;
        }

        public void DrawLine(Rule rule)
        {
            int to = getIndex(rule.Label);

            if (to >= Rules.Length)
                return;

            Rectangle toRect = Rules[to];
            double multX = ColumnWidth;
            double multY = RowHeight;

            Point point = new Point();
            point.X = Grid.GetColumn(toRect) *multX + Grid.GetColumnSpan(toRect) * multX / 2;
            point.Y = Grid.GetRow(toRect) *multY + Grid.GetRowSpan(toRect) *multY / 2;

            RulePolygon.Points.Add(point);
        }
    }
}
