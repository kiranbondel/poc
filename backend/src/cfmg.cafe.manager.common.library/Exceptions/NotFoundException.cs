﻿using System.Runtime.Serialization;

namespace Cfmg.Cafe.Manager.Common.Library.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        protected NotFoundException(SerializationInfo serializationInfo, StreamingContext context)
           : base(serializationInfo, context)
        {
        }

    }
}

