using Microsoft.Extensions.Logging;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;
using VisiWin.Toolkit.Base.Extensions;

namespace VisiWin.Toolkit.Infrastructure
{
    /// <summary>
    /// Base class for adapters that want to implement asynchronous logic in the adapter hooks (e.g. OnViewAttached, OnViewDetached, etc.).
    /// </summary>
    public abstract class AsyncAdapterBase : AdapterBase
    {
        #region Private Fields

        //Optional Logger, ggf. per MEF gesetzt
        [Import(typeof(ILogger<AsyncAdapterBase>), AllowDefault = true)]
        private ILogger<AsyncAdapterBase> _logger { get; set; }

        #region Locks für Serialisierung

        private readonly SemaphoreSlim _viewAttachedLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _viewDetachedLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _viewUnloadedLock = new SemaphoreSlim(1, 1);

        #endregion Locks für Serialisierung

        #endregion Private Fields

        #region Override Methods

        public override void OnViewAttached(IView view)
        {
            base.OnViewAttached(view);
            _ = OnViewAttachedAsync(view).ExecuteSafely(_logger, "OnViewAttached failed", _viewAttachedLock, GetType().Name);
        }

        public override void OnViewDetached(IView view)
        {
            base.OnViewDetached(view);
            _ = OnViewDetachedAsync(view).ExecuteSafely(_logger, "OnViewDetached failed", _viewDetachedLock, GetType().Name);
        }

        protected override void OnViewUnloaded(object sender, ViewUnloadedEventArg e)
        {
            base.OnViewUnloaded(sender, e);
            _ = OnViewUnloadedAsync(sender, e).ExecuteSafely(_logger, "OnViewUnloaded failed", _viewUnloadedLock, GetType().Name);
        }

        #endregion Override Methods

        #region Async-Hooks 

        protected virtual Task OnViewAttachedAsync(IView view) => Task.CompletedTask;

        protected virtual Task OnViewDetachedAsync(IView view) => Task.CompletedTask;

        protected virtual Task OnViewUnloadedAsync(object sender, ViewUnloadedEventArg e) => Task.CompletedTask;

        #endregion Async-Hooks 
    }
}