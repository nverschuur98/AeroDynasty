using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models
{
    public class Airline
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public List<Airliner> Fleet { get; set; }
        //public List<Route> Routes { get; set; }
        public double CashBalance { get; set; }
        public double Reputation { get; set; }

        public Airline(string name, Country country, double cashbalance, double reputation)
        {
            Name = name;
            Country = country;
            CashBalance = cashbalance;
            Reputation = reputation;

            Fleet = new List<Airliner>();
            //Routes = new List<Route>();
        }
    }
}
