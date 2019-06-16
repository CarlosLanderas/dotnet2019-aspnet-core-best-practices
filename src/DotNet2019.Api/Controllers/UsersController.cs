using DotNet2019.Api.Infrastructure.Data;
using DotNet2019.Api.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet2019.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class SampleController : ControllerBase
    {
        private readonly DataContext _context;

        public SampleController(DataContext context)
        {
            _context = context;
        }
        [HttpPost, Route("")]
        public async Task<IActionResult> Post(UserRequest request)
        {
            var isValid = ModelState.IsValid;

            var user = new User
            {
                Name = request.Name,
                Created = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            return Ok(user);

        }

        [HttpGet, Route("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok(_context.Users.FirstOrDefault(u => u.Id == id));
        }
    }
}
