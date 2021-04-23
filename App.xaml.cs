using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Designer_Offer
{
    public partial class App : Application
    {
        private static Mutex _mutex = null;

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
    }
}
