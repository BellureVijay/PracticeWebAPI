
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PracticeWebApi.Middleware
{
    //*Using IMiddleware Interface
    public class GlobaExceptionHandler2 : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                ProblemDetails problem = new ProblemDetails
                {
                    Title = "global exception",
                    Detail = "Server Error",
                    Status = 500,
                    Type = "server issue"
                };
                string json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
