using System.Threading;
using System.Windows;
using Autofac;
using Designer_Offer.Services;

namespace Designer_Offer
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            const string appName = "Designer Offer";
            
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                Current.Shutdown();
            }

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }
    }
}
