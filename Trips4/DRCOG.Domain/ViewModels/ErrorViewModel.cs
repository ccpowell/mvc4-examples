using System;

namespace DRCOG.Domain.ViewModels
{
    /// <summary>
    /// Model that is used to send information to the Error view
    /// </summary>
    public class ErrorViewModel 
    {
        public ErrorViewModel(string messageBody) { MessageBody = messageBody; }
        public ErrorViewModel(string messageBody, Exception exception, string controllerName, string actionName) : this(messageBody, string.Empty, exception, controllerName, actionName) { }
        public ErrorViewModel(string messageBody, string messageFooter, Exception exception, string controllerName, string actionName)
        {
            Exception = exception;
            ControllerName = controllerName;
            ActionName = actionName;
            MessageBody = messageBody;
            MessageFooter = messageFooter;
        }

        /// <summary>
        /// Gets or sets the main message to be displayed to the user on invalid credentials for the requested page.
        /// </summary>
        /// <value>The message.</value>
        public string MessageBody { get; set; }

        /// <summary>
        /// Gets or sets the message footer; i.e. a note of who to contact, etc.
        /// </summary>
        /// <value>The message footer.</value>
        public string MessageFooter { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller.
        /// </summary>
        /// <value>The name of the controller.</value>
        public string ControllerName { get; set; }
    }
}
