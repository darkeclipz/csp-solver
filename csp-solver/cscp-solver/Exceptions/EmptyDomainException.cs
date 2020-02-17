using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Csp.Solver.Exceptions
{
    [Serializable]
    internal class EmptyDomainException : Exception
    {
        public EmptyDomainException()
        {
        }

        public EmptyDomainException(string message) : base(message)
        {
        }

        public EmptyDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
