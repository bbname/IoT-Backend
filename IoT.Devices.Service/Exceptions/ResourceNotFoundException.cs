using System;
using System.Runtime.Serialization;

namespace IoT.Devices.Service.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string resourceId, string resourceType)
            : base($"{resourceType} not found") 
        {
            ResourceId = resourceId;
        }

        public ResourceNotFoundException(string resourceId, string resourceType, Exception innerException)
            : base($"{resourceType} not found", innerException)
        {
            ResourceId = resourceId;
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string ResourceId { get; }
    }
}
