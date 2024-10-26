using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AeroDynasty.Models.Core;
using AeroDynasty.Models.AirlinerModels;
using System.Security.Policy;
using System.ComponentModel;
using AeroDynasty.Models.RouteModels;
using AeroDynasty.Services;

namespace AeroDynasty.Models.AirlineModels
{
    public class Airline : BaseModel
    {
        private double _cashBalance { get; set; }
        private string _name { get; set; }
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public Country Country { get; set; }
        public double Reputation { get; set; }
        public double CashBalance
        {
            get
            {
                return _cashBalance;
            }
            set
            {
                _cashBalance = value;
                OnPropertyChanged(nameof(CashBalance));
            }
        }
        public List<Airliner> Fleet { get
            {
                if(GameData.Instance.Airliners != null)
                {
                    return new List<Airliner>(GameData.Instance.Airliners.Where(air => air.Owner == this));
                }

                return new List<Airliner>();
            }
        }
        public List<Route> Routes { get
            {
                if (GameData.Instance.Routes != null)
                {
                    return new List<Route>(GameData.Instance.Routes.Where(route => route.routeOwner == this));
                }

                return new List<Route>();
            }
        }

        public Airline(string name, Country country, double cashbalance, double reputation)
        {
            Name = name;
            Country = country;
            CashBalance = cashbalance;
            Reputation = reputation;

        }

        #region Cash Transactions
        /// <summary>
        /// Add an amount to the cash balance
        /// </summary>
        /// <param name="amount">the amount to add</param>
        /// <exception cref="Exception"></exception>
        public void addCash(double amount)
        {
            if (amount < 0)
            {
                throw new Exception("Huh why subtract when you want to add");
            }

            CashBalance += amount;
        }

        /// <summary>
        /// Returns if there is sufficient cash
        /// </summary>
        /// <param name="amount">the amount to pay</param>
        /// <returns></returns>
        private bool _sufficientCash(double amount)
        {
            if (amount < CashBalance)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if there is sufficient cash
        /// </summary>
        /// <param name="amount">the amount to pay</param>
        /// <returns></returns>
        public bool SufficientCash(Price amount)
        {
            return _sufficientCash(amount.Amount);
        }

        /// <summary>
        /// Returns if there is sufficient cash
        /// </summary>
        /// <param name="amount">the amount to pay</param>
        /// <returns></returns>
        public bool SufficientCash(double amount)
        {
            return _sufficientCash(amount);
        }

        /// <summary>
        /// Subtracts an amount from the cash balance
        /// </summary>
        /// <param name="amount"></param>
        /// <exception cref="Exception"></exception>
        private void _subtractCash(double amount)
        {
            if (SufficientCash(amount))
            {
                CashBalance -= amount;
            }
            else
            {
                throw new Exception("Not enough cash");
            }
        }

        /// <summary>
        /// Subtract an amount from the cash balance
        /// </summary>
        /// <param name="amount">the amount to subtract</param>
        public void SubtractCash(Price amount)
        {
            _subtractCash(amount.Amount);
        }

        /// <summary>
        /// Subtract an amount from the cash balance
        /// </summary>
        /// <param name="amount">the amount to subtract</param>
        public void SubtractCash(Double amount)
        {
            _subtractCash(amount);
        }

        #endregion
    }
}
