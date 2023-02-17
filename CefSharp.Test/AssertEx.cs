using System;
using System.Threading.Tasks;
using Xunit.Sdk;
using Nito.AsyncEx;
using System.Threading;

namespace CefSharp.Test
{
    internal static class AssertEx
    {
        /// <summary>
		/// Verifies that a event with the exact event args (and not a derived type) is raised
        /// This method differs from <see cref="Xunit.Assert.RaisesAsync{T}(Action{EventHandler{T}}, Action{EventHandler{T}}, Func{Task})"/>
        /// in that it waits for the event to be raised before returning (or is cancelled).
		/// </summary>
		/// <typeparam name="T">The type of the event arguments to expect</typeparam>
        /// <param name="cancelAfter">number of miliseconds to wait before the timeout</param>
		/// <param name="attach">Code to attach the event handler</param>
		/// <param name="detach">Code to detach the event handler</param>
		/// <param name="testCode">A delegate to the code to be tested</param>
		/// <returns>The event sender and arguments wrapped in an object</returns>
		/// <exception cref="RaisesException">Thrown when the expected event was not raised.</exception>
		public static async Task<Xunit.Assert.RaisedEvent<T>> RaisesAsync<T>(
            int cancelAfter,
            Action<EventHandler<T>> attach,
            Action<EventHandler<T>> detach,
            Action testCode) where T : EventArgs
        {
            var raisedEvent = await RaisesAsyncInternal(cancelAfter, attach, detach, testCode);

            if (raisedEvent == null)
                throw new RaisesException(typeof(T));

            if (raisedEvent.Arguments != null && !raisedEvent.Arguments.GetType().Equals(typeof(T)))
                throw new RaisesException(typeof(T), raisedEvent.Arguments.GetType());

            return raisedEvent;
        }

		private static async Task<Xunit.Assert.RaisedEvent<T>> RaisesAsyncInternal<T>(
            int cancelAfter,
            Action<EventHandler<T>> attach,
            Action<EventHandler<T>> detach,
            Action testCode) where T : EventArgs
        {
            GuardArgumentNotNull(nameof(attach), attach);
            GuardArgumentNotNull(nameof(detach), detach);
            GuardArgumentNotNull(nameof(testCode), testCode);

            using var cts = new CancellationTokenSource();
            var manualResetEvent = new AsyncManualResetEvent();

            cts.CancelAfter(cancelAfter);

            Xunit.Assert.RaisedEvent<T> raisedEvent = null;
            
            attach(Handler);
            testCode();
            await manualResetEvent.WaitAsync(cts.Token);
            detach(Handler);

            return raisedEvent;

            void Handler(object s, T args)
            {
                raisedEvent = new Xunit.Assert.RaisedEvent<T>(s, args);
                manualResetEvent.Set();
            }
        }

        internal static void GuardArgumentNotNull(string argName, object argValue)
        {
            if (argValue == null)
            {
                throw new ArgumentNullException(argName);
            }
        }
    }
}
