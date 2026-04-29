using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System.Data.Entity;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class ExtraController : Controller
    {
        // GET: Extra
        TravelSuggestEntities db = new TravelSuggestEntities();
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Extra/Blog
        public ActionResult Blog()
        {
            var blogs = db.BlogPosts
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return View(blogs);
        }

        // GET: /Extra/Blog/{slug}
        public ActionResult BlogDetails(string slug)
        {
            var blog = db.BlogPosts
                .Include(b => b.User)
                .Include(b => b.BlogPostPlaces.Select(bp => bp.Place))
                .FirstOrDefault(b => b.Slug == slug && b.IsPublished);

            if (blog == null)
                return HttpNotFound();

            blog.ViewCount++;
            db.SaveChanges();

            return View(blog);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.UtcNow;
                model.IsRead = false;

                db.Contacts.Add(model);
                db.SaveChanges();

                ViewBag.Success = "Cảm ơn bạn đã liên hệ!";
                ModelState.Clear(); // reset form sau khi gửi thành công
                return View();
            }

            // ❗ Quan trọng
            return View(model);
        }
    }
}