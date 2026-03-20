using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Composition;

namespace VisiWin.Toolkit.Mef.Bridge.Services
{
    [Export]
    public class ServiceLocatorBridge
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public ServiceLocatorBridge(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : class
            => _serviceProvider.GetRequiredService<T>();

        //[Export(typeof(IMapper))]
        //public IMapper Mapper => ServiceProvider.GetRequiredService<IMapper>();
    }
}