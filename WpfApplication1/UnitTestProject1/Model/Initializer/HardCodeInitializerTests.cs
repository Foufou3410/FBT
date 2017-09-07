using Microsoft.VisualStudio.TestTools.UnitTesting;
using FBT.Model.Initializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.Model.Initializer.Tests
{
    [TestClass()]
    public class HardCodeInitializerTests
    {
        HardCodeInitializer init = new HardCodeInitializer();
        [TestMethod()]
        public void initOptionsUniversTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void getDatesOfSimuDataTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void getSpotOfSimuDataTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void getVolatilityOfSimuDataTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void initRiskFreeRateTest()
        {
            var riskFreeRate = init.initRiskFreeRate(2);
            Console.Write(riskFreeRate);
        }
    }
}