using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Sppd.TeamTuner.Middlewares
{
    internal class GlobalExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionLoggerMiddleware> _logger;

        public GlobalExceptionLoggerMiddleware(RequestDelegate next, ILogger<GlobalExceptionLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has been caught");
                throw;
            }
        }
    }
}