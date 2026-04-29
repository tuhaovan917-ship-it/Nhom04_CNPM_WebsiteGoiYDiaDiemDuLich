using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Data.Entity;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Controllers
{
    public class UserController : Controller
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        // ============================
        // Hàm lấy UserId thật
        // ============================
        private int GetCurrentUserId()
        {
            if (Session["UserId"] == null)
                return -1;

            return (int)Session["UserId"];
        }

        // ============================
        // 1) Danh sách yêu thích
        // ============================
        public ActionResult Favorites()
        {
            int userId = GetCurrentUserId();
            if (userId == -1)
                return RedirectToAction("Login", "Account");

            var favorites = db.FavoritePlaces
                .Where(f => f.UserId == userId)
                .Include(f => f.Place)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();

            return View(favorites);
        }

        // ============================
        // 2) Thêm yêu thích
        // ============================
        [HttpPost]
        public ActionResult AddFavorite(int placeId)
        {
            int userId = GetCurrentUserId();
            if (userId == -1)
                return RedirectToAction("Login", "Account");

            bool exists = db.FavoritePlaces
                .Any(f => f.UserId == userId && f.PlaceId == placeId);

            if (!exists)
            {
                db.FavoritePlaces.Add(new FavoritePlace
                {
                    UserId = userId,
                    PlaceId = placeId,
                    CreatedAt = DateTime.Now
                });
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        // ============================
        // 3) Xóa yêu thích
        // ============================
        [HttpPost]
        public ActionResult RemoveFavorite(int favoriteId)
        {
            int userId = GetCurrentUserId();
            if (userId == -1)
                return RedirectToAction("Login", "Account");

            var fav = db.FavoritePlaces
                .FirstOrDefault(f => f.FavoriteId == favoriteId && f.UserId == userId);

            if (fav != null)
            {
                db.FavoritePlaces.Remove(fav);
                db.SaveChanges();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
