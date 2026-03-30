using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Happy.Filters
{
    /// <summary>
    /// Requires Role = Admin and a HotelId in session.
    /// </summary>
    public sealed class RequireAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var role = context.HttpContext.Session.GetString("UserRole");
            var hotelId = context.HttpContext.Session.GetString("HotelId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (role == "User")
            {
                context.Result = new RedirectToActionResult("UserDashboard", "Dashboard", null);
                return;
            }

            if (role != "Admin" || string.IsNullOrEmpty(hotelId))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
