using IoT.Devices.Service.Exceptions;
using IoT.WebAPI.Infrastructure.ErrorHandling.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoT.WebAPI.Infrastructure.ErrorHandling
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                string errorJson;

                if (ex is ValidationException validationException)
                {
                    var responseErrorMessage = "Validation error occured";
                    var errorResponse = new ValidationErrorResponse(responseErrorMessage, validationException);
                    errorJson = JsonSerializer.Serialize(errorResponse);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else if (ex is ResourceNotFoundException resourceNotFoundException)
                {
                    var responseErrorMessage = "Resource not found";
                    var errorResponse = new ResourceNotFoundErrorResponse(responseErrorMessage, resourceNotFoundException);
                    errorJson = JsonSerializer.Serialize(errorResponse);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var responseErrorMessage = "Error occured";
                    var errorResponse = new ErrorResponse(responseErrorMessage, ex);
                    errorJson = JsonSerializer.Serialize(errorResponse);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                await response.WriteAsync(errorJson);
            }
        }
    }
}
