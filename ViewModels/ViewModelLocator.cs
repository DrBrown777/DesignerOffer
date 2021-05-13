using Microsoft.Extensions.DependencyInjection;

namespace Designer_Offer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => 
            App.Host.Services.GetRequiredService<MainWindowViewModel>();
    }
}
