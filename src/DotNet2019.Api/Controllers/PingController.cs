using DotNet2019.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DotNet2019.Api.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : ControllerBase
    {
        private readonly ISomeService someService;

        public PingController(ISomeService someService)
        {
            this.someService = someService ?? throw new ArgumentNullException(nameof(someService));
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            return Ok(await someService.Ping());
        }
    }
}
