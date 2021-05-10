using Designer_Offer.ViewModels;
using System.Windows;

namespace Designer_Offer
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel ViewModel;

        public MainWindow(MainWindowViewModel _viewmodel)
        {
            InitializeComponent();
            
            ViewModel = _viewmodel;
            DataContext = ViewModel;
            //ContentRendered += MainWindow_ContentRendered;
            //Loaded += MainWindow_Loaded;
        }

        //private async void MainWindow_ContentRendered(object sender, System.EventArgs e)
        //{
        //    await ViewModel.GetAllCompaniesAsync();
        //    ViewModel.UpdatePages();
        //}

        //private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    await ViewModel.GetAllCompaniesAsync();
        //}
    }
}
