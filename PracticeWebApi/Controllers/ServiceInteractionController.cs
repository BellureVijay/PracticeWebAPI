using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.Services.Interfaces;

namespace PracticeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceInteractionController : ControllerBase
    {
        private readonly IServiceInteraction _serviceInteraction;

        public ServiceInteractionController(IServiceInteraction serviceInteraction)
        {
            _serviceInteraction = serviceInteraction;
        }
        [HttpGet]
        public async Task<string> GetOtherServiceResponse()
        {
            return await _serviceInteraction.CallOtherService();
        }
    }
}
