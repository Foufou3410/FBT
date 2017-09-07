using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.PointShapeLine
{
    public partial class PointShapeLineExample : UserControl
    {
        public PointShapeLineExample()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }   
    }
}