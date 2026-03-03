using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PracticeWebApi.Services.Interfaces;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPIIController : ControllerBase
    {
        private readonly IExternalAPICall externalAPICall;
        private readonly ILogger<ExternalAPIIController> _logger;
        public ExternalAPIIController(IExternalAPICall aPICall,ILogger<ExternalAPIIController> logger)
        {
            externalAPICall = aPICall;
            _logger = logger;
        }
        [HttpGet("CatsPicture")]
        [EnableRateLimiting("sliding")]
        public async Task<string> CallCatPicsAPI()
        {
            _logger.LogInformation("call Cats Pic APi is called");
            return await externalAPICall.GetCatPhotosAsync();
        }
    }
}
