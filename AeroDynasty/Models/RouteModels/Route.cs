using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;
using AeroDynasty.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Documents;

namespace AeroDynasty.Models.RouteModels
{
    public class Route
    {
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public Airline routeOwner { get; set; }
        public Price ticketPrice { get; set; }
        public int flightFrequency { get; set; }
        public ObservableCollection<FlightSchedule> scheduledFlights { get; set; }

        public Route()
        {
            Origin = null;
            Destination = null;
            routeOwner = null;
            ticketPrice = null;
            scheduledFlights = new ObservableCollection<FlightSchedule>();
        }

        public Route(Airport origin, Airport destination, Airline owner, Price ticketprice, int flightfrequency)
        {
            // Check if parameters are null and throw an exception if they are
            Origin = origin ?? throw new ArgumentNullException(nameof(origin), "Origin airport cannot be null.");
            Destination = destination ?? throw new ArgumentNullException(nameof(destination), "Destination airport cannot be null.");
            routeOwner = owner ?? throw new ArgumentNullException(nameof(owner), "Airline cannot be null.");
            ticketPrice = ticketprice ?? throw new ArgumentNullException(nameof(ticketprice), "Price cannot be null.");
            flightFrequency = flightfrequency;
            scheduledFlights = new ObservableCollection<FlightSchedule>();
        }

        /// <summary>
        /// Updates the route based on the route that is given
        /// </summary>
        /// <param name="route">The route to base the update on</param>
        public void UpdateWith(Route route)
        {
            Origin = route.Origin;
            Destination = route.Destination;
            routeOwner = route.routeOwner;
            ticketPrice = route.ticketPrice;
            flightFrequency = route.flightFrequency;
        }

        /// <summary>
        /// Returns the route name based on the origin and destination
        /// </summary>
        public string name { get
            {
                try
                {
                    return Origin.ICAO + " - " + Destination.ICAO;
                }
                catch(Exception ex)
                {
                    Trace.TraceWarning($"Could not retrieve route name. {this}");
                    Trace.TraceWarning(ex.Message);
                    Trace.TraceInformation(ex.StackTrace);
                    return " - ";
                }
            } 
        }

        /// <summary>
        /// Returns the route distance in km based on the origin to destination
        /// </summary>
        public double routeDistance
        {
            get
            {
                return GeoUtilities.CalculateDistance(Origin.Coordinates, Destination.Coordinates);
            }
        }

        /// <summary>
        /// Delete this route from the routes collections.
        /// </summary>
        public void deleteRoute()
        {
            try
            {
                var collection = GameData.Instance.Routes;

                //Make sure the route exists before deleting it.
                if (collection != null && collection.Contains(this))
                {
                    collection.Remove(this);
                }
            }catch(Exception ex) {
                Trace.WriteLine($"Could not delete route {this.name}");
                Trace.TraceWarning(ex.Message);
                Trace.TraceWarning(ex.StackTrace);
            }
            
        }

    }
}
