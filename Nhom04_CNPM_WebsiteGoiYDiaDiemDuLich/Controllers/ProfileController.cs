using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

//namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
//{
//    public class ProfileController : Controller
//    {
//        // GET: Profile
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class ProfileController : Controller
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        public ActionResult Index()
        {
            int uid = (Session["UserId"] != null) ? (int)Session["UserId"] : -1;
            if (uid == -1)
                return RedirectToAction("Login", "Account");

            var plans = db.TravelPlans
                .Where(p => p.UserId == uid)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            // ⭐ Lấy danh sách PlanId
            var planIds = plans.Select(p => p.PlanId).ToList();

            // ⭐ Truy vấn PlanShares bằng danh sách primitive (int)
            var tokens = db.PlanShares
                .Where(s => planIds.Contains(s.PlanId))
                .ToDictionary(s => s.PlanId, s => s.ShareToken);

            var model = new ProfileViewModel
            {
                UserInfo = db.Users.FirstOrDefault(u => u.UserId == uid),
                Plans = plans,
                SearchHistories = db.SearchHistories
                    .Where(h => h.UserId == uid)
                    .OrderByDescending(h => h.CreatedAt)
                    .ToList(),
                FavoritePlaces = db.FavoritePlaces
                    .Where(f => f.UserId == uid)
                    .Select(f => f.Place)
                    .ToList(),
                PlanTokens = tokens
            };

            return View(model);
        }
    }
}
