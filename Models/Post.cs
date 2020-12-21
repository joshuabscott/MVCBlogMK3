using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Post
    {
        //The actual post written about a topic or what ever
        public int Id { get; set; }
        public int BlogId { get; set; }

        public string Title { get; set; }
        public string Abstract { get; set; }

        public string Content { get; set; }
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        //In Microsoft documentation this is written public Type type
        public Blog Blog { get; set; }

        //In Microsoft docs this is public List<Type> Type
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }

        //This tells the code to do what I type:
        //var post = Post();
        //We do this so the lists exist from the start and can have records added later
        public Post()
        {
            Comments = new List<Comment>();
            Tags = new List<Tag>();
        }
    }
}
