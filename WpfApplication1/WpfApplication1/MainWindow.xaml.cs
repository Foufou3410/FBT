using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.PointShapeLine
{
    public partial class PointShapeLineExample : UserControl
    {
        private List<String> viewTypesList;
        private String startDate;
       //private Textbox dateBox;

        
        public PointShapeLineExample()
        {
            InitializeComponent();
            this.DataContext = new MainWindowView();
            //var dateBox = new TextBox();
            viewTypesList = new List<string>();
            viewTypesList.Add("Historique");
            viewTypesList.Add("Simulées");

        }


    private void Add_Click(object sender, RoutedEventArgs e)
    {
        int num1 = int.Parse(dateBox.Text);
        //Result.Text = (num1).ToString();
        MessageBox.Show(num1.ToString());
        // MessageBox.Show("The Result is: " + Result.Text);
    }

    public List<String> valuesType
    {
        get
        {
            return viewTypesList;
        }
    }

    public String selectedValuesType { get; set; }

  

    //Tickerstarted => retrieve selected values 

}

}