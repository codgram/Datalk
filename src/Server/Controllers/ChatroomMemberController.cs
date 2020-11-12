using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datalk.Server.Data;
using Datalk.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace Datalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatroomMemberController : ControllerBase
    {
        private readonly DatalkContext _context;

        public ChatroomMemberController(DatalkContext context)
        {
            _context = context;
        }

        // GET: api/ChatroomMember
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatroomMember>>> GetChatroomMember()
        {
            return await _context.ChatroomMember.ToListAsync();
        }

        // GET: api/ChatroomMember/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChatroomMember>> GetChatroomMember(string id)
        {
            var chatroomMember = await _context.ChatroomMember.FindAsync(id);

            if (chatroomMember == null)
            {
                return NotFound();
            }

            return chatroomMember;
        }

        // PUT: api/ChatroomMember/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatroomMember(string id, ChatroomMember chatroomMember)
        {
            if (id != chatroomMember.ChatroomMemberId)
            {
                return BadRequest();
            }

            _context.Entry(chatroomMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatroomMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChatroomMember
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ChatroomMember>> PostChatroomMember(ChatroomMember chatroomMember)
        {
            _context.ChatroomMember.Add(chatroomMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatroomMember", new { id = chatroomMember.ChatroomMemberId }, chatroomMember);
        }

        // DELETE: api/ChatroomMember/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ChatroomMember>> DeleteChatroomMember(string id)
        {
            var chatroomMember = await _context.ChatroomMember.FindAsync(id);
            if (chatroomMember == null)
            {
                return NotFound();
            }

            _context.ChatroomMember.Remove(chatroomMember);
            await _context.SaveChangesAsync();

            return chatroomMember;
        }

        private bool ChatroomMemberExists(string id)
        {
            return _context.ChatroomMember.Any(e => e.ChatroomMemberId == id);
        }
    }
}
