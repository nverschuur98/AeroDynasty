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
using System.ComponentModel;

namespace AeroDynasty.ModelViews
{
    public class GameViewModel : BaseViewModel
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public DateTime CurrentDate;
        public UserData UserData { get; }

        public string FormattedCurrentDate
        {
            get { return GameData.Instance.FormattedCurrentDate; }
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

            CurrentDate = GameData.Instance.CurrentDate;
            UserData = GameData.Instance.UserData;

            // Subscribe to PropertyChanged event of GameData
            GameData.Instance.PropertyChanged += GameData_PropertyChanged;

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

            //Set home screen
            NavigateHome();
        }

        #region COMMANDO FUNCTIONS
        

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
        private void PlayGame()
        {
            GameData.Instance.PlayCommand.Execute(null);
        }

        private void PauseGame()
        {
            GameData.Instance.PauseCommand.Execute(null);
        }

        // Event handler for when properties change in GameData
        private void GameData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameData.CurrentDate))
            {
                OnPropertyChanged(nameof(FormattedCurrentDate)); // Notify that FormattedCurrentDate has changed
            }

            if (e.PropertyName == nameof(GameData.UserData.Airline))
            {
                OnPropertyChanged(nameof(UserData.Airline)); // Notify that FormattedCurrentDate has changed
            }
        }

        #endregion

    }

}
