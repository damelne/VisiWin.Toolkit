using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.Composition;

namespace VisiWin.Toolkit.Mef.Bridge.options
{
    [Export(typeof(IOptions<>))]
    internal class GenericOptions<T> : IOptions<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;

        [ImportingConstructor]
        public GenericOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T Value
        {
            get
            {
                return _serviceProvider.GetRequiredService<IOptions<T>>().Value;
            }
        }
    }
}