using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Sppd.TeamTuner.Core.Exceptions;

using ArgumentException = Sppd.TeamTuner.Core.Exceptions.ArgumentException;

namespace Sppd.TeamTuner.Middlewares
{
    /// <summary>
    ///     Handles all uncaught exceptions and returns an error response.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code;
            string message;

            switch (ex)
            {
                case EntityNotFoundException e:
                    code = HttpStatusCode.NotFound;
                    message = $"Entity of type '{e.EntityType}' with Id={e.Identifier} could not be found";
                    break;

                case SecurityException _:
                    code = HttpStatusCode.Forbidden;
                    message = $"Not authorized: {ex.Message}";
                    break;

                case ArgumentException e:
                    code = HttpStatusCode.BadRequest;
                    message = $"Invalid argument for parameter {e.ParameterName}: {ex.Message}";
                    break;

                case ConcurrentUpdateException _:
                    code = HttpStatusCode.Conflict;
                    message = "The entity cannot be saved because it has been modified since you last retrieved it";
                    break;

                case BusinessException _:
                    code = HttpStatusCode.InternalServerError;
                    message = $"A business error has occured: {ex.Message}";
                    break;

                default:
                    code = HttpStatusCode.InternalServerError;
                    message = $"A unexpected error has occured: {ex.Message}";
                    break;
            }

            var result = JsonConvert.SerializeObject(new {error = message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            return context.Response.WriteAsync(result);
        }
    }
}