using CacheProxyService.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheProxyService.Filters.ExceptionFilters;

public class UnableToLocateExceptionFilter : ExceptionFilterAttribute
{
    private const int ErrorCode = 404;
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
            Status = ErrorCode,
            Type = $"https://httpstatuses.com/{ErrorCode}"
        };
        context.Result = new ObjectResult(error)
        {
            StatusCode = ErrorCode
        };
        context.ExceptionHandled = true;
    }
}