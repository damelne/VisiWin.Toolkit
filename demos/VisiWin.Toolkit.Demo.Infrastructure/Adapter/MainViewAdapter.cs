using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.Toolkit.Infrastructure;

namespace HMI
{
    [ExportAdapter(nameof(MainViewAdapter))]
    public class MainViewAdapter : AsyncAdapterBase
    {
        protected override async Task OnViewAttachedAsync(IView view)
        {
            await base.OnViewAttachedAsync(view);
            MyValue = 0;
            await Task.Delay(1000); //Simulate some async work
            MyValue = 42;
        }

        private int _myValue;

        public int MyValue
        {
            get => _myValue;
            set
            {
                if (_myValue != value)
                {
                    _myValue = value;
                    OnPropertyChanged(nameof(MyValue));
                }
            }
        }
    }
}