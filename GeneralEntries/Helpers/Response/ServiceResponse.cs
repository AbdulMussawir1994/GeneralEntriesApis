using Microsoft.EntityFrameworkCore;

namespace GeneralEntries.Helpers.Response;

public class ServiceResponse<T>
{
    public T Value { get; set; }
    public bool Status { get; set; } = false;
    public string Message { get; set; }  = string.Empty;
}

public static class MessageClass
{
    public const string Error500 = "There was an error completing your request.Please Try Again Later";
}

public class ApiEfCoreClass
{
    public ApiEfCoreClass(bool state, string message)
    {
        this.status = state;
        this.message = message;
        this.data = new { };
    }

    public ApiEfCoreClass(bool state, string message, object data)
    {
        this.status = state;
        this.message = message;
        this.data = data;
    }

    public bool status { get; set; }
    public string message { get; set; }
    public object data { get; set; }
}

public class APIResponse
{
    public int ResponseCode { get; set; }
    public string Result { get; set; }
    public string Errormessage { get; set; }
}
