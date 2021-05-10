using Autofac;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Pages;

namespace Designer_Offer.Services
{
    internal class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Login>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<Registration>().AsSelf();
            builder.RegisterType<RegistrationViewModel>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
