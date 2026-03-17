using System;
using System.Windows;
using System.Windows.Threading;
using VisiWin.ApplicationFramework;
using VisiWin.Diagnostics;

namespace HMI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SplashScreen splashScreen;

        protected override async void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
            Current.DispatcherUnhandledException += this.Current_DispatcherUnhandledException;

            this.splashScreen = new SplashScreen("/Images/Splashscreen.png");
            this.splashScreen.Show(false);

            var bootstrapper = new ApplicationBootstrapper("VisiWin.Toolkit.Demo.PlcSymbol");
            await bootstrapper.RunAsync();

            if (this.splashScreen != null)
            {
                this.splashScreen.Close(TimeSpan.FromMilliseconds(200));
                this.splashScreen = null;
            }

            base.OnStartup(e);
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ApplicationService.Log(EventSource.Application, EventCategory.Fatal, "UnhandledDispatcherException : " + e.Exception);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            ApplicationService.Log(EventSource.Application, EventCategory.Fatal, "UnhandledThreadException : " + ex);
        }
    }
}
