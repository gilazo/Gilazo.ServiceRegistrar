using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using Gilazo.ServiceRegistrar.Infrastructure;
using Gilazo.ServiceRegistrar.Presentation.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gilazo.ServiceRegistrar.Presentation.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public sealed class ServicesController : ControllerBase
    {
        private readonly IRegisterable<Service> _upsertableService;
        private readonly Application.IQueryable<MongoService, Service> _findableService;

        public ServicesController(IRegisterable<Service> upsertableService, Application.IQueryable<MongoService, Service> findableService)
        {
            _upsertableService = upsertableService;
            _findableService = findableService;
        }
        
        [HttpGet("status")]
        public async Task<ActionResult> Get()
        {
            return Ok(await _findableService.Query(_ => true));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] ServiceRegistrationRequest request)
        {
            await _upsertableService.Register(request.Service);
            return Ok();
        }
    }
}
