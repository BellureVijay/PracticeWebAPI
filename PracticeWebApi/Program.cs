using PracticeWebApi.Middleware;
using PracticeWebApi.Services.Interfaces;
using PracticeWebApi.Services.ServiceExtensions;
using PracticeWebApi.Utilities;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com");
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddPolicyHandler(HttpPolicyExtensions
.HandleTransientHttpError().WaitAndRetryAsync(2,_=>TimeSpan.FromSeconds(2)))
.AddPolicyHandler(HttpPolicyExtensions
.HandleTransientHttpError().CircuitBreakerAsync(3,TimeSpan.FromSeconds(30)));

builder.Services.AddHostedService<ApiPollingWorker>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GlobaExceptionHandler2>();
builder.Services.AddScoped<IExternalAPICall, ExternalAPiCall>();
builder.Services.AddScoped<IServiceInteraction, ServiceInteraction>();

var app = builder.Build();
app.MapGet("/job", () =>
{
    return Results.Ok("Hello world Job!");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//*custom middleware for excpetion using just use delegate
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception e)
    {
        context.Response.StatusCode = 500;
    }
});
app.UseMiddleware<GlobalExceptionHandler>();

app.UseMiddleware<GlobaExceptionHandler2>();


app.MapControllers();

app.Run();
