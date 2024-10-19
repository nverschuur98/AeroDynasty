using System.Diagnostics;

namespace AeroDynasty.Models.Core
{
    public class Price
    {
        private double _amount { get; set; }
        public double Amount
        {
            get => _amount;
            set
            {
                _amount = Math.Round(value, 2);
            }
        }

        public Price(double amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// Calculate the inflation on the price
        /// </summary>
        /// <param name="percentage">Enter the percentage as 5.0%</param>
        public void calcInflation(double percentage)
        {
            Amount = Amount * ((100+percentage) / 100.0);
        }
    }
}
