using System;
namespace TrustablePerimeter.Entities
{
    public class Account : AbstractEntity
    {
        public float Money { get; set; }
        public Customer Customer { get; set; }
        public int Id { get; set; }

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

        public override string ToString()
        {
            return "Account n°" + Id;
        }

        public Account()
        {
        }
    }
}
