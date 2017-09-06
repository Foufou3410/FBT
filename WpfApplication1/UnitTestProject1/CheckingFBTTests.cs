using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WpfApplication1.Tests
{
    [TestClass()]
    public class CheckingFBTTests
    {
        [TestMethod()]
        public void WithdrawTest()
        {
            CheckingFBT fb = new CheckingFBT(5d);
            Assert.AreEqual(fb.m_balance, 5d);
            fb.Withdraw(4d);
            Assert.AreEqual(fb.m_balance, 1d);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),"Withdrawal exceeds balance!")]
        public void WithdrawCS()
        {
            CheckingFBT fb = new CheckingFBT(5d);
            fb.Withdraw(6d);
        }
    }
}