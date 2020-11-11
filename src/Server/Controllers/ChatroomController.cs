using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datalk.Server.Data;
using Datalk.Shared.Models;

namespace Datalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomController : ControllerBase
    {
        private readonly DatalkContext _context;

        public ChatroomController(DatalkContext context)
        {
            _context = context;
        }

        // GET: api/Chatroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chatroom>>> GetChatroom()
        {
            return await _context.Chatroom.OrderBy(c => c.CreatedOn).ToListAsync();
        }

        // GET: api/Chatroom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chatroom>> GetChatroom(string id)
        {
            var chatroom = await _context.Chatroom.FindAsync(id);

            if (chatroom == null)
            {
                return NotFound();
            }

            return chatroom;
        }

        // PUT: api/Chatroom/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChatroom(string id, Chatroom chatroom)
        {
            if (id != chatroom.ChatroomId)
            {
                return BadRequest();
            }

            _context.Entry(chatroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatroomExists(id))
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

        // POST: api/Chatroom
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [BindProperty]
        public ChatroomMember ChatroomMember { get; set; }
        [HttpPost]
        public async Task<ActionResult<Chatroom>> PostChatroom(Chatroom chatroom)
        {
            
            
            _context.Chatroom.Add(chatroom);
            await _context.SaveChangesAsync();

            chatroom.UniqueName = chatroom.Name.Replace(" ", "-");
            ChatroomMember member = new ChatroomMember() {
                ChatroomId = chatroom.ChatroomId,
                DatalkUserId = chatroom.DatalkUserId,
                Type = ChatroomMemberType.Owner,
            };

            _context.ChatroomMember.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChatroom", new { id = chatroom.ChatroomId }, chatroom);
        }

        // DELETE: api/Chatroom/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Chatroom>> DeleteChatroom(string id)
        {
            var chatroom = await _context.Chatroom.FindAsync(id);
            if (chatroom == null)
            {
                return NotFound();
            }

            _context.Chatroom.Remove(chatroom);
            await _context.SaveChangesAsync();

            return chatroom;
        }

        private bool ChatroomExists(string id)
        {
            return _context.Chatroom.Any(e => e.ChatroomId == id);
        }
    }
}
