using AeroDynasty.Models.Core;
using AeroDynasty.Models.RouteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AeroDynasty.ModelViews.RouteViewModels
{
    public class ViewRouteViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private RoutesViewModel _routesViewModel;
        private Route _route;

        //Commandos
        public ICommand ButtonEditCommand { get; }

        #endregion

        //Constructor
        public ViewRouteViewModel(GameViewModel gameViewModel, RoutesViewModel routesViewModel, Route route)
        {
            _gameViewModel = gameViewModel;
            _routesViewModel = routesViewModel;
            Route = route;

            ButtonEditCommand = new RelayCommand(ButtonEdit);
        }

        #region Construction of variables

        //Setup public variables to local
        public Route Route
        {
            get => _route;
            set
            {
                _route = value;
                OnPropertyChanged(nameof(Route));
            }
        }

        //Setup commando handling
        private void ButtonEdit()
        {
            _routesViewModel.CurrentDetailViewModel = new NewRouteViewModel(_gameViewModel, _routesViewModel, Route);
        }

        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
