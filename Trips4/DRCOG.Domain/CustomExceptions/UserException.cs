using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.CustomExceptions
{
    public class UserException : BusinessRuleException
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected UserException() : base() { }
        /// <summary>
        /// Instantiates this class with a specific error message.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public UserException(string message) : base(message) { }
        /// <summary>
        /// Instantiates this class with a specific error message and an inner <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        /// <param name="inner">The inner <see cref="Exception"/></param>
        public UserException(string message, Exception inner) : base(message, inner) { }
    }
}
