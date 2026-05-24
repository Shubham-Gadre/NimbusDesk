using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when a domain business rule is violated.
    /// This exception is thrown to indicate invalid operations on domain entities.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with the specified message.
        /// </summary>
        /// <param name="message">The error message that describes the domain rule violation.</param>
        public DomainException(string message) : base(message)
        {
        }
    }
}
