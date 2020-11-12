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
        public async Task<ActionResult<IEnumerable<DatalkUser>>> GetChatroom()
        {
            return await _context.DatalkUser.ToListAsync();
        }

        // GET: api/user/username
        [HttpGet("{username}")]
        public async Task<ActionResult<DatalkUser>> GetUser(string username)
        {
            var user = await _context.DatalkUser.FirstOrDefaultAsync(c => c.UserName == username);

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
