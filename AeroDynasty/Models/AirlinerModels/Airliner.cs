using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Models.AirlineModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.AirlinerModels
{
    public class Airliner
    {
        public string Registration {  get; set; }
        public AircraftModel Model {  get; set; }
        public Airline Owner { get; set; }

        public bool PlayerOwned
        {
            get
            {
                return true;
            }
        }
    }
}
