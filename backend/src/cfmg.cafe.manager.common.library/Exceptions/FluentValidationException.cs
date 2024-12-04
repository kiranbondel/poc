using System.Runtime.Serialization;

namespace Cfmg.Cafe.Manager.Common.Library.Exceptions
{
    [Serializable]
    public class FluentValidationException : Exception
    {
        public FluentValidationException()
        {
        }

        public FluentValidationException(string message)
            : base(message)
        {
        }

        public FluentValidationException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        protected FluentValidationException(SerializationInfo serializationInfo, StreamingContext context)
           : base(serializationInfo, context)
        {
        }
    }
}

