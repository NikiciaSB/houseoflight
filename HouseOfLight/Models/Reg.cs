using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfLight.Models
{
    public class Reg
    {
        [Key]
        public int UserId {get;set;}
        
        [Required]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters or longer!")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and Charas are not allowed.")]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters or longer!")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and Charas are not allowed.")]
        public string LastName {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Username must be at least 2 characters or longer!")]
        public string Username {get;set;}


        [EmailAddress]
        [Required]
        public string Email {get;set;}

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Dude....Password must be 8 characters or longer!")]
        [RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{2,})$", ErrorMessage = "Must have at least one number or character in your Password")]
        
        public string Password {get;set;}
    // Will not be mapped to your users table!
        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public string ProfilePic {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Message> Messages{get;set;}
        public List<Comment> CreatedComments {get;set;}
    }
}