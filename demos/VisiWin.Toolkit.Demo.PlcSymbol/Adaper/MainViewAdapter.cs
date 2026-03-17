using System.ComponentModel.Composition;
using VisiWin.ApplicationFramework;
using VisiWin.DataAccess;

namespace HMI
{
    [ExportAdapter(nameof(MainViewAdapter))]
    public class MainViewAdapter : AdapterBase
    {
#pragma warning disable CS0649

        [Import(typeof(IVariableService))]
        private IVariableService _variableService;

#pragma warning restore CS0649

        public override void OnViewAttached(IView view)
        {
            _variableService.SetValueAsync(PlcSymbols.Definitions.MyVariable, "VariableValueFromAdapter").Wait();
            base.OnViewAttached(view);
        }
    }
}