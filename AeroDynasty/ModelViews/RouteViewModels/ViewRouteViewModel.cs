using AeroDynasty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.ModelViews.RouteViewModels
{
    public class ViewRouteViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private Route _route;

        //Commandos

        #endregion

        //Constructor
        public ViewRouteViewModel(GameViewModel gameViewModel, Route route)
        {
            _gameViewModel = gameViewModel;
            Route = route;
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


        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
