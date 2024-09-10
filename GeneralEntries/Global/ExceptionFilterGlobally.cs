using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneralEntries.Global;

public class ExceptionFilterGlobally : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var error = new Error
        {
            StatusCode = 500,
            Message = context.Exception.Message
        };

        context.Result = new JsonResult(error) { StatusCode = 500 };
    }
}
