using System.Linq;

namespace PracticeWebApi.Middleware
{
    public class CorrelationIdMiddlware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<CorrelationIdMiddlware> _logger;
        private const string HeaderName = "X-Correlation-ID";
        public CorrelationIdMiddlware(RequestDelegate requestDelegate,ILogger<CorrelationIdMiddlware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;

        }
        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId;
            if (context.Request.Headers.ContainsKey(HeaderName))
            {
                correlationId = context.Request.Headers[HeaderName];
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }
            context.Items[HeaderName] = correlationId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = correlationId;
                return Task.CompletedTask;
            });
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = correlationId
            }))
            {
                await _requestDelegate(context);
            }
        }

    }

}
