﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AeroDynasty.Models.RouteModels;
using AeroDynasty.ModelViews.RouteViewModels;
using System.Windows.Media;
using AeroDynasty.Services;

namespace AeroDynasty.ModelViews
{
    public class RoutesViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private object _currentDetailViewModel;
        private ICollectionView _routes;
        private Route _selectedRoute;

        //Commandos
        public ICommand NewRouteCommand { get; }
        public ICommand ViewRouteCommand { get; }

        #endregion

        //Constructor
        public RoutesViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            CurrentDetailViewModel = null;
            Routes = CollectionViewSource.GetDefaultView(GameData.Instance.Routes);

            NewRouteCommand = new RelayCommand(NewRoute);
            ViewRouteCommand = new RelayCommand(ViewRoute);
        }

        #region Construction of variables

        //Setup public variables to local
        public object CurrentDetailViewModel
        {
            get => _currentDetailViewModel;
            set
            {
                _currentDetailViewModel = value;
                OnPropertyChanged(nameof(CurrentDetailViewModel));
            }
        }

        public ICollectionView Routes
        {
            get => _routes;
            private set
            {
                _routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        public Route SelectedRoute
        {
            get => _selectedRoute;
            set
            {
                _selectedRoute = value;
                OnPropertyChanged(nameof(SelectedRoute));

                // Execute the command if the selected route is not null
                if (_selectedRoute != null)
                {
                    ViewRouteCommand.Execute(null);
                }
            }
        }

        //Setup commando handling
        private void NewRoute()
        {
            CurrentDetailViewModel = new NewRouteViewModel(_gameViewModel, this);
        }

        private void ViewRoute()
        {
            // Here, implement your logic to open the route (e.g., set the CurrentDetailViewModel)
            // For example:
            CurrentDetailViewModel = new ViewRouteViewModel(_gameViewModel, this, SelectedRoute);
        }

        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
