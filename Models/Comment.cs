using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Comment
    {
        //reviews and people telling you are wrong and they know more
        public int Id { get; set; }
        //Foreign Id for Post and Author
        public int PostId { get; set; }
        public string AuthorId { get; set; }

        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        //Type Post of post
        public Post Post { get; set; }
        //Type BlogUser of Author
        public BlogUser Author { get; set; }
    }
}
