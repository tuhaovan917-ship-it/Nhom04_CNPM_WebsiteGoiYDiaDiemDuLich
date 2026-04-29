using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System.Linq;
using System.Web.Mvc;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToList();

            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();

            ViewBag.Roles = new SelectList(
                new[] { "User", "Admin" },
                user.Role
            );

            return View(user);
        }

        // POST: Admin/Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            var user = db.Users.Find(model.UserId);
            if (user == null) return HttpNotFound();

            user.FullName = model.FullName;
            user.Role = model.Role;
            user.IsEmailConfirmed = model.IsEmailConfirmed;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
