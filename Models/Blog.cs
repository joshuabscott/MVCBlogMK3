using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Blog
    {
        //Catalog or catagory of blogs grouped
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        //public List<Post> Posts { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public Blog()
        {
            Posts = new HashSet<Post>();
        }
    }
}
