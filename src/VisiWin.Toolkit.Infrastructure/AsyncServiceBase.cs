using VisiWin.Toolkit.Base.Extensions;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using VisiWin.ApplicationFramework;

namespace VisiWin.Toolkit.Infrastructure
{
    /// <summary>
    /// Provides a base class for services that require asynchronous lifecycle hooks
    /// for project load and unload events.
    /// </summary>
    /// <remarks>
    /// This class bridges synchronous framework callbacks to asynchronous implementations
    /// using fire-and-forget execution with centralized exception handling and optional
    /// serialization via <see cref="SemaphoreSlim"/>.
    ///
    /// Derived classes should override the provided async methods instead of the synchronous ones.
    /// </remarks>
    public abstract class AsyncServiceBase : ServiceBase
    {
        #region Private Fields

        /// <summary>
        /// Optional logger, injected via MEF if available.
        /// </summary>
        [Import(typeof(ILogger<AsyncServiceBase>), AllowDefault = true)]
        private ILogger<AsyncServiceBase> _logger { get; set; }

        #region Locks for serialization

        private readonly SemaphoreSlim _loadProjectStartedLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _loadProjectCompletedLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _unloadProjectStartedLock = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim _unloadProjectCompletedLock = new SemaphoreSlim(1, 1);

        #endregion Locks for serialization

        #endregion Private Fields

        #region Override Methods

        /// <inheritdoc />
        protected override void OnLoadProjectStarted()
        {
            base.OnLoadProjectStarted();
            _ = OnLoadProjectStartedAsync().ExecuteSafely(_logger, "OnLoadProjectStarted failed", _loadProjectStartedLock, GetType().Name);
        }

        /// <inheritdoc />
        protected override void OnLoadProjectCompleted()
        {
            base.OnLoadProjectCompleted();
            _ = OnLoadProjectCompletedAsync().ExecuteSafely(_logger, "OnLoadProjectCompleted failed", _loadProjectCompletedLock, GetType().Name);
        }

        /// <inheritdoc />
        protected override void OnUnloadProjectStarted()
        {
            base.OnUnloadProjectStarted();
            _ = OnUnloadProjectStartedAsync().ExecuteSafely(_logger, "OnUnloadProjectStarted failed", _unloadProjectStartedLock, GetType().Name);
        }

        /// <inheritdoc />
        protected override void OnUnloadProjectCompleted()
        {
            base.OnUnloadProjectCompleted();
            _ = OnUnloadProjectCompletedAsync().ExecuteSafely(_logger, "OnUnloadProjectCompleted failed", _unloadProjectCompletedLock, GetType().Name);
        }

        #endregion Override Methods

        #region Async Hooks for Derived Classes

        /// <summary>
        /// Called before the project loading process begins.
        /// </summary>
        /// <remarks>
        /// Override this method to implement asynchronous logic that should run
        /// before a project is loaded.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        protected virtual Task OnLoadProjectStartedAsync() => Task.CompletedTask;

        /// <summary>
        /// Called after the project loading process has completed.
        /// </summary>
        /// <remarks>
        /// Override this method to implement asynchronous logic that should run
        /// after a project has been loaded.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        protected virtual Task OnLoadProjectCompletedAsync() => Task.CompletedTask;

        /// <summary>
        /// Called before the project unloading process begins.
        /// </summary>
        /// <remarks>
        /// Override this method to implement asynchronous logic that should run
        /// before a project is unloaded.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        protected virtual Task OnUnloadProjectStartedAsync() => Task.CompletedTask;

        /// <summary>
        /// Called after the project unloading process has completed.
        /// </summary>
        /// <remarks>
        /// Override this method to implement asynchronous logic that should run
        /// after a project has been unloaded.
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        protected virtual Task OnUnloadProjectCompletedAsync() => Task.CompletedTask;

        #endregion Async Hooks for Derived Classes
    }
}