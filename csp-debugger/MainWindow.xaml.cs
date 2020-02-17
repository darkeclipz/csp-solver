using Csp.Debugger.Controls;
using Csp.Debugger.Views;
using Csp.Model;
using Csp.Model.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace csp_debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GraphUserControlView graphView;

        public MainWindow()
        {
            InitializeComponent();
            graphView = new GraphUserControlView();
            graphView.Model = Models.PhoneFeatureModel();
            TabGraph.Content = new GraphUserControl(graphView);
        }

        private void SelectModelPhone_Click(object sender, RoutedEventArgs e)
        {
            graphView.Model = Models.PhoneFeatureModel();
        }

        private void SelectModelMapColoring_Click(object sender, RoutedEventArgs e)
        {
            graphView.Model = Models.AustralianMapColoringModel();
        }

        private void SelectModelMiniSudoku_Click(object sender, RoutedEventArgs e)
        {
            graphView.Model = Models.SudokuSmallModel();
        }
    }
}
