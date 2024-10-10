using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;
using AeroDynasty.Models.RouteModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AeroDynasty.Services
{
    public class GameData
    {
        //Singleton Instance
        private static GameData _instance;
        public static GameData Instance => _instance ?? (_instance = new GameData());

        //Observable Core Data
        public ObservableCollection<Country> Countries { get; private set; }

        //Observable non-change Data
        public ObservableCollection<Airline> Airlines { get; private set; }
        public ObservableCollection<Airport> Airports { get; private set; }
        public ObservableCollection<Manufacturer> Manufacturers { get; private set; }

        //Observable change Data
        public ObservableCollection<Route> Routes { get; private set; }
        public ObservableCollection<Airliner> Airliners { get; private set; }

        //Maps
        public Dictionary<string, Country> CountryMap { get; private set; }

        //Private Constructor
        private GameData()
        {
            //Load Core first
            LoadCountries();

            //Load non-change data
            LoadAirlines();
            LoadAirports();
            LoadManufacturers();

            //Load change data
            LoadRoutes();
            LoadAirliners();
        }

        //Functions
        private void LoadAirlines()
        {
            try
            {
                string JSONString = File.ReadAllText("Assets/AirlineData.json");
                JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
                JsonElement JSONRoot = JSONDoc.RootElement;

                // Create a list to hold the airlines
                var airlines = new List<Airline>();

                foreach (JsonElement airlineData in JSONRoot.EnumerateArray())
                {
                    // Extract the data
                    string name = airlineData.GetProperty("Name").ToString();
                    double cashbalance = Convert.ToDouble(airlineData.GetProperty("Cash Balance").ToString());
                    double reputation = Convert.ToDouble(airlineData.GetProperty("Reputation").ToString());
                    string countrycode = airlineData.GetProperty("CountryCode").ToString();

                    // Check if the country exists
                    if (CountryMap.TryGetValue(countrycode, out var country))
                    {
                        // Create the airline instance
                        Airline airline = new Airline(name, country, cashbalance, reputation);
                        airlines.Add(airline);
                    }
                    else
                    {
                        throw new Exception($"No such country found while creating airline: {name}.");

                    }
                }

                Airlines = new ObservableCollection<Airline>(airlines);
            }
            catch
            {
                throw new Exception("Error while creating airlines.");
            }
        }

        private void LoadAirports()
        {
            try
            {
                string JSONString = File.ReadAllText("Assets/AirportData.json");
                JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
                JsonElement JSONRoot = JSONDoc.RootElement;

                // Create a list to hold the airports
                var airports = new List<Airport>();

                foreach (JsonElement airportData in JSONRoot.EnumerateArray())
                {
                    // Extract the data
                    string airportName = airportData.GetProperty("Name").ToString();
                    string countryCode = airportData.GetProperty("CountryCode").ToString();
                    string iata = airportData.GetProperty("IATA").ToString();
                    string icao = airportData.GetProperty("ICAO").ToString();
                    double demandFactor = Convert.ToDouble(airportData.GetProperty("demandFactor").ToString());

                    // Extract the coordinates
                    JsonElement _coordinates = airportData.GetProperty("Coordinates");
                    double latitude = Convert.ToDouble(_coordinates.GetProperty("Latitude").ToString());
                    double longitude = Convert.ToDouble(_coordinates.GetProperty("Longitude").ToString());
                    Coordinates coordinates = new Coordinates(latitude, longitude);

                    // Check if the country exists in the map
                    if (CountryMap.TryGetValue(countryCode, out var country))
                    {
                        // Create the Airport instance with the Country reference
                        var airport = new Airport(airportName, iata, icao, country, coordinates, demandFactor);
                        airports.Add(airport);
                    }
                    else
                    {
                        // Handle cases where the country is not found if necessary
                        // For example, you can log a warning or create a default Country instance
                        throw new Exception($"No such country found while creating airport: {airportName}.");
                    }
                }
                // Assign the populated list to the ObservableCollection
                Airports = new ObservableCollection<Airport>(airports);
            }
            catch
            {
                throw new Exception($"Error while loading airports.");
            }
        }

        private void LoadRoutes()
        {
            Airport _origin = Airports.FirstOrDefault(airport => airport.ICAO == "EHAM");
            Airport _dest1 = Airports.FirstOrDefault(airport => airport.ICAO == "LEBL");
            Airport _dest2 = Airports.FirstOrDefault(airport => airport.ICAO == "EGLL");
            Airline _air = Airlines.FirstOrDefault(airline => airline.Name.Contains("KLM"));


            //Dummy routes
            Route _route1 = new Route(_origin, _dest1, _air, 100.0, 5);
            Route _route2 = new Route(_origin, _dest2, _air, 100.0, 5);

            Routes = new ObservableCollection<Route>();

            Routes.Add(_route1);
            Routes.Add(_route2);
        }

        private void LoadCountries()
        {
            var countries = JSONDataLoader.LoadFromFile<Country>("Assets/CountryData.json");
            Countries = new ObservableCollection<Country>(countries);

            // Create a mapping of CountryCode to Country instance
            CountryMap = Countries.ToDictionary(c => c.ISOCode, c => c);
        }

        private void LoadManufacturers()
        {
            string JSONString = File.ReadAllText("Assets/ManufacturerData.json");
            JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
            JsonElement JSONRoot = JSONDoc.RootElement;

            //Create a list to hold the manufacturers
            var manufacturers = new List<Manufacturer>();

            foreach(JsonElement manufacturer in JSONRoot.EnumerateArray())
            {
                //Extract data
                string Name = manufacturer.GetProperty("Name").ToString();
                string CountryCode = manufacturer.GetProperty("Country").ToString();
                DateTime FoundingDate;
                Country Country;

                if (!manufacturer.GetProperty("FoundingDate").TryGetDateTime(out FoundingDate))
                {
                    throw new Exception($"Failed to convert {manufacturer.GetProperty("FoundingDate").ToString()} into DateTime");
                }

                if (!CountryMap.TryGetValue(CountryCode, out Country)) {
                    throw new Exception($"Failed to convert {CountryCode} into a country");
                }

                //Add to local list
                manufacturers.Add(new Manufacturer(Name, Country, FoundingDate));

            }

            //Create the observable collection
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturers);
        }

        //To Extend
        private void LoadAirliners()
        {
            Airliners = new ObservableCollection<Airliner>();

            var dum = new Airliner();
            dum.Registration = "PH-TST";
            dum.Owner = Airlines.FirstOrDefault();

            Airliners.Add(dum);
        }
    }
}
