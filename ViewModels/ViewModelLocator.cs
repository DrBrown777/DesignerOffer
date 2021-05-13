using Designer_Offer.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Designer_Offer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowView => 
            App.Host.Services.GetRequiredService<MainWindowViewModel>();
        
        public LoginViewModel LoginView =>
            App.Host.Services.GetRequiredService<LoginViewModel>();

        public RegistrationViewModel RegistrationView =>
            App.Host.Services.GetRequiredService<RegistrationViewModel>();
    }
}
