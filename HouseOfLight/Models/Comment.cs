using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfLight.Models
{
    public class Comment
    {
        [Key]
        public int ComntId {get;set;}

        [Required]
        [MinLength(10, ErrorMessage="Comment must be at least 10 characters long")]
        public string Comments {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public Reg Creator {get;set;}
        public int MsgId {get;set;}
    }
}