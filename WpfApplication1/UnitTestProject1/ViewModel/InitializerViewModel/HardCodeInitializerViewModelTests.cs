﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using FBT.ViewModel.InitializerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBT.ViewModel.InitializerViewModel.Tests
{
    [TestClass()]
    public class HardCodeInitializerViewModelTests
    {
        [TestMethod()]
        public void HardCodeInitializerViewModelTest()
        {
            var init = new HardCodeInitializerViewModel();
            Assert.AreEqual(init.Name, "HardCode");
        }
    }
}