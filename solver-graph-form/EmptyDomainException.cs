using System;
using System.Runtime.Serialization;

namespace csp_solver_graph
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