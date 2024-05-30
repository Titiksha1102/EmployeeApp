using EmployeeApp.Data.Models;
using EmployeeApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service) {
            _service = service;
        }
        public IActionResult Index()
        {
            var responsefromdb = _service.GetAsync();
            ViewBag.res = responsefromdb;
            return View();
        }
    }
}
