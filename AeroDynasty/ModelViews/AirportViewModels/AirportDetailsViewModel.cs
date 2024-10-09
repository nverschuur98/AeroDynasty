using AeroDynasty.Models.AirportModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AeroDynasty.ModelViews.AirportViewModels
{
    public class AirportDetailsViewModel : BaseViewModel
    {
        #region Declaring of variables

        //Private variables
        private Airport _selectedAirport;
        private GameViewModel _gameViewModel;

        //Commandos
        public ICommand NavigateAirportsCommand { get; }

        #endregion

        //Constructor
        public AirportDetailsViewModel(GameViewModel gameViewModel, Airport selectedAirport)
        {
            _gameViewModel = gameViewModel;  // Store reference to GameViewModel
            SelectedAirport = selectedAirport;
            NavigateAirportsCommand = new RelayCommand(NavigateAirports);
        }

        #region Construction of variables

        //Setup public variables to local

        public Airport SelectedAirport
        {
            get => _selectedAirport;
            set
            {
                _selectedAirport = value;
                OnPropertyChanged(nameof(SelectedAirport));
            }
        }

        //Setup commando handling
        private void NavigateAirports()
        {
            _gameViewModel.CurrentViewModel = new AirportsViewModel(_gameViewModel);
        }

        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion
    }
}
