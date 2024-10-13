using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AeroDynasty.ModelViews.AircraftViewModels
{
    public class ManufacturersOverviewViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private Manufacturer _selectedManufacturer { get; set; }
        private ICollectionView _manufacturers;

        //Commandos

        #endregion

        //Constructor
        public ManufacturersOverviewViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            Manufacturers = CollectionViewSource.GetDefaultView(GameData.Instance.Manufacturers);
            Manufacturers.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        #region Construction of variables

        //Setup public variables to local
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


        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
