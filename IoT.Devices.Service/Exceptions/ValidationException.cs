using IoT.Devices.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IoT.Devices.Service.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<InvalidPropertyDTO> invalidProperties)
        {
            InvalidProperties = new List<InvalidPropertyDTO>(invalidProperties);
        }

        public ValidationException(string message, IEnumerable<InvalidPropertyDTO> invalidProperties) 
            : base(message)
        {
            InvalidProperties = new List<InvalidPropertyDTO>(invalidProperties);
        }

        public ValidationException(string message, IEnumerable<InvalidPropertyDTO> invalidProperties, Exception innerException) 
            : base(message, innerException)
        {
            InvalidProperties = new List<InvalidPropertyDTO>(invalidProperties);
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            InvalidProperties = new List<InvalidPropertyDTO>();
        }

        public IEnumerable<InvalidPropertyDTO> InvalidProperties { get; }
    }
}
