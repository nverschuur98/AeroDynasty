using AeroDynasty.Enums;
using AeroDynasty.Models.Core;

namespace AeroDynasty.Models.AircraftModels
{

    public class AircraftModel
    {
        public string Name { get; private set; }
        public string Family { get; private set; }
        public Price Price { get; private set; }
        public AircraftType Type { get; private set; }
        public EngineType EngineType { get; private set; }
        public int CruisingSpeed { get; private set; } //in km/h
        public int maxPax { get; private set; }
        public int maxCargo { get; private set; } //in kilogram
        public double maxRange { get; private set; } //in kilometer
        public int minRunwayLength { get; private set; } //in meter
        public Manufacturer Manufacturer { get; private set; }
        public DateTime IntroductionDate { get; private set; }
        public string FormattedIntroductionDate => IntroductionDate.ToString("dd-MMM-yyyy");
        public DateTime RetirementDate { get; private set; }
        public string FormattedRetirementDate => RetirementDate.ToString("dd-MMM-yyyy");

        public AircraftModel(
            string name, 
            string family, 
            Price price,
            AircraftType type, 
            EngineType engineType,
            int cruisingSpeed, 
            int maxPax, 
            int maxCargo, 
            double maxRange,
            int minRunwayLength, 
            Manufacturer manufacturer,
            DateTime introductionDate, 
            DateTime retirementDate)
        {
            Name = name;
            Family = family;
            Price = price;
            Type = type;
            EngineType = engineType;
            CruisingSpeed = cruisingSpeed;
            this.maxPax = maxPax; // Use "this." to avoid shadowing, but it's not strictly necessary here.
            this.maxCargo = maxCargo; // Use "this." to avoid shadowing.
            this.maxRange = maxRange; // Use "this." to avoid shadowing.
            this.minRunwayLength = minRunwayLength; // Use "this." to avoid shadowing.
            Manufacturer = manufacturer;
            IntroductionDate = introductionDate;
            RetirementDate = retirementDate;
        }
    }
}
