using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LinearRegression
{
    /// <summary>
    /// Interaction logic for FrmChart.xaml
    /// </summary>
    public partial class FrmChart : Window
    {
        public FrmChart(List<double> input, List<double> output, int degree)
        {
            InitializeComponent();

            if (degree < 1 || degree > 5) degree = 1;

            var x = ProcessInput(input, degree);
            var y = ProcessOutput(output);
            var h = Solve(x, y);
            LoadChart(input, output, h);
        }

        private Matrix<double> ProcessInput(List<double> input, int degree)
        {   
            var x = DenseMatrix.Create(input.Count, degree + 1, 1.0);

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < degree + 1; j++)
                {
                    x[i, j] = Math.Pow(input[i], j);
                }
            }

            return x;
        }

        private Matrix<double> ProcessOutput(List<double> output)
        {
            var y = DenseMatrix.Create(output.Count, 1, 1.0);

            for (int i = 0; i < output.Count; i++)
            {
                y[i, 0] = output[i];
            }

            return y;
        }

        private List<double> Solve(Matrix<double> x, Matrix<double> y)
        {
            var regression = new ClosedFormLR();
            var theta = regression.Regression(x, y);

            List<double> h = new List<double>();
            for (int i = 0; i < x.RowCount; i++)
            {
                var element = x.SubMatrix(i, 1, 0, x.ColumnCount);
                var z = element * theta;
                h.Add(z[0,0]);
            }

            return h;
        }

        private void LoadChart(List<double> x, List<double> y, List<double> h)
        {
            List<KeyValuePair<double, double>> valueList = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> hValueList = new List<KeyValuePair<double, double>>();            

            for (int i = 0; i < x.Count; i++)
            {
                var p = new KeyValuePair<double, double>(x[i], y[i]);
                valueList.Add(p);
            }
            
            for (int i = 0; i < x.Count; i++)
            {   
                var p = new KeyValuePair<double, double>(x[i], h[i]);
                hValueList.Add(p);
            }

            ScatterSeries realPointSeries = new ScatterSeries();
            realPointSeries.DependentValuePath = "Value";
            realPointSeries.IndependentValuePath = "Key";
            realPointSeries.IsSelectionEnabled = true;

            LineSeries hypoPointSeries = new LineSeries();
            hypoPointSeries.DependentValuePath = "Value";
            hypoPointSeries.IndependentValuePath = "Key";
            hypoPointSeries.IsSelectionEnabled = true;

            Chart chart = new Chart();
            chart.Title = "Regression Chart";
            chart.VerticalAlignment = VerticalAlignment.Center;
            chart.Height = 600;

            chart.Series.Add(realPointSeries);
            ((ScatterSeries)chart.Series[0]).ItemsSource = valueList;
            ((ScatterSeries)chart.Series[0]).Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            ((ScatterSeries)chart.Series[0]).Title = "Real Data Points";
            chart.Series.Add(hypoPointSeries);
            ((LineSeries)chart.Series[1]).ItemsSource = hValueList;
            ((LineSeries)chart.Series[1]).Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            ((LineSeries)chart.Series[1]).Title = "Predicted Data Points";
            
            this.grdArea.Children.Add(chart);
        }
    }
}
