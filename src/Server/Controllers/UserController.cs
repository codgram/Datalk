using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datalk.Server.Data;
using Datalk.Shared.Models;
using Datalk.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Datalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DatalkContext _context;

        public UserController(DatalkContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Areas.Identity.Data.DatalkUser>>> GetUser()
        {
            return await _context.DatalkUser.ToListAsync();
        }

        // GET: api/user/username
        [HttpGet("{username}")]
        public async Task<ActionResult<Areas.Identity.Data.DatalkUser>> GetUser(string username)
        {
            var user = await _context.DatalkUser.FirstOrDefaultAsync(c => c.UserName == username);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/user/email={email}
        [HttpGet("email={email}")]
        public async Task<ActionResult<Areas.Identity.Data.DatalkUser>> GetUserByEmail(string email)
        {
            var user = await _context.DatalkUser.FirstOrDefaultAsync(c => c.Email == email);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }


        // GET: api/user/email={email}
        [HttpGet("id={Id}")]
        public async Task<ActionResult<Areas.Identity.Data.DatalkUser>> GetUserById(string Id)
        {
            var user = await _context.DatalkUser.FirstOrDefaultAsync(c => c.Id == Id);

            if (user== null)
            {
                return NotFound();
            }

            return user;
        }
        

        private bool UserExists(string id)
        {
            return _context.DatalkUser.Any(e => e.Id == id);
        }
    }
}
