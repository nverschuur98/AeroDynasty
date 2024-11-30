using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;

namespace AeroDynasty.Models.RouteModels
{
    public class FlightLeg
    {
        public TimeSpan DepartureTime { get; set; }
        public DayOfWeek DepartureDay { get; set; }
        public TimeSpan FlightDuration { get { return _flightDuration; } }
        public TimeSpan ArrivalTime { get { return _arrivalTime; } }
        public DayOfWeek ArrivalDay { get { return _arrivalDay; } }
        public Airliner AssignedAirliner { get; set; }
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public String Name { get { return _name; } }

        public FlightLeg(TimeSpan departureTime, DayOfWeek departureDay, Airliner assignedAirliner, Airport origin, Airport destination)
        {
            DepartureTime = departureTime;
            DepartureDay = departureDay;
            AssignedAirliner = assignedAirliner;
            Origin = origin;
            Destination = destination;
        }

        private double _distance
        {
            get
            {
                return GeoUtilities.CalculateDistance(Origin.Coordinates, Destination.Coordinates);
            }
        }

        private TimeSpan _flightDuration
        {
            get
            {
                TimeSpan flightTime = TimeSpan.FromHours(_distance / AssignedAirliner.Model.CruisingSpeed);
                return new TimeSpan(0, (int)Math.Ceiling(flightTime.TotalMinutes), 0);
            }
        }

        private TimeSpan _arrivalTime
        {
            get
            {
                return DepartureTime + FlightDuration;
            }
        }

        private DayOfWeek _arrivalDay
        {
            get
            {
                if(ArrivalTime < DepartureTime)
                {
                    return (DayOfWeek)(((int)DepartureDay + 1) % 7);
                }
                else
                {
                    return DepartureDay;
                }
            }
        }

        private String _name
        {
            get
            {
                return Origin.ICAO + " - " + Destination.ICAO;
            }
        }
    }
}
