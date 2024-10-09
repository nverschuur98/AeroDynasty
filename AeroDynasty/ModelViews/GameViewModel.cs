using AeroDynasty.Models;
using AeroDynasty.Models.Core;
using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.RouteModels;
using AeroDynasty.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace AeroDynasty.ModelViews
{
    public class GameViewModel : BaseViewModel
    {
        private object _currentViewModel;
        private DateTime _currentDate;
        private bool _isPaused;

        //Maps
        private Dictionary<string, Country> _countryMap;
        public Dictionary<string, Country> CountryMap => _countryMap; // Expose CountryMap for GameStateManager

        //Observable Collections
        public ObservableCollection<Airline> Airlines { get; set; }
        public ObservableCollection<Airport> Airports { get; set; }
        public ObservableCollection<Country> Countries { get; set; }
        public ObservableCollection<Route> Routes { get; set; }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged();
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

        #region COMMANDOS
        public ICommand PlayCommand { get; set; }
        public ICommand PauseCommand { get; set; }

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAirportsCommand { get; }
        public ICommand NavigateAirlinesCommand { get; }
        public ICommand NavigateRoutesCommand { get; }
        public ICommand NavigateAirlinersCommand { get; }
        /*public ICommand NavigateFinancesCommand { get; }*/
        public ICommand NavigateSettingsCommand { get; }
        #endregion

        public GameViewModel()
        {
            //Init start date
            _currentDate = new DateTime(1946,1,1);

            //Init lists
            LoadCountries();
            LoadAirlines();
            LoadAirports();
            LoadRoutes();

            //Commands to handle the game
            PlayCommand = new RelayCommand(PlayGame);
            PauseCommand = new RelayCommand(PauseGame);

            //Commands to handle menu
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            NavigateAirportsCommand = new RelayCommand(NavigateAirports);
            NavigateAirlinesCommand = new RelayCommand(NavigateAirlines);
            NavigateRoutesCommand = new RelayCommand(NavigateRoutes);
            NavigateAirlinersCommand = new RelayCommand(NavigateAirliners);
            NavigateSettingsCommand = new RelayCommand(NavigateSettings);

            //Pause the game at start
            _isPaused = false;

            //Set home screen
            NavigateHome();
        }

        #region COMMANDO FUNCTIONS
        private void PlayGame()
        {
            IsPaused = false;
            StartGameTimer();
        }

        private void PauseGame()
        {
            IsPaused = true;
        }

        private void NavigateHome()
        {
            CurrentViewModel = new HomeViewModel();
        }

        private void NavigateAirports()
        {
            CurrentViewModel = new AirportsViewModel(this);  // Pass the current instance of GameViewModel
        }

        private void NavigateAirlines()
        {
            CurrentViewModel = new AirlinesViewModel(this);  // Pass the current instance of GameViewModel
        }

        private void NavigateRoutes()
        {
            CurrentViewModel = new RoutesViewModel(this);  // Pass the current instance of GameViewModel
        }

        private void NavigateAirliners()
        {
            CurrentViewModel = new AirlinersViewModel(this);  // Pass the current instance of GameViewModel
        }

        private void NavigateSettings()
        {
            CurrentViewModel = new SettingsViewModel(this);  // Pass the current instance of GameViewModel
        }
        #endregion

        #region FUNCTIONS
        private async void StartGameTimer()
        {
            while (!IsPaused)
            {
                await Task.Delay(1000); //Simulate a day per second
                CurrentDate = CurrentDate.AddDays(1);
                //UpdateAirlines();
            }
        }

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
                    if (_countryMap.TryGetValue(countrycode, out var country))
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

                foreach(JsonElement airportData in JSONRoot.EnumerateArray())
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
                    if (_countryMap.TryGetValue(countryCode, out var country))
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

        private void LoadCountries()
        {
            var countries = JSONDataLoader.LoadFromFile<Country>("Assets/CountryData.json");
            Countries = new ObservableCollection<Country>(countries);

            // Create a mapping of CountryCode to Country instance
            _countryMap = Countries.ToDictionary(c => c.ISOCode, c => c);
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
