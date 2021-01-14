using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Comment
    {
        #region Keys
        //Primary Key - reviews and people telling you are wrong and they know more
        public int Id { get; set; }
        //Foreign Id for Post and Author
        public int PostId { get; set; }   //Foreign Key to Parent
        public string BlogUserId { get; set; }
        #endregion

        #region Comment Properties
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        #endregion

        #region Navigation
        //Type Post of post
        public virtual Post Posts { get; set; }
        //Type BlogUser of Author called BlogUser
        public virtual BlogUser BlogUser { get; set; }
        #endregion
    }
}