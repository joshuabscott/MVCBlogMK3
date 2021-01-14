using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Models
{
    public class Post       //The actual post written about a topic or what ever
    {
        //Keys
        #region Keys 
        public int Id { get; set; }
        public int BlogId { get; set; } //Foreign Key to Parent
        #endregion

        //Describe the things that a blog post have
        #region Post Properties
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }

        public string Slug { get; set; } //Use the Title to Identity instead of the Id, will cover - ?? //routing engine and SEO??

        [Display(Name = "File Name")]
        public string FileName { get; set; } //-> You will need to do Add-Migration & Update-Database. 10/23 Image UPloader
        public byte[] Image { get; set; }
        public string ImageDataUrl { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsPublished { get; set; }
        #endregion

        //In Microsoft documentation this is written public Type type
        #region Navigation
        public virtual Blog Blogs { get; set; }
        //In Microsoft docs this is public List<Type> Type
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        #endregion

        //This tells the code to do what I type:
        //var post = Post();
        //We do this so the lists exist from the start and can have records added later
        public Post()
        {
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }
    }
}
