using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Utilities
{
    public static class ImageUtility //when you write a class there is now a type of ImageUtility
    {
        public static byte[] EncodeImage(IFormFile image)
        {
            // This is entry level code that turn our image into a storable format in the DB
            var ms = new MemoryStream();
            image.CopyTo(ms);   //image is going to st
            var output = ms.ToArray();

            ms.Close(); //MS = Memory Stream clean up,  garbage collecting
            ms.Dispose();   //MS = Memory Stream clean up,  garbage collecting

            return output;
        }

        public static string DecodeImage(Post post)
        {
            var binary = Convert.ToBase64String(post.Image);
            var ext = Path.GetExtension(post.FileName);
            string imageDataURL = $"data:image/{ext};base64,{binary}";
            return imageDataURL;
        }

        //public string GetImage(Post post, IFormFile image) //inside the type of ImageUtility there is 
        //{
        //    if (post != null)
        //    {
        //        if (post.Image != null)
        //        {
        //            var binary = Convert.ToBase64String(post.Image);
        //            var ext = Path.GetExtension(post.FileName);
        //            string imageDataURL = $"data:image/{ext};base64,{binary}";
        //            return imageDataURL;
        //        }
        //    }
        //    return String.Empty;
        //}
    }
}
