using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CavedRockCode
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger Logger;

        public CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
        {
            Logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch(Exception exception)
            {
                await HandleGlobalExceptionAsync(context, exception);
            }
        }

        private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ApplicationException)
            {
                Logger.LogWarning($"Validation error ocurred  in API........ {exception.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsJsonAsync(new { exception.Message });
            }
            else
            {
                Guid errorID = Guid.NewGuid();
                Logger.LogError(exception, $"Error ocurred in API {errorID}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsJsonAsync(new
                {
                    ErroriId = errorID,
                    Message = "Something bad happened in our API. Contact our support team with the ErrorId if the issue persist"
                });

            }
        }
    }

}