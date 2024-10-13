using AeroDynasty.Models.AirportModels;
using AeroDynasty.ModelViews.AirportViewModels;
using AeroDynasty.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AeroDynasty.ModelViews
{
    public class AirportsViewModel : BaseViewModel
    {

        #region Declaring of variables
        //Private variables
        private string _searchText;
        private Airport _selectedAirport;
        private GameViewModel _gameViewModel;
        private ICollectionView _filteredAirports;

        //Commandos
        public ICommand NavigateAirportDetailsCommand { get; }

        #endregion

        //Constructor
        public AirportsViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;  // Store reference to GameViewModel
            FilteredAirports = CollectionViewSource.GetDefaultView(GameData.Instance.Airports);
            FilteredAirports.Filter = FilterAirports;
            NavigateAirportDetailsCommand = new RelayCommand(NavigateAirportDetails);
        }

        #region Construction of variables

        //Setup public variables to local
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredAirports.Refresh();  // Trigger filtering when search text changes
            }
        }

        public Airport SelectedAirport
        {
            get => _selectedAirport;
            set
            {
                _selectedAirport = value;
                OnPropertyChanged(nameof(SelectedAirport));
            }
        }

        public ICollectionView FilteredAirports
        {
            get => _filteredAirports;
            private set
            {
                _filteredAirports = value;
                OnPropertyChanged(nameof(FilteredAirports));
            }
        }

        //Setup commando handling
        private void NavigateAirportDetails()
        {
            if (SelectedAirport != null)
            {
                _gameViewModel.CurrentViewModel = new AirportDetailsViewModel(_gameViewModel, SelectedAirport);
            }
            else
            {
                throw new Exception("No airport has been selected");
            }
        }

        #endregion

        private bool FilterAirports(object item)
        {
            if (item is Airport airport)
            {
                if (string.IsNullOrEmpty(SearchText))
                    return true;
                return airport.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                       airport.IATA.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                       airport.ICAO.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase) ||
                       airport.Country.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
