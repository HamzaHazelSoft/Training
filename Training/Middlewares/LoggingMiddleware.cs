using Serilog.Context;
using System.Security.Claims;
using UserManagementSystem.Models;

namespace UserManagementSystem.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next,ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = context.TraceIdentifier;

            // For pre-auth endpoints (register/login) this will be "Anonymous"
            // since the user isn't authenticated yet. AuthService pushes the
            // real UserName into LogContext as soon as identity is resolved
            var userName = context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
            var method = context.Request.Method;
            var path = context.Request.Path;

            // PushProperty puts RequestId/UserName into the ambient LogContext for the lifetime of this request. Any log written by
            // ANY class down the call chain (Controller, Service, Repository...) during this request will automatically carry these values in the
            // outputTemplate's [RequestId: ...] [UserName: ...] slots - without needing to pass them manually in every single log call.

            using (LogContext.PushProperty("RequestId", requestId))
            using (LogContext.PushProperty("UserName", userName))
            {
                _logger.LogInformation("Request started. Method: {Method}, Path: {Path}",method,path);
                await _next(context);
                _logger.LogInformation("Request completed. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}",
                                       method,path, context.Response.StatusCode);
            }

        }
    }
}
