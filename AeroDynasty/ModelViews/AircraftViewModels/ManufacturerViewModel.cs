using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AeroDynasty.ModelViews.AircraftViewModels
{

    public class ManufacturerViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private Manufacturer _manufacturer;

        //Commandos
        public ICommand OpenAircraftBuyCommand { get; }

        #endregion

        //Constructor
        public ManufacturerViewModel(GameViewModel gameViewModel, Manufacturer manufacturer)
        {
            _gameViewModel = gameViewModel;
            Manufacturer = manufacturer;

            OpenAircraftBuyCommand = new RelayCommand(OpenAircraftBuy);
        }

        #region Construction of variables
        //Setup public variables to local

        /// <summary>
        /// Handle the selected manufacturer
        /// </summary>
        public Manufacturer Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));
                OnPropertyChanged(nameof(FilteredAircraftModels));
            }
        }

        /// <summary>
        /// Returns the AiircraftModels based on the selected Manufacturer
        /// </summary>
        public ObservableCollection<AircraftModel> FilteredAircraftModels
        {
            get
            {
                if(_manufacturer == null)
                {
                    return new ObservableCollection<AircraftModel>();
                }

                ObservableCollection<AircraftModel> filteredAircraftModels = new ObservableCollection<AircraftModel>(GameData.Instance.AircraftModels.Where(am => am.Manufacturer == Manufacturer));

                return filteredAircraftModels;
            }
        }

        /// <summary>
        /// Returns a Dictionary<string, ObservableCollection<AircraftModel>>
        /// </summary>
        private Dictionary<string, ObservableCollection<AircraftModel>> AircraftFamilies
        {
            get
            {
                if (Manufacturer == null)
                {
                    return new Dictionary<string, ObservableCollection<AircraftModel>>();
                }

                // Group aircraft models by Family
                var groupedAircraftModels = FilteredAircraftModels
                    .OrderBy(am => am.Family)
                    .GroupBy(am => am.Family)
                    .ToDictionary(g => g.Key, g => new ObservableCollection<AircraftModel>(g.ToList()));

                return groupedAircraftModels;
            }
        }

        /// <summary>
        /// Expose the AircraftFamilies
        /// </summary>
        public ObservableCollection<KeyValuePair<string, ObservableCollection<AircraftModel>>> AircraftFamiliesForBinding
        {
            get
            {
                return new ObservableCollection<KeyValuePair<string, ObservableCollection<AircraftModel>>>(AircraftFamilies.ToList());
            }
        }

        //Setup commando handling
        public void OpenAircraftBuy()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Functions

        //Private functions

        //Public functions

        #endregion

    }
}
