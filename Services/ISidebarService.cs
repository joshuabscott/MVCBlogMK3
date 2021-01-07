using MVCBlogMK3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlogMK3.Services
{
    public interface ISidebarService
    {
        Task<IEnumerable<Blog>> GetBlogs();
        Task<IEnumerable<Post>> GetRecentPosts(int num);
        Task<IEnumerable<Post>> GetSuggestedPosts(int num, int ignoreId);
    }
}
