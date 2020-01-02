using System;
namespace TrustablePerimeter.Entities
{
    public class Account
    {
        public float Money { get; private set; }
        public Customer Customer { get; private set; }

        public float Withdraw(float moneyAmount)
        {
            if ((Money - moneyAmount) >= 0)
            {
                Money = Money - moneyAmount;
                return moneyAmount;
            }
            else
            {
                throw new ArgumentException("Not enough money");
            }
        }

        public void Credit(float moneyAmount)
        {
            if (moneyAmount < 0)
            {
                throw new ArgumentException("Can not credit a negative amount of money");
            }
            Money += moneyAmount;
        }

        public Account()
        {
        }
    }
}
