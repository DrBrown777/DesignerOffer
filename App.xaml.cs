using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Designer_Offer.Data;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Pages;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Designer_Offer.Services;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.Services.Repositories;

namespace Designer_Offer
{
    public partial class App : Application
    {
        public static bool IsDesignMode { get; private set; } = true;

        private static IHost _Host;

        public static IHost Host
        {
            get
            {
                if (_Host == null)
                {
                    _Host = Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
                }
                return _Host;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            var host = Host;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            _Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<ILoginService, LoginViewModel>();
            services.AddSingleton<IRegistrationService, RegistrationViewModel>();

            services.AddSingleton<WorkWindow>();
            services.AddSingleton<Login>();
            services.AddSingleton<Registration>();

            services.AddTransient<PrimeContext>();

            services.AddTransient<IRepository<Build>, DbRepository<Build>>();
            services.AddTransient<IRepository<Category>, DbRepository<Category>>();
            services.AddTransient<IRepository<Client>, DbRepository<Client>>();
            services.AddTransient<IRepository<Company>, DbRepository<Company>>();
            services.AddTransient<IRepository<CompanyPosition>, DbRepository<CompanyPosition>>();
            services.AddTransient<IRepository<Config>, DbRepository<Config>>();
            services.AddTransient<IRepository<Employee>, DbRepository<Employee>>();
            services.AddTransient<IRepository<Install>, DbRepository<Install>>();
            services.AddTransient<IRepository<InstallPart>, DbRepository<InstallPart>>();
            services.AddTransient<IRepository<Offer>, DbRepository<Offer>>();
            services.AddTransient<IRepository<Part>, DbRepository<Part>>();
            services.AddTransient<IRepository<Position>, DbRepository<Position>>();
            services.AddTransient<IRepository<Product>, DbRepository<Product>>();
            services.AddTransient<IRepository<ProductPart>, DbRepository<ProductPart>>();
            services.AddTransient<IRepository<ProductSupplier>, DbRepository<ProductSupplier>>();
            services.AddTransient<IRepository<Project>, DbRepository<Project>>();
            services.AddTransient<IRepository<Section>, DbRepository<Section>>();
            services.AddTransient<IRepository<Supplier>, DbRepository<Supplier>>();
            services.AddTransient<IRepository<Unit>, DbRepository<Unit>>();
            services.AddTransient<IRepository<UserData>, DbRepository<UserData>>();
        }

        public static string CurrentDirectory => IsDesignMode 
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;
    }
}
