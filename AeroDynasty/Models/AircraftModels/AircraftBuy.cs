using AeroDynasty.Models.AirlineModels;

namespace AeroDynasty.Models.AircraftModels
{
    public class AircraftBuy
    {
        public AircraftModel AircraftModel { get; private set; }
        public int Amount { get; set; }
        public Airline Buyer { get; set; }
    }
}
