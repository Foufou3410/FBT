﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppelWRE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppelWRE.Tests
{
    [TestClass()]
    public class WRETests
    {
        [TestMethod()]
        public void computeCorrelationMatrixTest()
        {
            // header
            Debug.WriteLine("******************************");
            Debug.WriteLine("*    WREmodelingCorr in C#   *");
            Debug.WriteLine("******************************");

            // sample data
            double[,] returns = { {0.05, 0.05, 0.6}, {-0.001, -0.001, 0.56}, {0.7, 0.7, 0.12}, {-0.3, -0.3, -0.1},
                                {0.1, 0.1, 0.3}};

            // call WRE via computeCovarianceMatrix encapsulation
            WRE classWRE = new WRE();
            double[,] myCorrMatrix = WRE.computeCorrelationMatrix(returns);

            // display result
            WRE.dispMatrix(myCorrMatrix);

            // ending the program            
            Console.WriteLine("\nType enter to exit");
        }
    }
}