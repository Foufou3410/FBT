using FBT;
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

namespace FBT
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<String> viewTypesList;
        private String startDate;
       //private Textbox dateBox;

        public MainWindow()
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