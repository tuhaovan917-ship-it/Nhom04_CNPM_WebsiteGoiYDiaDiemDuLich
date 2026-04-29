using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Areas.Admin.Controllers
{
    public class ReviewsController : BaseAdminController
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        // GET: Admin/Reviews
        public ActionResult Index()
        {
            var reviews = db.Reviews
                .Include(r => r.User)
                .Include(r => r.Place)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(reviews);
        }

        // GET: Admin/Reviews/Delete/5
        public ActionResult Delete(int id)
        {
            var review = db.Reviews
                .Include(r => r.User)
                .Include(r => r.Place)
                .FirstOrDefault(r => r.ReviewId == id);

            if (review == null)
                return HttpNotFound();

            return View(review);
        }

        // POST: Admin/Reviews/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ReviewId)
        {
            var review = db.Reviews.Find(ReviewId);
            if (review == null)
                return HttpNotFound();

            db.Reviews.Remove(review);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
