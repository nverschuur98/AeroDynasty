﻿using AeroDynasty.Enums;
using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;
using AeroDynasty.Models.RouteModels;
using AeroDynasty.ModelViews;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace AeroDynasty.Services
{
    public class GameData : BaseViewModel
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
        public ObservableCollection<AircraftModel> AircraftModels { get; private set; }

        //Observable change Data
        public ObservableCollection<Route> Routes { get; set; }
        public ObservableCollection<Airliner> Airliners { get; set; }

        //Maps
        public Dictionary<string, Country> CountryMap { get; private set; }
        public Dictionary<int, double> Inflations { get; private set; }

        //Game Time and state
        public UserData UserData { get; private set; }
        private DateTime _currentDate;
        private bool _isPaused;

        //Commands
        public ICommand PlayCommand { get; set; }
        public ICommand PauseCommand { get; set; }

        //Private Constructor
        private GameData()
        {
            //Load Core first
            LoadCountries();
            LoadInflations();

            //Load non-change data
            LoadAirlines();
            LoadAirports();
            LoadManufacturers();
            LoadAircrafts();

            //Load change data
            LoadRoutes();
            LoadAirliners();

            //Init start date
            Airline arl = Airlines.Where(al => al.Name.Contains("KLM")).FirstOrDefault();

            UserData = new UserData(arl);

            CurrentDate = new DateTime(1946, 1, 1);
            _isPaused = true;

            //Bind commands
            PlayCommand = new RelayCommand(PlayGame);
            PauseCommand = new RelayCommand(PauseGame);
        }

        #region Loading functions
        /// <summary>
        /// Loading the airline data from the data files
        /// </summary>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Loading the airport data from the data files
        /// </summary>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// [TEST] Loading the routes from the save files
        /// </summary>
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

        /// <summary>
        /// Loading the country data from the data files
        /// </summary>
        private void LoadCountries()
        {
            var countries = JSONDataLoader.LoadFromFile<Country>("Assets/CountryData.json");
            Countries = new ObservableCollection<Country>(countries);

            // Create a mapping of CountryCode to Country instance
            CountryMap = Countries.ToDictionary(c => c.ISOCode, c => c);
        }

        /// <summary>
        /// Loading the inflation data from the data files
        /// </summary>
        private void LoadInflations()
        {
            string JSONString = File.ReadAllText("Assets/InflationData.json");
            JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
            JsonElement Inflation = JSONDoc.RootElement.GetProperty("inflation_data");

            Inflations = new Dictionary<int, double>();

            foreach (var year in Inflation.EnumerateObject())
            {
                int y = Convert.ToInt32(year.Name);
                double i = year.Value.GetDouble();
                Inflations.Add(y, i);
            }
        }

        /// <summary>
        /// Loading the manufacturer data from the data files
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void LoadManufacturers()
        {
            string JSONString = File.ReadAllText("Assets/ManufacturerData.json");
            JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
            JsonElement JSONRoot = JSONDoc.RootElement;

            //Create a list to hold the manufacturers
            var manufacturers = new List<Manufacturer>();

            foreach (JsonElement manufacturer in JSONRoot.EnumerateArray())
            {
                //Extract data
                string Name = manufacturer.GetProperty("Name").ToString();
                string Description = manufacturer.GetProperty("Description").ToString();
                string CountryCode = manufacturer.GetProperty("Country").ToString();
                DateTime FoundingDate;
                Country Country;

                if (!manufacturer.GetProperty("FoundingDate").TryGetDateTime(out FoundingDate))
                {
                    throw new Exception($"Failed to convert {manufacturer.GetProperty("FoundingDate").ToString()} into DateTime");
                }

                if (!CountryMap.TryGetValue(CountryCode, out Country))
                {
                    throw new Exception($"Failed to convert {CountryCode} into a country");
                }

                //Add to local list
                manufacturers.Add(new Manufacturer(Name, Description, Country, FoundingDate));

            }

            //Create the observable collection
            Manufacturers = new ObservableCollection<Manufacturer>(manufacturers);
        }

        /// <summary>
        /// Loading the aircraft data from the data files
        /// </summary>
        private void LoadAircrafts()
        {
            string JSONString = File.ReadAllText("Assets/ManufacturerData.json");
            JsonDocument JSONDoc = JsonDocument.Parse(JSONString);
            JsonElement JSONRoot = JSONDoc.RootElement;

            //Create a list to hold the aircrafts
            var aircrafts = new List<AircraftModel>();

            //Loop trough all the manufacturers and process the aircrafts
            foreach (JsonElement manufacturer in JSONRoot.EnumerateArray())
            {
                // Use LINQ to find the manufacturer by name
                Manufacturer Manufacturer = Manufacturers.FirstOrDefault(m => m.Name.Equals(manufacturer.GetProperty("Name").ToString(), StringComparison.OrdinalIgnoreCase));

                // Check if the Aircrafts property exists and is an array
                if (manufacturer.TryGetProperty("Aircrafts", out JsonElement aircraftsElement) && aircraftsElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement aircraft in aircraftsElement.EnumerateArray())
                    {
                        //Create a new aircraftModel and set the correct properties
                        AircraftModel aircraftModel = new AircraftModel(
                            aircraft.GetProperty("Name").ToString(),
                            aircraft.GetProperty("Family").ToString(),
                            Enum.Parse<AircraftType>(aircraft.GetProperty("AircraftType").ToString()),
                            Enum.Parse<EngineType>(aircraft.GetProperty("EngineType").ToString()),
                            Convert.ToInt32(aircraft.GetProperty("CruisingSpeed").ToString()),
                            Convert.ToInt32(aircraft.GetProperty("maxPax").ToString()),
                            Convert.ToInt32(aircraft.GetProperty("maxCargo").ToString()),
                            Convert.ToInt32(aircraft.GetProperty("maxRange").ToString()),
                            Convert.ToInt32(aircraft.GetProperty("minRunwayLength").ToString()),
                            Manufacturer,
                            Convert.ToDateTime(aircraft.GetProperty("IntroductionDate").ToString()),
                            Convert.ToDateTime(aircraft.GetProperty("RetirementDate").ToString()));

                        //Add the model to the aircrafts list
                        aircrafts.Add(aircraftModel);
                    }
                }

                //End of the manufacturer
            }

            //Create the observable collection
            AircraftModels = new ObservableCollection<AircraftModel>(aircrafts);

        }

        /// <summary>
        /// [TEST] Loading the airliners from the save files
        /// </summary>
        private void LoadAirliners()
        {
            Airliners = new ObservableCollection<Airliner>();

            var dum = new Airliner();
            dum.Registration = "PH-TST";
            dum.Owner = Airlines.FirstOrDefault();
            dum.Model = AircraftModels.FirstOrDefault(am => am.Name.Equals("Boeing 307 Stratoliner", StringComparison.OrdinalIgnoreCase));

            Airliners.Add(dum);
        }
        #endregion

        #region Game functions

        public string FormattedCurrentDate => CurrentDate.ToString("dd-MMM-yyyy");

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                OnPropertyChanged();
            }
        }

        private void PlayGame()
        {
            IsPaused = false;
            StartGameTimer();
        }

        private void PauseGame()
        {
            IsPaused = true;
        }

        private async void StartGameTimer()
        {
            while (!IsPaused)
            {
                await Task.Delay(1000); //Simulate a day per second
                CurrentDate = CurrentDate.AddDays(1);
                //UpdateAirlines();
            }
        }

        public void SaveGame(string filePath)
        {
            bool wasPlaying = !IsPaused;

            if (wasPlaying)
            {
                PauseCommand.Execute(null);
            }

            // Use GameStateManager to save the game
            GameStateManager.SaveGame(this, filePath);

            if (wasPlaying)
            {
                PlayCommand.Execute(null);
            }
        }

        public void LoadGame(string filePath)
        {
            // Use GameStateManager to load the game
            GameStateManager.LoadGame(this, filePath);
        }

        #endregion
    }
}
