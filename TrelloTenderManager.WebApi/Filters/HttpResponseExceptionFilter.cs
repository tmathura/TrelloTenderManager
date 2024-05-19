using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrelloTenderManager.WebApi.Filters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

    /// <summary>
    /// Gets the order in which the filter is executed.
    /// </summary>
    public int Order => int.MaxValue - 10;

    /// <summary>
    /// Executes before the action method is invoked.
    /// </summary>
    /// <param name="context">The context for the action execution.</param>
    public void OnActionExecuting(ActionExecutingContext context) { }

    /// <summary>
    /// Executes after the action method is invoked.
    /// </summary>
    /// <param name="context">The context for the action execution.</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException httpResponseException)
        {
            context.Result = new ObjectResult(httpResponseException.Value)
            {
                StatusCode = httpResponseException.StatusCode
            };

            context.ExceptionHandled = true;

            _logger.Error($"Status code {httpResponseException.StatusCode}, {httpResponseException.Value} - {httpResponseException.Message} - {httpResponseException.StackTrace}");
        }
    }
}
