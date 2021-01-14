using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Blog
    {
        //This is intended for categorization of posts
        #region Keys
        //Catalog or category of blog grouped
        public int Id { get; set; }
        #endregion

        #region Blog Properties
        public string Name { get; set; }
        public string Url { get; set; }
        #endregion

        public virtual ICollection<Post> Posts { get; set; }
        public Blog()
        {
            Posts = new HashSet<Post>();
        }

        #region Navigation
        #endregion
    }
}
