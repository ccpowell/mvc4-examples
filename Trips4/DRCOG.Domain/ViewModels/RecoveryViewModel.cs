using System;
using System.Reflection;
using System.Diagnostics;
using DRCOG.Domain.Models;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Security;

namespace DRCOG.Domain.ViewModels
{
    /// <summary>
    /// Contains all necessary data to render the Login view.
    /// </summary>
    public class RecoveryViewModel 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        public RecoveryViewModel() : this(string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RecoveryViewModel(string message)
        {
            this.Message = message;
            AssemblyVersion = GetEntryAssembly();
        }

        /// <summary>
        /// Gets or sets the message to be displayed to the user on invalid login credentials.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public PasswordRecoveryModel PasswordRecoveryModel { get; set; }

        /// <summary>
        /// Gets or sets the Url the user will be directed to after authentication.
        /// </summary>
        public string ReturnUrl { get; set; }

        public readonly string AssemblyVersion;

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
