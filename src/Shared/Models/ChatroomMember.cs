using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datalk.Shared.Models
{

    public enum ChatroomMemberType { 
        Owner,
        Admin,
        Member
    }
    public class ChatroomMember {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ChatroomMemberId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        
        public string ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }
        public string DatalkUserId { get; set; } //User Id
        public ChatroomMemberType Type { get; set; }
        
    }
}