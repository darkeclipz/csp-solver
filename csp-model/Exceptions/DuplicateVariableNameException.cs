using System;
using System.Runtime.Serialization;

namespace Csp.Model.Exceptions
{
    [Serializable]
    internal class DuplicateVariableNameException : Exception
    {
        public DuplicateVariableNameException()
        {
        }

        public DuplicateVariableNameException(string message) : base(message)
        {
        }

        public DuplicateVariableNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateVariableNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}