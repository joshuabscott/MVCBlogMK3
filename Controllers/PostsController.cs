using System;
using System.Collections.Generic;
using System.IO;  //MemoryStream
using System.Linq;
using System.Text.RegularExpressions; //Regex.Replace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBlogMK3.Data;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get : BlogPosts
        public async Task<IActionResult> BlogPosts(int? id)  //id == blog.Id
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);  //id == blog.Id
            if (blog == null)
            {
                return NotFound();
            }

            ViewData["BlogName"] = blog.Name;
            ViewData["BlogId"] = blog.Id;

            var blogPosts = await _context.Posts.Where(p => p.BlogId == id).ToListAsync();  //id == blog.Id
            return View(blogPosts);
        }



        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.Blogs);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5  -----------------------------------------------------------------------------
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            if(post.Image != null)  // add Line
            {
                var binary = Convert.ToBase64String(post.Image);  // add Line
                var ext = Path.GetExtension(post.FileName);       // add Line
                string imageDataURL = $"data:image/{ext};base64,{binary}";  // add Line
                ViewData["Image"] = imageDataURL;   // add Line
            }

            return View(post);
        }

        // GET: Posts/Create ----------------------------Heavily modified
        public IActionResult Create(int? id)
        {
            if (id == null)// add Line
            {
                ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Name");// add Line
            }
            else
            {
                var blog = _context.Blogs.Find(id);// add Line
                if (blog == null)// add Line
                {
                    return NotFound();// add Line
                }

                var newPost = new Post()
                {
                    BlogId = (int)id  // add Line
                };
                ViewData["BlogName"] = blog.Name;   // add Line
                ViewData["BlogId"] = id;           // add Line
                return View(newPost);
            }
            return View();
        }

        // POST: Posts/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Abstract,Content,IsPublished")] Post post, IFormFile image) //delete the Id in a create when it is scaffolded like this
        {
            if (ModelState.IsValid)
            {
                post.Created = DateTime.Now;
                post.Updated = DateTime.Now;
                post.Slug = Regex.Replace(post.Title.ToLower(), @"\s", "-");
                //Write Image
                if(image != null)
                {
                    post.FileName = image.FileName;
                    var ms = new MemoryStream();
                    image.CopyTo(ms);
                    post.Image = ms.ToArray();

                    ms.Close();
                    ms.Dispose();
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Abstract,Content,IsPublished")] Post post, IFormFile image) //add IformFile image
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
