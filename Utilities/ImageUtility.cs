using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Utilities
{
    public class ImageUtility //when you write a class there is now a type of ImageUtility
    {
        public static string GetImage(Post post/*, IFormFile image*/) //inside the type of ImageUtility there is 
        {
            var binary = Convert.ToBase64String(post.Image);
            var ext = Path.GetExtension(post.FileName);
            //post.Image = $"data:image/{ext};base64,{binary}";
            return $"data:image/{ext};base64,{binary}";
        }
    }
}
