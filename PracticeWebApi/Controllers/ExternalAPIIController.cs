using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.Services.Interfaces;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPIIController : ControllerBase
    {
        private readonly IExternalAPICall externalAPICall;
        public ExternalAPIIController(IExternalAPICall aPICall)
        {
            externalAPICall = aPICall;
        }
        [HttpGet("CatsPicture")]
        public async Task<string> CallCatPicsAPI()
        {
            return await externalAPICall.GetCatPhotosAsync();
        }
    }
}
