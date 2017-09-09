using Microsoft.VisualStudio.TestTools.UnitTesting;
using FBT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FBT.ViewModel.Tests
{
    [TestClass()]
    public class ManagerVMTests
    {
        [TestMethod()]
        public void ManagerVMTest()
        {
            DateTime theDate = new DateTime(1900, 1, 1);
            ManagerVM boss = new ManagerVM(theDate, "365", "2");
            
        }

        [TestMethod()]
        public void pleaseUpdateManagerTest()
        {
            throw new NotImplementedException();
        }
    }
}