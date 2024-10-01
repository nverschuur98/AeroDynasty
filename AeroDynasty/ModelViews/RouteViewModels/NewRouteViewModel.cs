using AeroDynasty.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace AeroDynasty.ModelViews.RouteViewModels
{
    public class NewRouteViewModel : BaseViewModel
    {
        #region Declaring of variables
        
        //Private variables
        private GameViewModel _gameViewModel;
        private RoutesViewModel _routesViewModel;

        private ICollectionView _originAirports;
        private Airport _selectedOriginAirport;

        private ICollectionView _destinationAirports;
        private Airport _selectedDestinatonAirport;
        private bool _destinationAirportsEnabled;

        private ICollectionView _airlines;
        private Airline _selectedAirline;

        private double _ticketprice;
        private int _frequency;

        //Commandos
        public ICommand ButtonSaveCommand { get; }
        public ICommand ButtonCancelCommand { get; }

        #endregion

        //Constructor
        public NewRouteViewModel(GameViewModel gameViewModel, RoutesViewModel routesViewModel)
        {
            _gameViewModel = gameViewModel;
            _routesViewModel = routesViewModel;

            OriginAirports = CollectionViewSource.GetDefaultView(_gameViewModel.Airports.OrderBy(Airport => Airport.Name));
            DestinationAirports = CollectionViewSource.GetDefaultView(_gameViewModel.Airports.OrderBy(Airport => Airport.Name));
            DestinationAirports.Filter = DestinationAirportsFilter;
            DestinationAirportsEnabled = false;

            Airlines = CollectionViewSource.GetDefaultView(_gameViewModel.Airlines.OrderBy(Airline => Airline.Name));

            //Commands
            ButtonSaveCommand = new RelayCommand(ButtonSave);
            ButtonCancelCommand = new RelayCommand(ButtonCancel);
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
            get => _selectedDestinatonAirport;
            set
            {
                _selectedDestinatonAirport = value;
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

        public double Ticketprice
        {
            get => _ticketprice;
            set
            {
                try
                {
                    _ticketprice = Convert.ToDouble(value);
                }
                catch
                {
                    throw new Exception($"Could not convert {value} to a double");
                }

                OnPropertyChanged(nameof(Ticketprice));
                
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

        //Setup commando handling
        private void ButtonSave()
        {
            if (SaveNewRoute())
            {
                _routesViewModel.Routes.Refresh();
                _routesViewModel.CurrentDetailViewModel = null;
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

            if (Frequency < 0)
            {
                MessageBox.Show("The frequency cannot be lower then 0");
                return false;
            }

            try
            {
                //Create new route instance
                Route _route = new Route(SelectedOriginAirport, SelectedDestinationAirport, SelectedAirline, Ticketprice, Frequency);
                
                //Add route to global collection
                _gameViewModel.Routes.Add(_route);
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
