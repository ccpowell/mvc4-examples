using System;
using System.Diagnostics;

namespace DRCOG.Domain.ViewModels
{
    /// <summary>
    /// Model for the HomeView
    /// </summary>
    public class HomeViewModel 
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeModel"/> class.
        /// </summary>
        public HomeViewModel() : this(string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public HomeViewModel(string message)
        {
            this.Message = message;
            try
            {
                this._AssemblyVersion = GetEntryAssembly();
            }
            catch { } //Do Nothing
        }

        public string Message { get; set; }

        private string _AssemblyVersion;

        public string AssemblyVersion { get { return _AssemblyVersion; } }

        public void RefreshAssemblyVersion()
        {
            var trace = new StackTrace();
            foreach (var frame in trace.GetFrames())
            {
                var assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly.GetName().Name.Equals("Trips4"))
                {
                    this._AssemblyVersion = assembly.GetName().Version.ToString();
                    break;
                }
            }
        }

        protected string GetEntryAssembly()
        {
            var trace = new StackTrace();
            foreach (var frame in trace.GetFrames())
            {
                var assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly.GetName().Name.Equals("Trips4"))
                {
                    return assembly.GetName().Version.ToString();
                }
            }
            return String.Empty;
        }
    }
}
