using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Happy.Filters
{
    /// <summary>
    /// Requires an authenticated session with Role = User (guest booking flow).
    /// </summary>
    public sealed class RequireUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var role = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (role == "Admin")
            {
                context.Result = new RedirectToActionResult("Dashboard", "AdminDashboard", null);
                return;
            }

            if (role != "User")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
