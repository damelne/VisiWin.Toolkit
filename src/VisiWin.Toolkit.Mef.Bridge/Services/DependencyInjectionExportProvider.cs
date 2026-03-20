using System;
using System.ComponentModel.Composition;

namespace VisiWin.Toolkit.Mef.Bridge.Services
{
    public class DependencyInjectionExportProvider
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public DependencyInjectionExportProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //[Export(typeof(IMapper))]
        //public IMapper Mapper => _serviceProvider.GetRequiredService<IMapper>();
    }
}