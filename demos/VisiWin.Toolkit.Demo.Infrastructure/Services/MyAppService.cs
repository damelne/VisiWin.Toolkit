using System.ComponentModel.Composition;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.Toolkit.Infrastructure;

namespace HMI
{
    [ExportService(typeof(IMyAppService))]
    [Export(typeof(IMyAppService))]
    public class MyAppService : AsyncServiceBase, IMyAppService
    {
        private int _backgroundTaskCounter;

        public int BackgroundTaskCounter
        {
            get => _backgroundTaskCounter;

            set
            {
                if (value != _backgroundTaskCounter)
                {
                    _backgroundTaskCounter = value;
                    OnPropertyChanged(nameof(BackgroundTaskCounter));
                }
            }
        }

        protected override async Task OnLoadProjectCompletedAsync()
        {
            await base.OnLoadProjectCompletedAsync();

            await Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    BackgroundTaskCounter += 1;
                }
            });
        }
    }
}