using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace AeroDynasty.ModelViews.AircraftViewModels
{
    public class ManufacturersOverviewViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private object _currentDetailView;
        private Manufacturer _selectedManufacturer { get; set; }
        private ICollectionView _manufacturers;

        //Commandos
        public ICommand NavigateManufacturerDetailsCommand { get; }

        #endregion

        //Constructor
        public ManufacturersOverviewViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            Manufacturers = CollectionViewSource.GetDefaultView(GameData.Instance.Manufacturers);
            Manufacturers.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            NavigateManufacturerDetailsCommand = new RelayCommand(NavigateManufacturerDetails);
        }

        #region Construction of variables

        //Setup public variables to local
        public object CurrentDetailView
        {
            get => _currentDetailView;
            set
            {
                _currentDetailView = value;
                OnPropertyChanged(nameof(CurrentDetailView)); 
            }
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                OnPropertyChanged(nameof(SelectedManufacturer));
            }
        }

        public ICollectionView Manufacturers
        {
            get => _manufacturers;
            set
            {
                _manufacturers = value;
                OnPropertyChanged(nameof(Manufacturers));
            }
        }

        //Setup commando handling
        private void NavigateManufacturerDetails()
        {
            CurrentDetailView = new ManufacturerViewModel(_gameViewModel, SelectedManufacturer);
        }


        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
