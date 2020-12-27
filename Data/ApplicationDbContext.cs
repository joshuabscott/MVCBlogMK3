using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>//Add BlogUser Step 1
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }

        public DbSet<Comment> Comments{ get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
