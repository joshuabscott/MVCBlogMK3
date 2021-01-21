using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MVCBlogMK3.Data;
using MVCBlogMK3.Models;

namespace MVCBlogMK3.Controllers
{
    [Authorize(Roles = "Administrator, Moderator")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.BlogUser).Include(c => c.Posts);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create-----------------------------Delete here and the Create View in Comments---- moving to Details of  Posts
        public IActionResult Create()
        {
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Content")] Comment comment, string userComment)
        {
            if (ModelState.IsValid)
            {
                var email = HttpContext.User.Identity.Name;
                var blogUserId = _context.Users.FirstOrDefault(u => u.Email == email).Id;
                var blogUser = _context.Users.FirstOrDefault(u => u.Email == email);
                var post = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);

                comment.Created = DateTime.Now;
                comment.Updated = DateTime.Now;
                comment.Content = userComment;
                comment.BlogUserId = blogUserId;
                comment.BlogUser = blogUser;
                comment.Posts = post;

                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return Redirect($"~/Posts/Details/{comment.PostId}");
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewData["Id"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "DisplayName", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from over-posting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,BlogUserId,Body,Created,Updated")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (comment.Content.Contains("<!DOCTYPE"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(comment.Content);
                        XmlNode elem = doc.DocumentElement.FirstChild.NextSibling;
                        comment.Content = elem.FirstChild.InnerText.ToString();
                    }
                    comment.Posts = _context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                    comment.BlogUser = _context.Users.FirstOrDefault(u => u.Id == comment.BlogUserId);
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            ViewData["BlogUserId"] = new SelectList(_context.Set<BlogUser>(), "Id", "Id", comment.BlogUserId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Id", comment.PostId);
            //return View(comment);
            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.BlogUser)
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
