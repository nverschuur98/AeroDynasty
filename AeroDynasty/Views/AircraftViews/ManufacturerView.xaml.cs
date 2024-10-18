using AeroDynasty.ModelViews;
using AeroDynasty.ModelViews.AircraftViewModels;
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

namespace AeroDynasty.Views.AircraftViews
{
    /// <summary>
    /// Interaction logic for ManufacturerView.xaml
    /// </summary>
    public partial class ManufacturerView : UserControl
    {
        public ManufacturerView()
        {
            InitializeComponent();
        }

        private void OpenAircraftBuyClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                try
                {
                    // Access the DataContext, which should be the ViewModel
                    var viewModel = DataContext as ManufacturerViewModel;

                    // Execute the command with the selected airport as a parameter
                    viewModel?.OpenAircraftBuyCommand.Execute(null);
                }
                catch
                {
                    throw new Exception("Something went wrong trying to execute the command.");
                }
            }
        }
    }
}
