using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class PlannerController : Controller
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        // ============================
        // LẤY USER THẬT
        // ============================
        private int GetUserId()
        {
            return Session["UserId"] != null ? (int)Session["UserId"] : -1;
        }

        // ============================
        // 1. DANH SÁCH KẾ HOẠCH CỦA USER
        // ============================
        public ActionResult Index()
        {
            int uid = GetUserId();
            if (uid == -1)
                return RedirectToAction("Login", "Account");

            var plans = db.TravelPlans
                .Where(p => p.UserId == uid)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(plans);
        }

        // ============================
        // 2. TẠO KẾ HOẠCH (GET)
        // ============================
        public ActionResult Create()
        {
            int uid = GetUserId();
            if (uid == -1)
                return RedirectToAction("Login", "Account");

            ViewBag.Places = db.Places
                .OrderBy(p => p.Title)
                .ToList();

            return View();
        }

        // ============================
        // 3. TẠO KẾ HOẠCH (POST)
        // ============================
        [HttpPost]
        public ActionResult CreatePlan(
            string Title,
            DateTime? StartDate,
            DateTime? EndDate,
            int[] DayPlaceIds,
            decimal? MoveCost,
            decimal? StayCost,
            decimal? FoodCost,
            decimal? OtherCost)
        {
            int uid = GetUserId();
            if (uid == -1)
                return RedirectToAction("Login", "Account");

            // Validate ngày
            if (StartDate == null || EndDate == null)
            {
                TempData["Error"] = "Ngày bắt đầu và kết thúc không được bỏ trống.";
                return RedirectToAction("Create");
            }

            if (EndDate < StartDate)
            {
                TempData["Error"] = "Ngày kết thúc phải sau ngày bắt đầu.";
                return RedirectToAction("Create");
            }

            int totalDays = (EndDate.Value - StartDate.Value).Days + 1;

            // Validate số lượng ngày và điểm
            if (DayPlaceIds == null || DayPlaceIds.Length != totalDays)
            {
                TempData["Error"] = "Bạn phải chọn địa điểm cho từng ngày.";
                return RedirectToAction("Create");
            }

            // Tổng chi phí
            decimal total = (MoveCost ?? 0) + (StayCost ?? 0) + (FoodCost ?? 0) + (OtherCost ?? 0);

            // Tạo kế hoạch
            TravelPlan plan = new TravelPlan
            {
                UserId = uid,
                Title = string.IsNullOrWhiteSpace(Title) ? "Kế hoạch du lịch" : Title,
                StartDate = StartDate,
                EndDate = EndDate,
                MoveCost = MoveCost,
                StayCost = StayCost,
                FoodCost = FoodCost,
                OtherCost = OtherCost,
                TotalCost = total,
                Itinerary = "Lịch trình được tạo bởi người dùng.",
                CreatedAt = DateTime.Now
            };

            db.TravelPlans.Add(plan);
            db.SaveChanges();

            // Thêm từng ngày
            for (int i = 0; i < totalDays; i++)
            {
                db.PlanItems.Add(new PlanItem
                {
                    PlanId = plan.PlanId,
                    PlaceId = DayPlaceIds[i],
                    DayNo = i + 1,
                    Notes = "Người dùng chọn"
                });
            }

            db.SaveChanges();

            return RedirectToAction("Share", new { id = plan.PlanId });
        }

        // ============================
        // 4. TRANG SHARE KẾ HOẠCH
        // ============================
        public ActionResult Share(int id)
        {
            int uid = GetUserId();
            if (uid == -1)
                return RedirectToAction("Login", "Account");

            var plan = db.TravelPlans
                .Include(p => p.PlanItems.Select(pi => pi.Place))
                .FirstOrDefault(p => p.PlanId == id && p.UserId == uid);

            if (plan == null) return HttpNotFound();

            // Tạo token chia sẻ nếu chưa có
            var share = db.PlanShares.FirstOrDefault(s => s.PlanId == id);

            if (share == null)
            {
                share = new PlanShare
                {
                    PlanId = id,
                    ShareToken = "share-" + Guid.NewGuid().ToString("N").Substring(0, 10),
                    CreatedAt = DateTime.Now
                };

                db.PlanShares.Add(share);
                db.SaveChanges();
            }

            // Tạo URL chia sẻ
            ViewBag.ShareUrl = Url.Action("Public", "Planner",
                new { token = share.ShareToken },
                Request.Url.Scheme);

            ViewBag.ShareToken = share.ShareToken;

            return View(plan);
        }

        // ============================
        // 5. XEM KẾ HOẠCH PUBLIC (KHÔNG CẦN LOGIN)
        // ============================
        public ActionResult Public(string token)
        {
            var share = db.PlanShares.FirstOrDefault(s => s.ShareToken == token);
            if (share == null) return HttpNotFound();

            var plan = db.TravelPlans
                .Include("PlanItems")
                .Include("PlanItems.Place")
                .FirstOrDefault(p => p.PlanId == share.PlanId);

            if (plan == null) return HttpNotFound();

            // Ép load Title (phòng lazy load tắt)
            foreach (var item in plan.PlanItems)
            {
                var tmp = item.Place?.Title;
            }

            return View(plan);
        }
    }
}
