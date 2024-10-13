using AeroDynasty.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.AircraftModels
{
    public class AircraftModel
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public int maxPax { get; set; }
        public int maxCargo { get; set; }
        public double maxRange { get; set; }
        public int minRunwayLength { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public DateTime IntroductionDate { get; set; }
        public string FormattedIntroductionDate => IntroductionDate.ToString("dd-MMM-yyyy");
        public DateTime RetirementDate { get; set; }
        public string FormattedRetirementDate => RetirementDate.ToString("dd-MMM-yyyy");
        public AircraftType Type { get; set; }
    }
}
