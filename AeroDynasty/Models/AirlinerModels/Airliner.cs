using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.AirlinerModels
{
    public class Airliner
    {
        public string registration {  get; set; }
        public string model {  get; set; }
        public double fuelConsumption {  get; set; }
        public double maintenanceCost { get; set; }
        public double range {  get; set; }
    }
}
