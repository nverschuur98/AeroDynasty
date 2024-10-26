using AeroDynasty.Models.RouteModels;
using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AeroDynasty.Services;

namespace AeroDynasty.ModelViews.RouteViewModels
{
    public class NewRouteViewModel : BaseViewModel
    {
        #region Declaring of variables
        
        //Private variables
        private GameViewModel _gameViewModel;
        private RoutesViewModel _routesViewModel;
        private Route _route;
        private Route _referenceRoute;
        private bool isEdit;

        private ICollectionView _originAirports;
        private Airport _selectedOriginAirport;

        private ICollectionView _destinationAirports;
        private Airport _selectedDestinationAirport;
        private bool _destinationAirportsEnabled;

        private ICollectionView _airlines;
        private Airline _selectedAirline;

        private Price _ticketprice;
        private int _frequency;

        //Commandos
        public ICommand ButtonSaveCommand { get; private set; }
        public ICommand ButtonCancelCommand { get; private set; }

        #endregion

        // Shared initialization logic
        private void InitializeViewModel()
        {
            OriginAirports = CollectionViewSource.GetDefaultView(GameData.Instance.Airports.OrderBy(Airport => Airport.Name));
            DestinationAirports = CollectionViewSource.GetDefaultView(GameData.Instance.Airports.OrderBy(Airport => Airport.Name));
            DestinationAirports.Filter = DestinationAirportsFilter;
            DestinationAirportsEnabled = false;

            Ticketprice = new Price(0);
            Airlines = CollectionViewSource.GetDefaultView(GameData.Instance.Airlines.OrderBy(Airline => Airline.Name));

            // Commands
            ButtonSaveCommand = new RelayCommand(ButtonSave);
            ButtonCancelCommand = new RelayCommand(ButtonCancel);
        }

        // Constructor for creating a new route
        public NewRouteViewModel(GameViewModel gameViewModel, RoutesViewModel routesViewModel): this(gameViewModel, routesViewModel, null)
        {
            Route = new Route();
        }

        // Constructor for editing an existing route
        public NewRouteViewModel(GameViewModel gameViewModel, RoutesViewModel routesViewModel, Route route)
        {
            _gameViewModel = gameViewModel;
            _routesViewModel = routesViewModel;

            InitializeViewModel();

            Route = _referenceRoute = route;

            // If route is not null, it's an edit mode
            isEdit = Route != null;

        }

        #region Construction of variables

        //Setup public variables to local
        public ICollectionView OriginAirports
        {
            get => _originAirports;
            private set
            {
                _originAirports = value;
                OnPropertyChanged(nameof(OriginAirports));
            }
        }

        public Airport SelectedOriginAirport
        {
            get => _selectedOriginAirport;
            set
            {
                _selectedOriginAirport = value;

                //Enable the destination field if originAirport is not empty
                if (_selectedOriginAirport != null)
                {
                    DestinationAirportsEnabled = true;
                }
                else
                {
                    DestinationAirportsEnabled = false;
                }

                // Refresh the destination airport filter
                DestinationAirports.Refresh(); // Refresh the view to apply the filter

                OnPropertyChanged(nameof(SelectedOriginAirport));
            }
        }

        public ICollectionView DestinationAirports
        {
            get => _destinationAirports;
            private set
            {
                _destinationAirports = value;
                OnPropertyChanged(nameof(DestinationAirports));
            }
        }

        public Airport SelectedDestinationAirport
        {
            get => _selectedDestinationAirport;
            set
            {
                _selectedDestinationAirport = value;
                OnPropertyChanged(nameof(SelectedDestinationAirport));
            }
        }

        public bool DestinationAirportsEnabled
        {
            get => _destinationAirportsEnabled;
            set
            {
                _destinationAirportsEnabled = value;
                OnPropertyChanged(nameof(DestinationAirportsEnabled));
            }
        }

        private bool DestinationAirportsFilter(object obj)
        {
            Airport airport = obj as Airport;
            return airport != null && airport != SelectedOriginAirport;
        }

        public ICollectionView Airlines
        {
            get => _airlines;
            set
            {
                _airlines = value;
                OnPropertyChanged(nameof(Airlines));
            }
        }

        public Airline SelectedAirline
        {
            get => _selectedAirline;
            set
            {
                _selectedAirline = value;
                OnPropertyChanged(nameof(SelectedAirline));
            }
        }

        public Price Ticketprice
        {
            get => _ticketprice;
            set
            {
                try
                {
                    _ticketprice = value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not convert {value} to a Price: {ex.Message}");
                }

                OnPropertyChanged(nameof(Ticketprice));
                
            }
        }

        public double TicketpriceAmount
        {
            get
            {
                _ticketprice ??= new Price(0); // Initialize _ticketprice if it's null
                return _ticketprice.Amount;
            }
            set
            {
                
                try
                {
                    _ticketprice ??= new Price(0); // Ensure _ticketprice is initialized before setting
                    _ticketprice.Amount = value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not convert {value} to a double: {ex.Message}");
                }

                OnPropertyChanged(nameof(TicketpriceAmount));

            }
        }

        public int Frequency
        {
            get => _frequency;
            set
            {
                try
                {
                    _frequency = Convert.ToInt32(value);
                }
                catch
                {
                    throw new Exception($"Could not convert {value} to a double");
                }

                OnPropertyChanged(nameof(Frequency));
            }
        }

        public Route Route
        {
            get => _route;
            set
            {
                try
                {
                    _route = value;
                    if(value != null)
                    {
                        SelectedOriginAirport = value.Origin;
                        SelectedDestinationAirport = value.Destination;
                        SelectedAirline = value.routeOwner;
                        Ticketprice = value.ticketPrice;
                        Frequency = value.flightFrequency;
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not convert {value} to a Route: {ex.Message}");
                }

                OnPropertyChanged(nameof(Route));
            }

        }

        //Setup commando handling
        private void ButtonSave()
        {
            if (SaveNewRoute())
            {
                _routesViewModel.Routes.Refresh();
                _routesViewModel.CurrentDetailViewModel = new ViewRouteViewModel(_gameViewModel, _routesViewModel, Route);
            }
        }

        private void ButtonCancel()
        {
            _routesViewModel.CurrentDetailViewModel = null;
        }

        #endregion

        #region Functions

        //Private functions
        private bool SaveNewRoute()
        {
            //Check if everything is entered
            if (SelectedOriginAirport == null)
            {
                MessageBox.Show("The selected origin airport cannot be empty");
                return false;
            }

            if (SelectedDestinationAirport == null)
            {
                MessageBox.Show("The selected destination airport cannot be empty");
                return false;
            }

            if (SelectedAirline == null)
            {
                MessageBox.Show("The selected airline cannot be empty");
                return false;
            }

            if (Ticketprice == null)
            {
                MessageBox.Show("The price cannot be null");
                return false;
            }

            if (Frequency < 0)
            {
                MessageBox.Show("The frequency cannot be lower then 0");
                return false;
            }

            try
            {

                //Create new route instance
                Route = new Route(SelectedOriginAirport, SelectedDestinationAirport, SelectedAirline, Ticketprice, Frequency);

                if (isEdit)
                {
                    //Overwrite the route with the new details
                    _referenceRoute.UpdateWith(Route);
                }
                else
                {
                    //Add route to global collection
                    GameData.Instance.Routes.Add(Route);
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        //Public functions

        #endregion
    }
}
