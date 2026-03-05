using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeProjectWebAPI.Models.DTO;
using PracticeProjectWebAPI.services.Interfaces;

namespace PracticeProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;   
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts(int page,int pageSize)
        {
            var response = await _service.GetProduct(page,pageSize);
            return Ok(response);
        }
    }
}
