using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AeroDynasty.ModelViews
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;

        //Commandos
        public ICommand BtnSaveGame { get; }
        public ICommand BtnLoadGame { get; }
        public ICommand BtnExitGame { get; }
        public ICommand BtnToggleFullScreen { get; }

        #endregion

        //Constructor
        public SettingsViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;

            BtnSaveGame = new RelayCommand(SaveGame);
            BtnLoadGame = new RelayCommand(LoadGame);
            BtnExitGame = new RelayCommand(ExitGame);
            BtnToggleFullScreen = new RelayCommand(ToggleFullScreen);
        }

        #region Construction of variables

        //Setup public variables to local


        //Setup commando handling
        private void SaveGame()
        {
            throw new NotImplementedException();
        }

        private void LoadGame()
        {
            throw new NotImplementedException();
        }

        private void ToggleFullScreen()
        {
            // Access the main window
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow.WindowState == WindowState.Maximized && mainWindow.WindowStyle == WindowStyle.None)
            {
                // Currently in fullscreen, so exit fullscreen
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.ResizeMode = ResizeMode.CanResize;
                mainWindow.Width = 1000; // Full screen width
                mainWindow.Height = 600; // Full screen height
            }
            else
            {
                // Enter True Fullscreen Mode (Taskbar hidden)
                mainWindow.WindowStyle = WindowStyle.None; // Removes window border, title bar
                mainWindow.ResizeMode = ResizeMode.NoResize; // Disable resizing
                mainWindow.WindowState = WindowState.Normal; // Set state to Normal first
                mainWindow.Top = 0; // Move the window to the top of the screen
                mainWindow.Left = 0; // Move the window to the left of the screen
                mainWindow.Width = SystemParameters.PrimaryScreenWidth; // Full screen width
                mainWindow.Height = SystemParameters.PrimaryScreenHeight; // Full screen height
                mainWindow.WindowState = WindowState.Maximized; // Maximized after setting size
            }
        }

        private void ExitGame()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
