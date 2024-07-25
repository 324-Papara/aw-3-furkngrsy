using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Para.Api.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Request'i loglama
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} {context.Request.QueryString}");

            // Response'u yakalamak için response body stream'i geçici olarak değiştirme
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                // Response'u loglama
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                _logger.LogInformation($"Response: {context.Response.StatusCode} {context.Response.ContentType} {responseBodyText}");

                // Response body'yi tekrar orijinaline döndürme
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
