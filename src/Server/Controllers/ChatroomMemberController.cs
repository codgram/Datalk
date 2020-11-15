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
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Datalk.Server.Services;
using System.Text.Encodings.Web;

namespace Datalk.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatroomMemberController : ControllerBase
    {
        private readonly DatalkContext _context;
        private readonly UserManager<DatalkUser> _userManager;
        private readonly IEmailSender _emailSender;

        

        public ChatroomMemberController(DatalkContext context, UserManager<DatalkUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        // GET: api/ChatroomMember
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatroomMember>>> GetChatroomMember()
        {
            return await _context.ChatroomMember.ToListAsync();
        }

        // GET: api/ChatroomMember/{id}
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

        //Get: api/ChatroomMember/{chatroomId}/invite
        [HttpGet("{chatroomId}/invite/{memberEmail}")]
        public async Task<ActionResult<Chatroom>> AddMember(string chatroomId, string memberEmail)
        {
            var user = await getUserAsync(User);
            var newUser = await _userManager.FindByEmailAsync(memberEmail);

            var chatroomMember = await _context.ChatroomMember.Where(c => c.ChatroomId == chatroomId).FirstOrDefaultAsync(c => c.DatalkUserId == "userId");
            var chatroom = await _context.Chatroom.FirstOrDefaultAsync(c => c.ChatroomId == chatroomId);

            string toEmail = memberEmail;
            string subject = "Invitation to " + chatroom.Name;

            var page = "/room/" + chatroomId + "/accept/";
            var schema = HttpContext.Request.Scheme + "://";
            var host = HttpContext.Request.Host.Value;
            var callbackUrl = schema + host + page;
            string body= $"{user.UserName} invited you to {chatroom.Name} <a href='{callbackUrl}'>Click here</a>.";
            

            if(chatroomMember == null) {
                await _emailSender.SendEmailAsync(toEmail, subject, body);
                
            }

            return chatroom;
        }

        // GET: api/ChatroomMember/{chatroomId}/members
        [HttpGet("{chatroomId}/members")]
        public async Task<ActionResult<IEnumerable<ChatroomMember>>> GetChatroomMembers(string chatroomId)
        {
            var chatroomMember = await _context.ChatroomMember.Where(c => c.ChatroomId == chatroomId).ToListAsync();

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


        public async Task<DatalkUser> getUserAsync(ClaimsPrincipal User)
        {
            var userID = _userManager.GetUserId(User);
            DatalkUser user = await _userManager.FindByIdAsync(userID);

            return user;
        }
        
    }
}
