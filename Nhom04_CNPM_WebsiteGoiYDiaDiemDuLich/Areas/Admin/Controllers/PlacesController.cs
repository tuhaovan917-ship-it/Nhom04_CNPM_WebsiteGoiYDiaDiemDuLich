using Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Areas.Admin.Controllers
{
    public class PlacesController : BaseAdminController
    {
        TravelSuggestEntities db = new TravelSuggestEntities();

        // GET: Admin/Places
        public ActionResult Index()
        {
            var places = db.Places
                .Include(p => p.PlaceType)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(places);
        }

        // GET: Admin/Places/Create
        public ActionResult Create()
        {
            ViewBag.PlaceTypeId = new SelectList(db.PlaceTypes, "PlaceTypeId", "Name");
            return View();
        }

        // POST: Admin/Places/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Place model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = System.DateTime.UtcNow;
                db.Places.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaceTypeId = new SelectList(db.PlaceTypes, "PlaceTypeId", "Name", model.PlaceTypeId);
            return View(model);
        }

        // GET: Admin/Places/Edit/5
        public ActionResult Edit(int id)
        {
            var place = db.Places.Find(id);
            if (place == null) return HttpNotFound();

            ViewBag.PlaceTypeId = new SelectList(db.PlaceTypes, "PlaceTypeId", "Name", place.PlaceTypeId);
            return View(place);
        }

        // POST: Admin/Places/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Place model)
        {
            var place = db.Places.Find(model.PlaceId);
            if (place == null)
                return HttpNotFound();

            place.Title = model.Title;
            place.Description = model.Description;
            place.City = model.City;
            place.AvgCost = model.AvgCost;
            place.PlaceTypeId = model.PlaceTypeId;
            place.UpdatedAt = DateTime.UtcNow;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Places/Delete/5
        public ActionResult Delete(int id)
        {
            var place = db.Places.Find(id);
            if (place == null) return HttpNotFound();

            return View(place);
        }

        // POST: Admin/Places/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var place = db.Places.Find(id);
            db.Places.Remove(place);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
