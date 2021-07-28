using System;

namespace IoT.WebAPI.Infrastructure.ErrorHandling.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, Exception exception)
        {
            Message = message;
            DetailedMessage = exception.Message;
            Type = exception.GetType().Name;
        }

        public string Message { get; }
        public string DetailedMessage { get; }
        public string Type { get; }
    }
}
