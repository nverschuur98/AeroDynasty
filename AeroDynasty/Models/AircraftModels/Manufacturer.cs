using AeroDynasty.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input.Manipulations;

namespace AeroDynasty.Models.AircraftModels
{
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public DateTime FoundingDate { get; set; }

        public string FormattedFoundingDate 
        {
            get
            {
                return FoundingDate.ToString("dd-MMM-yyyy");
            }
        }

        public Manufacturer(string name,string description, Country country, DateTime foundingDate)
        {
            Name = name;
            Description = description;
            Country = country;
            FoundingDate = foundingDate;
        }
    }

}
