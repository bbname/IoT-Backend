using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.Exceptions;
using System.Collections.Generic;

namespace IoT.WebAPI.Infrastructure.ErrorHandling.Responses
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public ValidationErrorResponse(string message, ValidationException exception) 
            : base(message, exception)
        {
            InvalidProperties = exception.InvalidProperties;
        }

        public IEnumerable<InvalidPropertyDTO> InvalidProperties { get; }
    }
}
