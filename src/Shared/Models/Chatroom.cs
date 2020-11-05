using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Datalk.Shared.Models
{
    public class Chatroom {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ChatroomId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string DatalkUserId { get; set; } // Created By

        public string Name { get; set; }
        public string UniqueName { get; set; }
        public string Description { get; set; }
        
    }
}