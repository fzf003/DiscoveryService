
using System;

namespace consulapp.ProcessManager
{
    public class ProcessInfo
    {
        /// <summary>
        /// Create a new <see cref="ProcessInfo"/> instance.
        /// </summary>
        /// <param name="fileName">The filename of the process.</param>
        /// <param name="arguments">The command-line arguments for the process.</param>
        public ProcessInfo(string fileName, string arguments)
        {
            this.Key = Guid.NewGuid();
            this.FileName = fileName;
            this.Arguments = arguments;
        }

        /// <summary>
        /// A unique key for this instance.
        /// </summary>
        public Guid Key { get; private set; }
        /// <summary>
        /// The filename of the process.
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// The command-line arguments for the process.
        /// </summary>
        public string Arguments { get; private set; }
    }

    /// <summary>
    /// Describes a process with user data that can be queued.
    /// </summary>
    /// <typeparam name="T">The type of the user data.</typeparam>
    public class ProcessInfo<T> : ProcessInfo
    {
        /// <summary>
        /// Create a new <see cref="ProcessInfo&lt;T&gt;"/> instance.
        /// </summary>
        /// <param name="fileName">The filename of the process.</param>
        /// <param name="arguments">The command-line arguments for the process.</param>
        /// <param name="data">The user data.</param>
        public ProcessInfo(string fileName, string arguments, T data)
            : base(fileName, arguments)
        {
            this.Data = data;
        }

        /// <summary>
        /// User data.
        /// </summary>
        public T Data { get; private set; }
    }
}