using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCBlogMK3.Data;
using MVCBlogMK3.Models;
using MVCBlogMK3.Models.ViewModels;

namespace MVCBlogMK3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Category(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.Blogs)
                .Where(p => p.IsPublished == true && p.BlogId == id)
                .OrderByDescending(p => p.Created)
                .ToListAsync();

            ViewData["BlogId"] = id;

            return View(nameof(Index), new PostIndexVM(posts));
        }

        public async Task<IActionResult> Index(string query, int page = 0)
        {
            var posts = _context.Posts
                .Include(p => p.Blogs)
                .OrderByDescending(p => p.Created)
                .Where(p => p.IsPublished == true);


            if (query != null)
            {
                posts = posts.Where(p => p.Content.ToLower().Contains(query.ToLower()) || p.Title.ToLower().Contains(query.ToLower()));
                ViewData["Query"] = query;
            }

            int allPostCount = posts.Count();
            posts = posts.Skip(page * 4).Take(4);

            PostIndexVM model = new PostIndexVM(await posts.ToListAsync(), allPostCount, page, 4);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
