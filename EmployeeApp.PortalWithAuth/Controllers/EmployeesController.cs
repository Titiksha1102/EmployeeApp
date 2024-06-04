
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;

using EmployeeApp.Services.EmployeeServiceFolder;
using EmployeeApp.Services.AddressServiceFolder;
using System.Text.RegularExpressions;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.PortalWithAuth.Models;

namespace EmployeeApp.PortalWithAuth.Controllers
{

    public class EmployeesController : Controller
    {
        private readonly EmployeeDB2Context _context;
        private readonly IEmployeeService _empservice;
        private readonly IAddressService _addressService;
        private readonly ILogger<EmployeesController> _logger;
        private readonly List<string> _adminUsers;

        public EmployeesController(EmployeeDB2Context context, 
            IEmployeeService empservice, 
            IAddressService addressService, 
            ILogger<EmployeesController> logger,
            List<string> adminUsers)
        {
            _context = context;
            _empservice = empservice;
            _addressService = addressService;
            _logger = logger;
            _adminUsers = adminUsers;
        }
        

        
        [Route("/")]

        public async Task<IActionResult> Landing()
        {
            return View();

            /*if (_adminUsers.Contains(HttpContext.User.Identity.Name))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Landing");
            }*/
        }
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [Route("LoginClicked")]
        public async Task<IActionResult> LoginClicked([Bind("Email","Password")]EAGClientViewModel eag)
        {
            _logger.LogInformation("" + eag.Email);
            var user = _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Email == eag.Email);

            if (user != null)
            {
                if (user.Password == eag.Password)
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                else
                    return View("LoginInvalid");
            }
            else
            {
                _logger.LogInformation("user object is null");
                return NotFound();
            }
            
        }

        [ServiceFilter(typeof(CustAuthAttribute))]
        [Route("Employees/Index")]

        public async Task<IActionResult> Index()
        {
            var res = _context.Users
            .Include(e => e.Address)
            .Include(e => e.Group)
            .ToList();

            return View(res);
        }


        [Route("Employees/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("" + id);
            var user= _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Id == id);
            ViewBag.empEditId = user.Id;
            ViewBag.addressEditId = user.Address.Id;
            var eag = new EAGClientViewModel();

            eag.Name = user.Name;
            eag.Email = user.Email;
            eag.Phone = user.Phone;


            eag.AddressLine1 = user.Address.AddressLine1;
            eag.AddressLine2 = user.Address.AddressLine2;
            eag.State = user.Address.State;
            eag.Country = user.Address.Country;
            if (user.LastModifiedPersonId == 1)
            {
                eag.GroupName = "Not assigned";
            }
            else
            {
                eag.GroupName = _context.Groups
                                .Where(g => g.Id == user.GroupId)
                                .Select(g => g.Name)
                                .FirstOrDefault();
            }
            return View(eag);
        }

        
        [Route("Employees/Create")]
        public IActionResult Create()
        {
            ViewBag.GroupName = new SelectList(_context.Groups, "Name", "Name");
            if (HttpContext.User.IsInRole("Administrator"))
            {
                return View("CreateByAdmin");
            }
            else
            {
                return View("Register");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Employees/CreateSave")]
        public async Task<IActionResult> CreateSave([Bind("Name,Email,Phone,Password,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country,createdPersonID,lastModifiedPersonID")] EAGClientViewModel eag)
        {
            _logger.LogInformation("inside create save");
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(eag.ToString());
                    var address = new Address
                    {
                        AddressLine1 = eag.AddressLine1,
                        AddressLine2 = eag.AddressLine2,
                        State = eag.State,
                        Country = eag.Country,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1
                    };
                    address = await _addressService.Create(address);
                    _logger.LogInformation(address.ToString());
                    var user = new User
                    {
                        Name = eag.Name,
                        DateOfJoining = eag.DateOfJoining,
                        AddressId = address.Id,
                        IsActive = eag.IsActive,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1

                    };
                    _logger.LogInformation("date of joining:" + user.DateOfJoining);
                    user = await _empservice.Create(user);
                    _logger.LogInformation("" + user.Address.AddressLine1);
                    _logger.LogInformation("" + user.Id);
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }
                
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }
                
            
            return NotFound();
        }
        [Route("Employees/CreateSaveAdmin")]
        /*public async Task<IActionResult> CreateSaveAdmin([Bind("Name,Email,Phone,Password,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country,createdPersonID,lastModifiedPersonID")] EAGClientViewModel eag)
        {
            _logger.LogInformation("inside create save");
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(eag.ToString());
                    var address = new Address
                    {
                        AddressLine1 = eag.AddressLine1,
                        AddressLine2 = eag.AddressLine2,
                        State = eag.State,
                        Country = eag.Country,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1
                    };
                    address = await _addressService.Create(address);
                    _logger.LogInformation(address.ToString());
                    var user = new User
                    {
                        Name = eag.Name,
                        DateOfJoining = eag.DateOfJoining,

                        AddressId = address.Id,
                        IsActive = eag.IsActive,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1

                    };
                    _logger.LogInformation("date of joining:" + user.DateOfJoining);
                    user = await _empservice.Create(user);
                    _logger.LogInformation("" + user.Address.AddressLine1);
                    _logger.LogInformation("" + user.Id);
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                }

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }


            return NotFound();
        }
*/
        
        [Route("Employees/Edit/{id}/{addressid}")]
        public async Task<IActionResult> Edit(int? id, int? addressid)
        {

            TempData["empEditId"] = id;
            TempData["addEditId"] = addressid;
            _logger.LogInformation("inside edit page");
            if (id == null)
            {

                return NotFound();
            }

            var user = _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            EAGClientViewModel eag = new EAGClientViewModel();

            eag.Name = user.Name;
            eag.DateOfJoining = user.DateOfJoining;
            eag.AddressLine1 = user.Address.AddressLine1;
            eag.AddressLine2 = user.Address.AddressLine2;
            eag.State = user.Address.State;
            eag.Country = user.Address.Country;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            return View(eag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Employees/EditClicked")]
        public async Task<IActionResult> EditClicked([Bind("Name,AddressLine1,AddressLine2,State,Country,Email,Phone")] EAGClientViewModel eag)
        {
            int empEditId = (int)TempData["empEditId"];
            int addEditId = (int)TempData["addEditId"];

            var emp = _context.Users
            .Include(e => e.Address)
            .Include(e => e.Group)
            .FirstOrDefault(m => m.Id == empEditId);
            var address = emp.Address;

            address.AddressLine1 = eag.AddressLine1;
            address.AddressLine2 = eag.AddressLine2;
            address.State = eag.State;
            address.Country = eag.Country;
                
            _context.Update(address);
            _context.SaveChanges();

            emp.Name = eag.Name;
            emp.Email = eag.Email;
            emp.Phone = emp.Phone;
            _context.Update(emp);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = emp.Id });
        }

        // GET: Employees/Delete/5
        /*[Route("Employees/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.DelId = id;
            _logger.LogInformation("inside del");
            var employee=await _empservice.GetDetails(id);
            return View(employee);
        }*/

        // POST: Employees/Delete/5


        /*[Route("Employees/DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("inside del confirmed");
            await _empservice.Delete(id);
            return RedirectToAction(nameof(Index));
        }*/

        /*private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }*/
    }
}
