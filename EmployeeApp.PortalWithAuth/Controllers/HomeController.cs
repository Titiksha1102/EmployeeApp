using EmployeeApp.PortalWithAuth.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeApp.PortalWithAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<string> _adminUsers;

        public HomeController(ILogger<HomeController> logger, List<string> adminUsers)
        {
            _logger = logger;
            _adminUsers = adminUsers;
        }

        public async Task<IActionResult> Index()
        {
            
            if (_adminUsers.Contains(HttpContext.User.Identity.Name))
            {
                return RedirectToAction(nameof(AdminPanel));
            }
            else
            {
                return View();
            }
        }
        [ServiceFilter(typeof(CustAuthAttribute))]
        public async Task<IActionResult> AdminPanel()
        {
            return View();
        }

    }
}
