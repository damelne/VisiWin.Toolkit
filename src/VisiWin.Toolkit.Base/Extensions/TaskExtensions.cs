using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VisiWin.Toolkit.Base.Extensions
{
    /// <summary>
    /// Provides extension methods for safely executing <see cref="Task"/> instances.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Executes a <see cref="Task"/> in a fire-and-forget manner while capturing
        /// any exceptions to prevent them from going unobserved and potentially
        /// crashing the application (e.g. via <see cref="TaskScheduler.UnobservedTaskException"/>).
        /// </summary>
        /// <remarks>
        /// Intended for scenarios where a task is deliberately not awaited
        /// (e.g. event handlers or adapter hooks).
        ///
        /// Since this method uses <c>async void</c>, the caller cannot await the task
        /// or react to its completion. Therefore, it should only be used for controlled
        /// fire-and-forget operations.
        ///
        /// Any exceptions thrown during execution are caught and optionally forwarded
        /// to the <paramref name="onException"/> callback.
        /// </remarks>
        /// <param name="task">
        /// The task to execute.
        /// </param>
        /// <param name="onException">
        /// Optional callback invoked if an exception occurs during task execution.
        /// </param>
#pragma warning disable RECS0165
        public static async void SafeFireAndForget(this Task task, Action<Exception> onException = null)
#pragma warning restore RECS0165
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
        }

        /// <summary>
        /// Executes a <see cref="Task"/> while capturing all exceptions to prevent
        /// unobserved failures. Optionally supports serialized execution using
        /// <see cref="SemaphoreSlim"/> and logging of errors.
        /// </summary>
        /// <remarks>
        /// Suitable for controlled asynchronous execution with centralized
        /// exception handling, e.g. in fire-and-forget scenarios or framework hooks.
        ///
        /// Important:
        /// The provided <paramref name="task"/> is already started when this method
        /// is called. Therefore, the optional <paramref name="semaphore"/> only protects
        /// the awaiting of the task, not its creation or start.
        ///
        /// If serialization is required before task start, use an overload accepting
        /// <c>Func&lt;Task&gt;</c> instead.
        ///
        /// This method returns a <see cref="Task"/> and should be awaited by the caller
        /// whenever possible.
        /// </remarks>
        /// <param name="task">
        /// The already started <see cref="Task"/> to execute and monitor.
        /// </param>
        /// <param name="logger">
        /// Optional <see cref="ILogger"/> used to log exceptions.
        /// If not provided, exceptions are only captured.
        /// </param>
        /// <param name="errorMessage">
        /// Optional contextual error message that will be logged together with the exception.
        /// </param>
        /// <param name="semaphore">
        /// Optional <see cref="SemaphoreSlim"/> used to serialize concurrent executions.
        /// If provided, <see cref="SemaphoreSlim.WaitAsync"/> is called before awaiting
        /// and released in the <c>finally</c> block.
        /// </param>
        /// <param name="contextName">
        /// Optional context name (e.g. class name) used to improve log traceability.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the completion of the monitored execution.
        /// </returns>
        public static async Task ExecuteSafely(this Task task, ILogger logger = null, string errorMessage = null, SemaphoreSlim semaphore = null, string contextName = null)
        {
            if (semaphore != null)
                await semaphore.WaitAsync();

            try
            {
                await task;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger?.LogError(ex, $"{contextName}: {errorMessage}");
                else
                    System.Diagnostics.Trace.WriteLine($"{contextName}: {errorMessage}, {ex.Message}");
            }
            finally
            {
                semaphore?.Release();
            }
        }

        /// <summary>
        /// Executes a <see cref="Task"/> while capturing all exceptions to prevent
        /// unobserved failures. Optionally supports serialized execution using
        /// <see cref="SemaphoreSlim"/>.
        /// </summary>
        /// <remarks>
        /// Suitable for controlled asynchronous execution with centralized
        /// exception handling, e.g. in fire-and-forget scenarios or framework hooks.
        ///
        /// Important:
        /// The provided <paramref name="task"/> is already started when this method
        /// is called. Therefore, the optional <paramref name="semaphore"/> only protects
        /// the awaiting of the task, not its creation or start.
        ///
        /// If serialization is required before task start, use an overload accepting
        /// <c>Func&lt;Task&gt;</c> instead.
        ///
        /// This method returns a <see cref="Task"/> and should be awaited by the caller
        /// whenever possible.
        /// </remarks>
        /// <param name="task">
        /// The already started <see cref="Task"/> to execute and monitor.
        /// </param>
        /// <param name="semaphore">
        /// Optional <see cref="SemaphoreSlim"/> used to serialize concurrent executions.
        /// If provided, <see cref="SemaphoreSlim.WaitAsync"/> is called before awaiting
        /// and released in the <c>finally</c> block.
        /// </param>
        /// <param name="onException">
        /// Optional callback invoked if an exception occurs during task execution.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the completion of the monitored execution.
        /// </returns>
        public static async Task ExecuteSafely(this Task task, SemaphoreSlim semaphore = null, Action<Exception> onException = null)
        {
            if (semaphore != null)
                await semaphore.WaitAsync();

            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
            }
            finally
            {
                semaphore?.Release();
            }
        }
    }
}