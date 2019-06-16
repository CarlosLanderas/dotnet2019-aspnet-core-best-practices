using DotNet2019.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2019.Api.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : ControllerBase
    {
        [HttpPost, Route("")]
        public IActionResult Post(SampleRequest request)
        {
            var isValid = ModelState.IsValid;
            return Ok();
        }

        public IActionResult Get()
        {
            return Ok(new
            {
                Foo = 5,
                Bar = 13
            });
        }
    }
}
