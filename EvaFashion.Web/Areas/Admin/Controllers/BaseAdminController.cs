using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EvaFashion.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "admin")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", new { area = "Admin" });
            }
            base.OnActionExecuting(context);
        }
    }
}
