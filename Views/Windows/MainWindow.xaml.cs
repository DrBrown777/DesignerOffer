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
            //Loaded += MainWindow_Loaded;
        }

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ViewModel.UpdatePages();
        //}
    }
}
