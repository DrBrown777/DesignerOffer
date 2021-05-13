using Designer_Offer.ViewModels;
using System.Windows;

namespace Designer_Offer
{
    public partial class MainWindow : Window
    {
        internal MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}
