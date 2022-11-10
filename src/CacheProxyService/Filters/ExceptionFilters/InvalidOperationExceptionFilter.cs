using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheProxyService.Filters.ExceptionFilters;

public class InvalidOperationExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<InvalidOperationExceptionFilter> _logger;

    public InvalidOperationExceptionFilter(ILogger<InvalidOperationExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not InvalidOperationException) return;

        _logger.LogError(context.Exception, "An error occurred");
        
        var error = new ProblemDetails
        {
            Title = "An error occurred",
            Detail = context.Exception.Message,
            Status = 500,
            Type = "https://httpstatuses.com/500"
        };
        context.Result = new ObjectResult(error)
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
}