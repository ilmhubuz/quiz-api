using Microsoft.Extensions.Logging;
using Quiz.Shared.Exceptions;

namespace Quiz.CSharp.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} | Time: {DateTime.UtcNow}");
            await next(context); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Unhandled exception: {ex.Message}" );
            
            var (statusCode, errorMessage) = ex switch
            {
                CustomNotFoundException notFound => (StatusCodes.Status404NotFound, notFound.Message),
                _ => (StatusCodes.Status500InternalServerError, ex.Message)
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(errorMessage);
        }
    }
}