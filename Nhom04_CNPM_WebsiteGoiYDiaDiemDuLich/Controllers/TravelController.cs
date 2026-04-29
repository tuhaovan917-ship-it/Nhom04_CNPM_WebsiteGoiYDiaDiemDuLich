using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class TravelController : Controller
    {
        private readonly TravelSuggestEntities db = new TravelSuggestEntities();

        // ============================
        // LẤY USER THẬT
        // ============================
        private int GetUserId()
        {
            return Session["UserId"] != null ? (int)Session["UserId"] : -1;
        }

        // ============================
        // LƯU LỊCH SỬ TÌM KIẾM
        // ============================
        private void SaveSearchHistory(string keyword, string filterJson)
        {
            int uid = GetUserId();
            if (uid == -1) return;

            db.SearchHistories.Add(new SearchHistory
            {
                UserId = uid,
                Keyword = keyword,
                FiltersJson = filterJson,
                CreatedAt = DateTime.Now
            });

            db.SaveChanges();
        }

        // ============================
        // TỌA ĐỘ THÀNH PHỐ
        // ============================
        private Dictionary<string, Tuple<double, double>> CityCoordinates =
            new Dictionary<string, Tuple<double, double>>
            {
                { "Hà Nội", Tuple.Create(21.028511, 105.804817) },
                { "TP. Hồ Chí Minh", Tuple.Create(10.823099, 106.629664) },
                { "Đà Nẵng", Tuple.Create(16.054407, 108.202167) },
                { "Hải Phòng", Tuple.Create(20.844911, 106.688084) },
                { "Cần Thơ", Tuple.Create(10.033333, 105.783333) }
            };

        private string MapTag(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;
            switch (type.Trim())
            {
                case "Phượt": return "Phiêu lưu";
                case "Cặp đôi": return "Cặp đôi";
                case "Gia đình": return "Gia đình";
                case "Nhóm bạn": return "Thiên nhiên";
                case "Cao cấp": return "Check-in";
                case "Tiết kiệm": return "Giá rẻ";
                default: return null;
            }
        }

        // ============================
        // HAVERSINE DISTANCE
        // ============================
        public static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;

            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            lat1 *= Math.PI / 180;
            lat2 *= Math.PI / 180;

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            return 2 * R * Math.Asin(Math.Sqrt(a));
        }

        // ============================
        // FORM TIÊU CHÍ
        // ============================
        public ActionResult Preferences()
        {
            //ViewBag.Cities = CityCoordinates.Keys.ToList();
            List<string> places = db.Places
                .Select(p => p.Title)
                .Distinct()
                .ToList();
            ViewBag.Cities = places;
            //ViewBag.TravelTypes = new List<string>
            //{
            //    "Phượt", "Cặp đôi", "Gia đình",
            //    "Nhóm bạn", "Cao cấp", "Tiết kiệm"
            //};
            List<string> tags = db.PlaceTypes.Select(x => x.Name).ToList();
            ViewBag.TravelTypes = tags;
            List<string> interests = db.Tags.Select(x => x.Name).ToList();
            ViewBag.Interests = interests;
            return View();
        }

        [HttpPost]
        public ActionResult Results(
            string StartLocation,
            int Budget,
            string Interests,
            string TravelType,
            int Distance
        )
        {
            // ============================
            // 1️⃣ VALIDATE
            // ============================
            var errors = new List<string>();

            var startPlace = db.Places
                .FirstOrDefault(p => p.Title == StartLocation);

            if (startPlace == null || startPlace.Latitude == null || startPlace.Longitude == null)
                errors.Add("Địa điểm xuất phát không hợp lệ.");

            if (Budget <= 0)
                errors.Add("Ngân sách phải lớn hơn 0.");

            if (Distance <= 0)
                errors.Add("Khoảng cách phải lớn hơn 0 km.");

            if (errors.Any())
            {
                ViewBag.Errors = errors;
                //return Preferences();
                ViewBag.Cities = db.Places.Select(p => p.Title).Distinct().ToList();
                ViewBag.TravelTypes = db.PlaceTypes.Select(x => x.Name).ToList();
                ViewBag.Interests = db.Tags.Select(x => x.Name).ToList();
                return View("Preferences");

            }

            // ============================
            // 2️⃣ TỌA ĐỘ XUẤT PHÁT
            // ============================
            double startLat = (double)startPlace.Latitude;
            double startLng = (double)startPlace.Longitude;

            // ============================
            // 3️⃣ QUERY ĐỊA ĐIỂM
            // ============================
            var places = db.Places
                .Include(p => p.PlaceImages)
                .Include(p => p.PlaceTags.Select(t => t.Tag))
                .Include(p => p.PlaceType)
                .Where(p => p.Latitude != null && p.Longitude != null)
                .ToList();

            // ============================
            // 4️⃣ HARD FILTER – NGÂN SÁCH
            // ============================
            decimal allow = Budget * 1.2m;
            places = places
                .Where(p => (p.AvgCost ?? decimal.MaxValue) <= allow)
                .ToList();

            // ============================
            // 5️⃣ TÍNH ĐIỂM
            // ============================
            var results = new List<RecommendResultDTO>();

            foreach (var p in places)
            {
                double dist = Haversine(
                    startLat, startLng,
                    (double)p.Latitude,
                    (double)p.Longitude
                );

                if (dist > Distance) continue;

                double score = 0;

                // Khoảng cách – 40%
                score += (1 - dist / Distance) * 40;

                // Ngân sách – 30%
                if (p.AvgCost.HasValue)
                {
                    double costRatio = (double)p.AvgCost.Value / Budget;
                    score += Math.Max(0, 1 - costRatio) * 30;
                }

                // Rating – 20%
                score += ((double)(p.AvgRating ?? 0) / 5.0) * 20;

                // Sở thích – 5%
                if (!string.IsNullOrWhiteSpace(Interests) &&
                    p.PlaceTags.Any(t => t.Tag.Name == Interests))
                {
                    score += 5;
                }

                // Loại hình – 5%
                if (!string.IsNullOrWhiteSpace(TravelType) &&
                    p.PlaceType != null &&
                    p.PlaceType.Name == TravelType)
                {
                    score += 5;
                }

                results.Add(new RecommendResultDTO
                {
                    Place = p,
                    Distance = Math.Round(dist, 1),
                    Score = Math.Round(score, 1)
                });
            }

            return View(
                "Recommendations",
                results
                    .OrderByDescending(x => x.Score)
                    .ThenBy(x => x.Distance)
                    .ToList()
            );
        }

        public ActionResult Recommendations(
            string q = "",
            int? typeId = null,
            string startCity = ""
        )
        {
            // ============================
            // 1️⃣ LẤY ĐIỂM XUẤT PHÁT TỪ DATABASE
            // ============================
            Place startPlace = null;

            if (!string.IsNullOrWhiteSpace(startCity))
            {
                startPlace = db.Places
                    .FirstOrDefault(p =>
                        p.Title == startCity &&
                        p.Latitude != null &&
                        p.Longitude != null
                    );
            }

            // Nếu không chọn hoặc không tìm thấy → lấy mặc định
            if (startPlace == null)
            {
                startPlace = db.Places
                    .FirstOrDefault(p => p.Latitude != null && p.Longitude != null);
            }

            // Trường hợp cực đoan (DB chưa có tọa độ)
            if (startPlace == null)
            {
                return View(new List<RecommendResultDTO>());
            }

            double startLat = (double)startPlace.Latitude;
            double startLon = (double)startPlace.Longitude;

            // ============================
            // 2️⃣ QUERY ĐỊA ĐIỂM
            // ============================
            var places = db.Places
                .Include(p => p.PlaceImages)
                .Include(p => p.PlaceType)
                .Include(p => p.PlaceTags.Select(t => t.Tag))
                .Where(p => p.Latitude != null && p.Longitude != null)
                .AsQueryable();

            // ============================
            // 3️⃣ TÌM KIẾM THEO TỪ KHÓA
            // ============================
            if (!string.IsNullOrWhiteSpace(q))
            {
                string kw = q.ToLower();

                places = places.Where(p =>
                    p.Title.ToLower().Contains(kw) ||
                    p.Description.ToLower().Contains(kw) ||
                    p.City.ToLower().Contains(kw) ||
                    p.PlaceTags.Any(pt => pt.Tag.Name.ToLower().Contains(kw))
                );

                SaveSearchHistory(q, null);
            }

            // ============================
            // 4️⃣ LỌC THEO LOẠI HÌNH DU LỊCH
            // ============================
            if (typeId.HasValue)
            {
                places = places.Where(p => p.PlaceTypeId == typeId.Value);
            }

            // ============================
            // 5️⃣ TÍNH KHOẢNG CÁCH + CHẤM ĐIỂM
            // ============================
            var result = places
                .ToList()
                .Select(p =>
                {
                    double distance = Haversine(
                        startLat,
                        startLon,
                        (double)p.Latitude.Value,
                        (double)p.Longitude.Value
                    );

                    return new RecommendResultDTO
                    {
                        Place = p,
                        Distance = Math.Round(distance, 2),
                        Score = (double)(p.AvgRating ?? 0)
                    };
                })
                .OrderByDescending(r => r.Score)
                .ThenBy(r => r.Distance)
                .ToList();

            // ============================
            // 6️⃣ GỬI DỮ LIỆU RA VIEW
            // ============================
            ViewBag.StartCity = startPlace.Title;

            return View(result);
        }


        // ============================
        // CHI TIẾT
        // ============================
        public ActionResult Details(int id)
        {
            var place = db.Places
                .Include(p => p.PlaceImages)
                .Include(p => p.Reviews)
                .Include(p => p.PlaceType)
                .FirstOrDefault(p => p.PlaceId == id);

            if (place == null) return HttpNotFound();

            return View(place);
        }

        // ============================
        // COMPARE
        // ============================
        public ActionResult Compare(int? addId, int? removeId)
        {
            List<int> ids = Session["CompareList"] as List<int> ?? new List<int>();

            if (addId != null && !ids.Contains(addId.Value))
                ids.Add(addId.Value);

            if (removeId != null)
                ids.Remove(removeId.Value);

            Session["CompareList"] = ids;

            var selected = db.Places
                .Include(p => p.PlaceImages)
                .Include(p => p.Reviews)
                .Include(p => p.PlaceType)
                .Where(p => ids.Contains(p.PlaceId))
                .ToList();

            ViewBag.AllPlaces = db.Places.ToList();

            return View(selected);
        }

        [HttpPost]
        public ActionResult AddReview(int PlaceId, int Rating, string Comment)
        {
            int uid = (Session["UserId"] != null) ? (int)Session["UserId"] : -1;

            if (uid == -1)
                return RedirectToAction("Login", "Account");

            if (Rating < 1 || Rating > 5)
            {
                TempData["Error"] = "Số sao không hợp lệ.";
                return RedirectToAction("Details", "Travel", new { id = PlaceId });
            }

            Review r = new Review
            {
                PlaceId = PlaceId,
                UserId = uid,
                Rating = Rating,
                Comment = Comment,
                CreatedAt = DateTime.Now
            };

            using (var db = new TravelSuggestEntities())
            {
                db.Reviews.Add(r);
                db.SaveChanges();
            }

            TempData["Success"] = "Gửi đánh giá thành công!";

            return RedirectToAction("Details", "Travel", new { id = PlaceId });
        }

    }
}
