using AeroDynasty.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.AircraftModels
{
    public class Manufacturer
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public DateTime FoundingDate { get; set; }
        public string FormattedFoundingDate => FoundingDate.ToString("dd-MMM-yyyy");

        public Manufacturer(string name, Country country, DateTime foundingDate)
        {
            Name = name;
            Country = country;
            FoundingDate = foundingDate;
        }
    }

}
