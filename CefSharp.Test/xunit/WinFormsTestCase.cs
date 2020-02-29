using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CefSharp.Test
{
    /// <summary>
    /// Wraps test cases for FactAttribute and TheoryAttribute so the test case runs on the WinForms thread
    /// </summary>
    [DebuggerDisplay(@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public class WinFormsTestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        IXunitTestCase testCase;

        public WinFormsTestCase(IXunitTestCase testCase)
        {
            this.testCase = testCase;
        }

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", error: true)]
        public WinFormsTestCase() { }

        public IMethodInfo Method
        {
            get { return testCase.Method; }
        }

        public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                         IMessageBus messageBus,
                                         object[] constructorArguments,
                                         ExceptionAggregator aggregator,
                                         CancellationTokenSource cancellationTokenSource)
        {
            var tcs = new TaskCompletionSource<RunSummary>();
            var thread = new Thread(() =>
            {
                try
                {
                    // Set up the SynchronizationContext so that any awaits
                    // resume on the STA thread as they would in a GUI app.
                    SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());

                    var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

                    // Arrange to pump messages to execute any async work associated with the test.
                    var applicationContext = new ApplicationContext();
                    var testCaseTask = taskFactory.StartNew(async delegate
                    {
                        try
                        {
                            return await this.testCase.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
                        }
                        finally
                        {
                            // The test case's execution is done. Terminate the message pump.
                            applicationContext.ExitThread();
                        }
                    }).Unwrap();
                    Application.Run(applicationContext);

                    // Report the result back to the Task we returned earlier.
                    CopyTaskResultFrom(tcs, testCaseTask);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public string DisplayName
        {
            get { return testCase.DisplayName; }
        }

        public string SkipReason
        {
            get { return testCase.SkipReason; }
        }

        public ISourceInformation SourceInformation
        {
            get { return testCase.SourceInformation; }
            set { testCase.SourceInformation = value; }
        }

        public ITestMethod TestMethod
        {
            get { return testCase.TestMethod; }
        }

        public object[] TestMethodArguments
        {
            get { return testCase.TestMethodArguments; }
        }

        public Dictionary<string, List<string>> Traits
        {
            get { return testCase.Traits; }
        }

        public string UniqueID
        {
            get { return testCase.UniqueID; }
        }

        public Exception InitializationException
        {
            get { return testCase.InitializationException; }
        }

        public int Timeout
        {
            get { return testCase.Timeout; }
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            testCase = info.GetValue<IXunitTestCase>("InnerTestCase");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("InnerTestCase", testCase);
        }

        private static void CopyTaskResultFrom<T>(TaskCompletionSource<T> tcs, Task<T> template)
        {
            if (tcs == null)
                throw new ArgumentNullException("tcs");
            if (template == null)
                throw new ArgumentNullException("template");
            if (!template.IsCompleted)
                throw new ArgumentException("Task must be completed first.", "template");

            if (template.IsFaulted)
                tcs.SetException(template.Exception);
            else if (template.IsCanceled)
                tcs.SetCanceled();
            else
                tcs.SetResult(template.Result);
        }
    }
}
