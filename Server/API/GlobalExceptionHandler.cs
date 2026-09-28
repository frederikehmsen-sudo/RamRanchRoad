using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public class GlobalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails()
            {
                Title = exception.Message
            });
        return default;
    }
}