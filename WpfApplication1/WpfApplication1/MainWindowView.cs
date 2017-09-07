using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppelWRE;
using System.Diagnostics;
using System.Runtime.InteropServices;

using FBT.Model.FinancialModel;
using FBT.Model.Initializer;

namespace FBT
{
    class MainWindowView
    {


        class Prices
        {

            public Prices(decimal value, DateTime date)
            {
                this.price = value;
                this.date = date;
            }

            public DateTime date { get; set; }
            public decimal price { get; set; }
        }
        #region Public Constructors

        static void LinqSQL()
        {
            Console.WriteLine("Récupération à l'aide de LINQ; syntaxe à la SQL");
            using (DataClasses1DataContext asdc = new DataClasses1DataContext())
            {
                var q2 = (from lignes in asdc.HistoricalShareValues
                          where lignes.id == "AC FP"
                          select new Prices(lignes.value, lignes.date));
                var listPrices = new List<Prices>();
                foreach (Prices p in q2)
                {
                    listPrices.Add(p);
                    Console.WriteLine(p.date);
                    Console.WriteLine(p.price);
                }
            }
        }
        public MainWindowView()
        {

            Console.WriteLine("Démarrer");

            LinqSQL();


        }
        #endregion Public Constructors

        #region Public Properties

        public string ViewPayOff
        {
           
           get {return "coucou"; }
        }

        #endregion Public Properties

    }

}


