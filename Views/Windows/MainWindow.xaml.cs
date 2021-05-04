using Designer_Offer.ViewModels;
using System.Windows;

namespace Designer_Offer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }
    }
}
