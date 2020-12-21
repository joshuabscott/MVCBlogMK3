using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    //We fight for the User! Tron quotes. This gives the User "Super" Identity powers
    //Chile inherits from the parent/base class. All of the abilities plus a few more from the User

    public class BlogUser : IdentityUser
    {
        //I will stop before I even hit the controller if I don't have these fields filled
        [Required]
        //I will check for the total length of the string so this is not fool proof
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public string DisplayName { get; set; }
        //public List<Comment> Comments { get; set; } = new List<Comment>(); //instantiation instead of in the constructor
        public List<Comment> Comments { get; set; }

        public BlogUser()
        {
            Comments = new List<Comment>();
            DisplayName = "New User";
            //this.
        }
    }
}
