using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models
{
    public class Route
    {
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public Airline routeOwner { get; set; }
        //public Airliner assignedAirliner { get; set; }
        public double ticketPrice { get; set; }
        public int flightFrequency { get; set; }

        public Route(Airport origin, Airport destination, Airline owner, double ticketprice, int flightfrequency)
        {
            // Check if parameters are null and throw an exception if they are
            Origin = origin ?? throw new ArgumentNullException(nameof(origin), "Origin airport cannot be null.");
            Destination = destination ?? throw new ArgumentNullException(nameof(destination), "Destination airport cannot be null.");
            routeOwner = owner ?? throw new ArgumentNullException(nameof(owner), "Airline cannot be null.");
            ticketPrice = ticketprice;
            flightFrequency = flightfrequency;
        }

        /// <summary>
        /// Returns the route name based on the origin and destination
        /// </summary>
        public string name { get
            {
                return Origin.ICAO + " - " + Destination.ICAO; 
            } 
        }

        /// <summary>
        /// Return the origin in "(ICAO) Name" format
        /// </summary>
        public string OriginICAO_Name
        {
            get
            {
                return "(" + Origin.ICAO + ") " + Origin.Name;
            }
        }

        /// <summary>
        /// Return the destination in "(ICAO) Name" format
        /// </summary>
        public string DestinationICAO_Name
        {
            get
            {
                return "(" + Destination.ICAO + ") " + Destination.Name;
            }
        }
    }
}
