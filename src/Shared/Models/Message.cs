using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Datalk.Shared.Models
{
    public class Message {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MessageId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public string ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }

        public string DatalkUserId { get; set; } // Sent by
        public string UserName { get; set; }

        public string Content { get; set;}
        
        
    }
}