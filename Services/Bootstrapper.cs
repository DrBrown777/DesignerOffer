using Autofac;
using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Pages;

namespace Designer_Offer.Services
{
    internal class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<PrimeContext>().AsSelf();

            builder.RegisterType<Login>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<Registration>().AsSelf();
            builder.RegisterType<RegistrationViewModel>().AsSelf();
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
