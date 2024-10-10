using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AeroDynasty.ModelViews.AirlinerViewModels
{
    public class AirlinersOwnedViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private ICollectionView _airliners;
        private Airliner _selectedAirliner;

        //Commandos

        #endregion

        //Constructor
        public AirlinersOwnedViewModel()
        {
            Airliners = CollectionViewSource.GetDefaultView(GameData.Instance.Airliners);
        }

        #region Construction of variables

        //Setup public variables to local
        public ICollectionView Airliners
        {
            get => _airliners;
            private set
            {
                _airliners = value;
                OnPropertyChanged(nameof(Airliners));
            }
        }

        public Airliner SelectedAirliner
        {
            get => _selectedAirliner;
            set
            {
                _selectedAirliner = value;
                OnPropertyChanged(nameof(SelectedAirliner));
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
