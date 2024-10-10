using AeroDynasty.ModelViews.AirlinerViewModels;

namespace AeroDynasty.ModelViews
{
    public class AirlinersViewModel : BaseViewModel
    {
        #region Declaring of variables
        //Private variables
        private GameViewModel _gameViewModel;
        private object _ownedAirlinersViewModel;
        private object _newAircraftsViewModel;

        //Commandos

        #endregion

        //Constructor
        public AirlinersViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            OwnedAirlinersViewModel = new AirlinersOwnedViewModel();
        }

        #region Construction of variables

        //Setup public variables to local
        public object OwnedAirlinersViewModel
        {
            get => _ownedAirlinersViewModel;
            set
            {
                _ownedAirlinersViewModel = value;
                OnPropertyChanged(nameof(OwnedAirlinersViewModel));
            }
        }

        public object NewAircraftsViewModel
        {
            get => _newAircraftsViewModel;
            set
            {
                _newAircraftsViewModel = value;
                OnPropertyChanged(nameof(NewAircraftsViewModel));
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
