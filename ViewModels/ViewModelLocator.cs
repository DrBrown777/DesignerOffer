using Designer_Offer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Designer_Offer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowView => 
            App.Host.Services.GetRequiredService<MainWindowViewModel>();
        
        public LoginViewModel LoginView =>
            (LoginViewModel)App.Host.Services.GetRequiredService<ILoginService>();

        public RegistrationViewModel RegistrationView =>
            (RegistrationViewModel)App.Host.Services.GetRequiredService<IRegistrationService>();
    }
}
