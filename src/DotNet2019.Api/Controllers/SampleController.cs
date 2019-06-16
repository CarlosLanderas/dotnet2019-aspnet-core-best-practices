using DotNet2019.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2019.Api.Controllers
{
    [ApiController]
    [Route("api/sample")]
    [AllowAnonymous]
    public class SampleController : ControllerBase
    {
        [HttpPost, Route("")]
        public IActionResult Post(SampleRequest request)
        {
            var isValid = ModelState.IsValid;
            return Ok();
        }
    }
}
