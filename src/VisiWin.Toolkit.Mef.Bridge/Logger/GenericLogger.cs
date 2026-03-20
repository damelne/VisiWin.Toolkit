using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.Composition;

namespace VisiWin.Toolkit.Mef.Bridge.logger
{
    [Export(typeof(ILogger<>))]
    internal class GenericLogger<T> : ILogger<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<T> _innerLogger;

        [ImportingConstructor]
        public GenericLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _innerLogger = _serviceProvider.GetRequiredService<ILogger<T>>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _innerLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _innerLogger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }

    }
}
