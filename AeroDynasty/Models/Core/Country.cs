using AeroDynasty.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AeroDynasty.Models.Core
{
    public class Country
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public Continent Continent { get; set; }

        public String FlagPath { get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Flags", $"{ISOCode}.png");
            } 
        }

    }
}
