using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.AirportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.RouteModels
{
    public class FlightSchedule
    {
        public FlightLeg Outbound {  get; }
        public FlightLeg Inbound { get; }
        public TimeSpan TurnaroundTime { get; }

        public FlightSchedule(Airport Origin, Airport Destination, DayOfWeek DayOfWeek, TimeSpan DepartureTime, Airliner AssignedAirliner)
        {
            TurnaroundTime = TimeSpan.FromMinutes(30);

            FlightLeg _out = new FlightLeg(DepartureTime, DayOfWeek, AssignedAirliner, Origin, Destination);
            FlightLeg _in = new FlightLeg(_out.ArrivalTime + TurnaroundTime, _out.ArrivalDay, AssignedAirliner, Destination, Origin);

            Outbound = _out;
            Inbound = _in;

        }
    }
}
