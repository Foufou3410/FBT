using System;

namespace WpfApplication1
{
    public class CheckingFBT
    {
        public double m_balance;

        public CheckingFBT(double current_balance)
        {
            m_balance = current_balance;
        }

        public void Withdraw(double amount)
        {
            if (m_balance >= amount)
            {
                m_balance -= amount;
            }
            else
            {
                throw new ArgumentException("Withdrawal exceeds balance!");
            }
        }
    }
}
