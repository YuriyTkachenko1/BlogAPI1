using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace BlogAPI.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;
            var requestPath = request.Path;
            var method = request.Method;
            var user = context.User?.Identity?.Name ?? "Anonymous";
            var timestamp = DateTime.UtcNow;

            _logger.LogInformation("[AUDIT] Request: {Method} {Path} by {User} at {Timestamp}",
                method, requestPath, user, timestamp);

            await _next(context);

            stopwatch.Stop();
            var responseStatusCode = context.Response.StatusCode;

            _logger.LogInformation("[AUDIT] Response: {StatusCode} for {Method} {Path} in {ElapsedMilliseconds}ms",
                responseStatusCode, method, requestPath, stopwatch.ElapsedMilliseconds);
        }
    }

}
