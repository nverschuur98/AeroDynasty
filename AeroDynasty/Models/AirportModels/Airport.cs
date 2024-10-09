using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using AeroDynasty.Models.Core;

namespace AeroDynasty.Models.AirportModels
{
    public class Airport
    {
        public string Name { get; set; }
        public string IATA { get; set; }
        public string ICAO { get; set; }
        public Country Country { get; set; }
        public Coordinates Coordinates { get; set; }
        public double demandFactor { get; set; }

        public Airport(string name, string iata, string icao, Country country, Coordinates coordinates, double demandfactor)
        {
            Name = name;
            IATA = iata;
            ICAO = icao;
            Country = country;
            Coordinates = coordinates;
            demandFactor = demandfactor;
        }

        // Assuming images are stored in the Assets folder
        public string CountryFlag
        {
            get
            {
                string path = $"Assets/Flags/{Country.ISOCode.ToLower()}.png"; // Adjust the extension as necessary
                if (!File.Exists(path))
                {
                    System.Diagnostics.Debug.WriteLine($"File not found: {path}");
                }
                return path;
            }
        }

        public string Codes
        {
            get
            {
                return ICAO + "/" + IATA;
            }
        }

        /// <summary>
        /// Return the airport in "(ICAO) Name" format
        /// </summary>
        public string ICAO_Name
        {
            get
            {
                return "(" + ICAO + ") " + Name;
            }
        }
    }
}
