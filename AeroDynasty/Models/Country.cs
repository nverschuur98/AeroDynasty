using AeroDynasty.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public Continent Continent { get; set; }

    }
}
