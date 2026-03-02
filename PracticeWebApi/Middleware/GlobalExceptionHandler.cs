using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PracticeWebApi.Middleware
{
    //Inject RequestDelegate and Calling InvokeAsync
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                context.Response.StatusCode = 500;
                ProblemDetails data = new ProblemDetails
                {
                    Title = "server error",
                    Detail = "middleware erorr",
                    Status=500,
                };
                var json = JsonSerializer.Serialize(data);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
