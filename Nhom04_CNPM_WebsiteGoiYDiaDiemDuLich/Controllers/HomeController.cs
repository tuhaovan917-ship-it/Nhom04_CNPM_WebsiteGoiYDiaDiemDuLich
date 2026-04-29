using System;
using System.Linq;
using System.Web.Mvc;
using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models.ViewModels;
using System.Data.Entity;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class HomeController : Controller
    {
        private TravelSuggestEntities db = new TravelSuggestEntities();

        public ActionResult Index()
        {
            var fromDate = DateTime.UtcNow.AddDays(-7);

            // ===== TRENDING =====
            // Trending = Favorite + Review trong 7 ngày gần nhất
            var trendingQuery = db.Places
                .Include(p => p.PlaceImages)
                .Select(p => new
                {
                    Place = p,
                    Score =
                        p.FavoritePlaces.Count(f => f.CreatedAt >= fromDate)
                      + p.Reviews.Count(r => r.CreatedAt >= fromDate)
                });

            var hasTrendingData = trendingQuery.Any(x => x.Score > 0);

            var trendingPlaces = hasTrendingData
                ? trendingQuery
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.Place)
                    .Take(3)
                    .ToList()
                : db.Places
                    .Include(p => p.PlaceImages)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3)
                    .ToList();

            // ===== POPULAR =====
            // Popular = tổng số lượt Favorite
            var popularPlaces = db.Places
                .Include(p => p.PlaceImages)
                .OrderByDescending(p => p.FavoritePlaces.Count())
                .Take(3)
                .ToList();

            // ===== TOP RATED =====
            var topRatedPlaces = db.Places
                .Include(p => p.PlaceImages)
                .OrderByDescending(p => p.AvgRating)
                .Take(3)
                .ToList();

            var model = new HomeViewModel
            {
                TrendingPlaces = trendingPlaces,
                PopularPlaces = popularPlaces,
                TopRatedPlaces = topRatedPlaces
            };

            return View(model);
        }
    }
}
