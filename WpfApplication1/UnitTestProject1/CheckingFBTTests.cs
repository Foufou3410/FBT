using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WpfApplication1.Tests
{
    [TestClass()]
    public class CheckingFBTTests
    {
        CheckingFBT fb = new CheckingFBT(5d);
        [TestMethod()]
        public void WithdrawTest()
        {
            Assert.AreEqual(fb.m_balance, 5d);
            fb.Withdraw(4d);
            Assert.AreEqual(fb.m_balance, 1d);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException),"Withdrawal exceeds balance!")]
        public void WithdrawCS()
        {
            fb.Withdraw(6d);
        }
    }
}