using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfLight.Models
{
    public class Message
    {
        [Key]
        public int MsgId {get;set;}
        
        [Required]
        [MinLength(5, ErrorMessage="Message must be at least 5 characters long")]
        public string Title {get;set;}
        
        [Required]
        [MinLength(10, ErrorMessage="Message must be at least 10 characters long")]
        public string Messages {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public Reg Creator {get;set;}
        public List<Comment> Comments {get;set;}
    }
}