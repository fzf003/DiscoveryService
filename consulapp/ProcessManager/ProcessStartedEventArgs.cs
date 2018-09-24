using System;

namespace consulapp.ProcessManager {

    public class ProcessEventArgs : EventArgs {
        internal ProcessEventArgs (ProcessInfo processInfo) {
            this.ProcessInfo = processInfo;
        }

        /// <summary>
        /// The <see cref="ProcessInfo"/> object of the process.
        /// </summary>
        public ProcessInfo ProcessInfo { get; private set; }
    }
    public sealed class ProcessStartedEventArgs : ProcessEventArgs {
        internal ProcessStartedEventArgs (ProcessInfo processInfo) : base (processInfo) { }
    }

    public sealed class ProcessFinishedEventArgs : ProcessEventArgs {
        internal ProcessFinishedEventArgs (ProcessInfo processInfo) : base (processInfo) { }
    }


     public sealed class ProcessFailedToStartEventArgs : ProcessEventArgs
    {
        internal ProcessFailedToStartEventArgs(ProcessInfo processInfo, Exception exception)
            : base(processInfo)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// The Exception that was thrown during Process.Start().
        /// </summary>
        public Exception Exception { get; private set; }
    }


    public sealed class ProcessDataReceivedEventArgs : ProcessEventArgs
    {
        internal ProcessDataReceivedEventArgs(ProcessInfo processInfo, string data)
            : base(processInfo)
        {
            this.Data = data;
        }

        /// <summary>
        /// The data that was received.
        /// </summary>
        public string Data { get; private set; }
    }




}