using System.Web.Mvc;

namespace Nhom04_CNPM_WebsiteGoiYDiaDiemDuLich.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null || Session["Role"]?.ToString() != "Admin")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" },
                        { "area", "" }
                    }
                );
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
