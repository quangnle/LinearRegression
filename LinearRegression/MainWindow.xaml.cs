using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinearRegression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] _headers = null;
        private List<double> _input = null;
        private List<double> _output = null;

        public MainWindow()
        {
            InitializeComponent();
            cboDegree.ItemsSource = ("1,2,3,4,5").Split(',');
        }

        private void btnVisualize_Click(object sender, RoutedEventArgs e)
        {
            var xCol = cboX.SelectedItem as string;
            var yCol = cboY.SelectedItem as string;

            _input = new List<double>();
            _output = new List<double>();

            foreach (DataRow dr in (dataGrid.ItemsSource as DataView).Table.Rows)
            {
                _input.Add(Convert.ToDouble(dr[xCol]));
                _output.Add(Convert.ToDouble(dr[yCol]));
            }

            var degree = Convert.ToInt32(cboDegree.SelectedItem as string);

            FrmChart fChart = new FrmChart(_input, _output, degree);
            
            fChart.Width = 800;
            fChart.Height = 700;
            fChart.ShowDialog();
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            var fileName = dlg.FileName;
            if (File.Exists(fileName))
            {
                try
                {
                    var lines = File.ReadAllLines(fileName);
                    _headers = lines[0].Split(',');

                    cboX.ItemsSource = _headers;
                    cboY.ItemsSource = _headers;

                    DataTable dt = new DataTable();

                    foreach (var colName in _headers)
                    {
                        dt.Columns.Add(colName);
                    }

                    for (int i = 1; i < lines.Length; i++)
                    {
                        var items = lines[i].Split(',');
                        var dr = dt.NewRow();
                        for (int j = 0; j < items.Length; j++)
                        {
                            dr[j] = Convert.ToDouble(items[j]);
                        }
                        dt.Rows.Add(dr);
                    }

                    dataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception)
                {
                    MessageBox.Show("The selected file is in incorrected format of CSV. Please check again.");
                }
            }
        }
    }
}
