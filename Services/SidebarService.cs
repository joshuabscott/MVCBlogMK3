using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCBlogMK3.Data;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Services
{
    public class SidebarService : ISidebarService
    {
        private readonly ApplicationDbContext _context;

        public SidebarService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            var blogs = _context.Blogs.Include(b => b.Posts).Where(b => b.Posts.Where(p => p.IsPublished == true).Count() > 0);
            
            return (await blogs.ToListAsync());
        }

        public async Task<IEnumerable<Post>> GetRecentPosts(int num)
        {
            var posts = _context.Posts.Where(p => p.IsPublished == true)
                .OrderByDescending(p => p.Created)
                .Take(num);

            return (await posts.ToListAsync());
        }

        public async Task<IEnumerable<Post>> GetSuggestedPosts(int num, int ignoreId)
        {
            //ignore id so we don't suggest the same post the user is on
            var posts = _context.Posts
                .Include(p => p.Blogs)
                .Where(p => p.IsPublished == true && p.Id != ignoreId)
                .OrderByDescending(p => p.Comments.Count())
                .Take(num);

            return (await posts.ToListAsync());
        }
    }
}
