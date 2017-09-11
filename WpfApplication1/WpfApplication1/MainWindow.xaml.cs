using System;
using System.Windows.Controls;
using System.Collections.Generic;
using FBT;
using System.Windows;

namespace FBT
{
    public partial class MainWindow : UserControl
    {
 
        public MainWindow()
        {
   
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

     

    }
}