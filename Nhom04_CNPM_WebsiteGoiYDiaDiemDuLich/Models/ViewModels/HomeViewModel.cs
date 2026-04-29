using System.Collections.Generic;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Place> TrendingPlaces { get; set; }
        public List<Place> TopRatedPlaces { get; set; }
        public List<Place> PopularPlaces { get; set; }
    }
}
