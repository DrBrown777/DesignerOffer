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

            /*Регистрация моделей представлений юзер контролов*/
            services.AddSingleton<ProjectManagerViewModel>();
            services.AddSingleton<CompanyManagerViewModel>();
            services.AddSingleton<ServiceManagerViewModel>();

            /*Регистрация интерфейсов страниц*/
            services.AddSingleton<ILoginService, LoginViewModel>();
            services.AddSingleton<IRegistrationService, RegistrationViewModel>();

            /*Сервис текущего пользователя*/
            services.AddSingleton<IEntity, Employee>();

            /*Регистрация сервис диалогов*/
            services.AddTransient<IUserDialog, UserDialogService>();

            /*Регистрация репозиториев сущностей базы данных*/
            services.AddSingleton<PrimeContext>();

            //services.AddTransient<IRepository<Build>, DbRepository<Build>>();
            services.AddTransient<IRepository<Build>, BuildRepository>();
            services.AddTransient<IRepository<Category>, DbRepository<Category>>();
            //services.AddTransient<IRepository<Client>, DbRepository<Client>>();
            services.AddTransient<IRepository<Client>, ClientRepository>();
            services.AddTransient<IRepository<Company>, DbRepository<Company>>();
            services.AddTransient<IRepository<Config>, DbRepository<Config>>();
            //services.AddTransient<IRepository<Employee>, DbRepository<Employee>>();
            services.AddTransient<IRepository<Employee>, EmployeeRepository>();
            services.AddTransient<IRepository<Install>, DbRepository<Install>>();
            services.AddTransient<IRepository<InstallPart>, DbRepository<InstallPart>>();
            //services.AddTransient<IRepository<Manufacturer>, DbRepository<Manufacturer>>();
            services.AddTransient<IRepository<Manufacturer>, ManufacturerRepository>();
            services.AddTransient<IRepository<Offer>, DbRepository<Offer>>();
            services.AddTransient<IRepository<Part>, DbRepository<Part>>();
            services.AddTransient<IRepository<Position>, DbRepository<Position>>();
            services.AddTransient<IRepository<Product>, DbRepository<Product>>();
            services.AddTransient<IRepository<ProductPart>, DbRepository<ProductPart>>();
            services.AddTransient<IRepository<Project>, DbRepository<Project>>();
            services.AddTransient<IRepository<Section>, DbRepository<Section>>();
            //services.AddTransient<IRepository<Supplier>, DbRepository<Supplier>>();
            services.AddTransient<IRepository<Supplier>, SupplierRepository>();
            services.AddTransient<IRepository<Unit>, DbRepository<Unit>>();
            services.AddTransient<IRepository<UserData>, DbRepository<UserData>>();
        }

        public static string CurrentDirectory => IsDesignMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;
    }
}
