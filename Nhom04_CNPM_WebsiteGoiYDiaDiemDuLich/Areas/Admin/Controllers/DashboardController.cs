using System.Linq;
using System.Web.Mvc;
using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        public ActionResult Index()
        {
            ViewBag.TotalPlaces = db.Places.Count();
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalReviews = db.Reviews.Count();
            ViewBag.TotalBlogs = db.BlogPosts.Count();

            return View();
        }
    }

}
