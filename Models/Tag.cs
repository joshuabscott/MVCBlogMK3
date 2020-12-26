using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Tag
    {
        //Hashtags for search I think? 
        public int Id { get; set; }
        public int PostId { get; set; }  //Foreign Key to Parent
        // treat this as a look up table
        public string Name { get; set; }
        public virtual Post Posts { get; set; }
    }
}
