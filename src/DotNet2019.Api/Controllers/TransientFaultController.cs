using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DotNet2019.Api.Controllers
{
    [ApiController]
    [Route("api/error")]
    public class TransientFaultController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Get()
        {
            var value = new Random().Next(2);

            if (value % 2 == 0)
            {
                return Ok("Pong!");
            }

            return new ContentResult { Content = ":(", StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
