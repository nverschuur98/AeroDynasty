using AeroDynasty.ModelViews;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AeroDynasty
{
    public partial class MainWindow : Window
    {
        // Constructor van MainWindow
        public MainWindow()
        {
            InitializeComponent();

            // Instantie van de GameViewModel koppelen aan de DataContext van de View
            GameViewModel gameViewModel = new GameViewModel();
            this.DataContext = gameViewModel;
        }
    }
}