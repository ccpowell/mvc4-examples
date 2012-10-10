using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.CustomExceptions
{
    public abstract class BusinessRuleException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        protected BusinessRuleException() { }
        /// <summary>
        /// Instantiates this class with a specific error message.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        protected BusinessRuleException(string message) : base(message) { }
        /// <summary>
        /// Instantiates this class with a specific error message and an inner <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        /// <param name="inner">The inner <see cref="Exception"/></param>
        protected BusinessRuleException(string message, Exception inner) : base(message, inner) { }
    }
}
