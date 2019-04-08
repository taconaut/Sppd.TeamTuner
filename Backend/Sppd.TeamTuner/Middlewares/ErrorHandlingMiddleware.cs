using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Sppd.TeamTuner.Core.Exceptions;

namespace Sppd.TeamTuner.Middlewares
{
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

                case BusinessException _:
                    code = HttpStatusCode.BadRequest;
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