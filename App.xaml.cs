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
using Designer_Offer.Views.UControl;
using System.Linq;

namespace Designer_Offer
{
    public partial class App : Application
    {
        public static Window ActiveWindow => Current.Windows.OfType<Window>()
            .FirstOrDefault(w => w.IsActive);

        public static Window FocusedWindow => Current.Windows.OfType<Window>()
            .FirstOrDefault(w => w.IsFocused);

        public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

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
            /*Региститрация представлений*/
            services.AddSingleton<WorkWindow>();
            services.AddSingleton<Login>();
            services.AddSingleton<Registration>();
            services.AddSingleton<ProjectManager>();
            services.AddSingleton<CompanyManager>();
            services.AddSingleton<ServiceManager>();
            
            services.AddTransient<OfferManager>();
            services.AddTransient<PartManager>();

            /*Регистрация представлений редактирования данных*/
            services.AddTransient<ClientEditorWindow>();
            services.AddTransient<BuildEditorWindow>();
            services.AddTransient<CompanyEditorWindow>();
            services.AddTransient<EmployeeEditorWindow>();
            services.AddTransient<PositionEditorWindow>();
            services.AddTransient<SectionEditorWindow>();
            services.AddTransient<UnitEditorWindow>();
            services.AddTransient<SupplierEditorWindow>();
            services.AddTransient<CategoryEditorWindow>();
            services.AddTransient<ProductEditorWindow>();
            services.AddTransient<InstallEditorWindow>();
            services.AddTransient<ManufacturerEditorWindow>();
            services.AddTransient<OfferInitWindow>();

            /*Регистрация моделей представлений окон*/
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<WorkWindowViewModel>();

            /*Регистрация моделей представлений редактирования данных*/
            services.AddTransient<ClientEditorViewModel>();
            services.AddTransient<BuildEditorViewModel>();
            services.AddTransient<CompanyEditorViewModel>();
            services.AddTransient<EmployeeEditorViewModel>();
            services.AddTransient<PositionEditorViewModel>();
            services.AddTransient<SectionEditorViewModel>();
            services.AddTransient<UnitEditorViewModel>();
            services.AddTransient<SupplierEditorViewModel>();
            services.AddTransient<CategoryEditorViewModel>();
            services.AddTransient<ProductEditorViewModel>();
            services.AddTransient<InstallEditorViewModel>();
            services.AddTransient<ManufacturerEditorViewModel>();
            services.AddTransient<OfferInitViewModel>();

            /*Регистрация моделей представлений юзер контролов*/
            services.AddSingleton<ProjectManagerViewModel>();
            services.AddSingleton<CompanyManagerViewModel>();
            services.AddSingleton<ServiceManagerViewModel>();
            
            services.AddTransient<OfferManagerViewModel>();
            services.AddTransient<PartManagerViewModel>();

            /*Регистрация интерфейсов страниц*/
            services.AddSingleton<ILoginService, LoginViewModel>();
            services.AddSingleton<IRegistrationService, RegistrationViewModel>();

            /*Сервис текущего пользователя*/
            services.AddSingleton<Employees>();
            /*Сервис текущего КП*/
            services.AddSingleton<Offers>();

            /*Регистрация сервис диалогов*/
            services.AddTransient<IUserDialog, UserDialogService>();

            /*Регистрация сервиса калькулятора*/
            services.AddTransient<ICalculator, CalculatorService>();

            /*Регистрация репозиториев сущностей базы данных*/
            services.AddSingleton<PrimeContext>();

            //services.AddTransient<IRepository<Build>, DbRepository<Build>>();
            services.AddTransient<IRepository<Builds>, BuildRepository>();
            services.AddTransient<IRepository<Categories>, DbRepository<Categories>>();
            //services.AddTransient<IRepository<Client>, DbRepository<Client>>();
            services.AddTransient<IRepository<Clients>, ClientRepository>();
            services.AddTransient<IRepository<Companies>, DbRepository<Companies>>();
            services.AddTransient<IRepository<Configs>, DbRepository<Configs>>();
            //services.AddTransient<IRepository<Employee>, DbRepository<Employee>>();
            services.AddTransient<IRepository<Employees>, EmployeeRepository>();
            services.AddTransient<IRepository<Installs>, DbRepository<Installs>>();
            //services.AddTransient<IRepository<InstallPart>, DbRepository<InstallPart>>();
            //services.AddTransient<IRepository<Manufacturer>, DbRepository<Manufacturer>>();
            services.AddTransient<IRepository<Manufacturers>, ManufacturerRepository>();
            //services.AddTransient<IRepository<Offer>, DbRepository<Offer>>();
            services.AddTransient<IRepository<Offers>, OfferRepository>();
            services.AddTransient<IRepository<Parts>, DbRepository<Parts>>();
            services.AddTransient<IRepository<Positions>, DbRepository<Positions>>();
            //services.AddTransient<IRepository<Product>, DbRepository<Product>>();
            services.AddTransient<IRepository<Products>, ProductRepository>();
            //services.AddTransient<IRepository<ProductPart>, DbRepository<ProductPart>>();
            services.AddTransient<IRepository<Projects>, DbRepository<Projects>>();
            services.AddTransient<IRepository<Sections>, DbRepository<Sections>>();
            //services.AddTransient<IRepository<Supplier>, DbRepository<Supplier>>();
            services.AddTransient<IRepository<Suppliers>, SupplierRepository>();
            services.AddTransient<IRepository<Units>, DbRepository<Units>>();
            services.AddTransient<IRepository<UsersData>, DbRepository<UsersData>>();
        }

        public static string CurrentDirectory => IsDesignMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;
    }
}
