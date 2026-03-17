using System;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using NLog;
using NLog.Targets;
using VisiWin.ApplicationFramework;
using VisiWin.Diagnostics;
using VisiWin.Project;

namespace HMI
{
    internal class ApplicationBootstrapper : VisiWinBootstrapper
    {
        public ApplicationBootstrapper(string projectName)
            : base(projectName)
        {
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Add the types of this assembly so that they can be found by MEF
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationBootstrapper).Assembly));

            // Add more Catalogs/Type if needed. Common Types are:
            // AssemblyCatalog
            // TypeCatalog
            // DirectoryCatalog
        }


        protected override DependencyObject CreateShell()
        {
            return new MainWindow();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (MainWindow)this.Shell;
            Application.Current.MainWindow?.Show();
        }

        protected override void OnRunProjectCompleted(object sender, RunProjectCompletedEventArgs e)
        {
            if (!e.Connected)
            {
                var fileTarget = LogManager.Configuration?.FindTargetByName("vwLogFile") as FileTarget;
                var fileName = fileTarget?.FileName?.Render(new LogEventInfo { TimeStamp = DateTime.Now });
                var message = "The application could not be started due to an error. More information can be found in the Log file. ";
                if (fileName != null)
                    message += $"({fileName})";

                ApplicationService.Log(EventSource.Project, EventCategory.Fatal, e.Error?.ToString());
                MessageBox.Show(message, "Startup failed");
                Environment.Exit(-1);
            }

            base.OnRunProjectCompleted(sender, e);
        }
    }
}