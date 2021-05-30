using Designer_Offer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Designer_Offer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowView => 
            App.Host.Services.GetRequiredService<MainWindowViewModel>();

        public WorkWindowViewModel WorkWindowView =>
            App.Host.Services.GetRequiredService<WorkWindowViewModel>();

        public ProjectManagerViewModel ProjectManagerView =>
           App.Host.Services.GetRequiredService<ProjectManagerViewModel>();

        public LoginViewModel LoginView =>
            (LoginViewModel)App.Host.Services.GetRequiredService<ILoginService>();

        public RegistrationViewModel RegistrationView =>
            (RegistrationViewModel)App.Host.Services.GetRequiredService<IRegistrationService>();

        public ClientEditorViewModel ClientEditorView =>
            App.Host.Services.GetRequiredService<ClientEditorViewModel>();
    }
}
