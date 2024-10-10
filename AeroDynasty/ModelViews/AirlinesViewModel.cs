using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AeroDynasty.ModelViews
{
    internal class AirlinesViewModel : BaseViewModel
    {
        private GameViewModel _gameViewModel;
        private ICollectionView _filteredAirlines;
        private string _searchText;

        public AirlinesViewModel(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            FilteredAirlines = CollectionViewSource.GetDefaultView(GameData.Instance.Airlines);
            FilteredAirlines.Filter = FilterAirlines;
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredAirlines.Refresh();  // Trigger filtering when search text changes
            }
        }

        public ICollectionView FilteredAirlines
        {
            get => _filteredAirlines;
            private set
            {
                _filteredAirlines = value;
                OnPropertyChanged(nameof(FilteredAirlines));
            }
        }

        private bool FilterAirlines(object item)
        {
            if (item is Airline airline)
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    return true;
                }

                return airline.Name.Contains(SearchText, System.StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

    }
}
