using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Models.ViewModels
{
    public class ProfileViewModel
    {
        public User UserInfo { get; set; }
        public List<SearchHistory> SearchHistories { get; set; }
        public List<TravelPlan> Plans { get; set; }
        public List<Place> FavoritePlaces { get; set; }

        // thêm dòng này
        public Dictionary<int, string> PlanTokens { get; set; }
    }
}