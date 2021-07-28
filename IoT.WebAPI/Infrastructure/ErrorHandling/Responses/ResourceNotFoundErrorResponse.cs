using IoT.Devices.Service.Exceptions;
using System;

namespace IoT.WebAPI.Infrastructure.ErrorHandling.Responses
{
    public class ResourceNotFoundErrorResponse : ErrorResponse
    {
        public ResourceNotFoundErrorResponse(string message, ResourceNotFoundException exception) 
            : base(message, exception)
        {
            ResourceId = exception.ResourceId;
        }

        public string ResourceId { get; }
    }
}
