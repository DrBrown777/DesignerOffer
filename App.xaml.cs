using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Designer_Offer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Designer_Offer
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        private static IHost _Host;

        public static IHost Host => _Host = Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();//проверить на null

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "Designer Offer";

            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
