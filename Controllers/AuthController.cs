using Microsoft.AspNetCore.Mvc;
using Happy.DTOs.Auth;
using Happy.Services.Interfaces;

namespace Happy.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _service.RegisterAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(dto);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _service.LoginAsync(dto);

            if (result == null)
            {
                ModelState.AddModelError("Email", "Invalid credentials");
                return View(dto);
            }

            HttpContext.Session.SetString("AuthToken", result.Token);
            HttpContext.Session.SetString("UserRole", result.Role);
            HttpContext.Session.SetString("UserId", result.UserId.ToString());

            if (result.Role == "Admin")
                return RedirectToAction("Dashboard", "AdminDashboard");

            return RedirectToAction("UserDashboard", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}