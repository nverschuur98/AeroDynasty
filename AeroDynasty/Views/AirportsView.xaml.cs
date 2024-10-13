using AeroDynasty.Models.AirportModels;
using AeroDynasty.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeroDynasty.Views
{
    /// <summary>
    /// Interaction logic for AirportsView.xaml
    /// </summary>
    public partial class AirportsView : UserControl
    {
        public AirportsView()
        {
            InitializeComponent();
        }

        private void NavigateAirportDetailsClick(Object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Access the DataContext, which should be the ViewModel
                var viewModel = DataContext as AirportsViewModel;

                // Execute the command with the selected airport as a parameter
                viewModel?.NavigateAirportDetailsCommand.Execute(null);
            }
            catch
            {
                throw new Exception("Something went wrong trying to execute the command.");
            }
            
        }
    }
}
