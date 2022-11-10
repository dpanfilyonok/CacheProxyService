using CacheProxyService.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheProxyService.Filters.ExceptionFilters;

// TODO change it or make generic
public class UnableToLocateExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<UnableToLocateExceptionFilter> _logger;

    public UnableToLocateExceptionFilter(ILogger<UnableToLocateExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not UnableToLocateException) return;
        
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