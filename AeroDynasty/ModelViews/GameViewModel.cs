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
