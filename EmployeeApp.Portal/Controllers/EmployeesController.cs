
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.Portal.Models;
using EmployeeApp.Services.EmployeeServiceFolder;
using EmployeeApp.Services.AddressServiceFolder;
using System.Text.RegularExpressions;
using System.Numerics;

namespace EmployeeApp.Portal.Controllers
{

    public class EmployeesController : Controller
    {
        private readonly EmployeeDB2Context _context;
        private readonly IEmployeeService _empservice;
        private readonly IAddressService _addressService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(EmployeeDB2Context context, IEmployeeService empservice, IAddressService addressService, ILogger<EmployeesController> logger)
        {
            _context = context;
            _empservice = empservice;
            _addressService = addressService;
            _logger = logger;
        }

        // GET: Employees
        [Route("/")]
        public async Task<IActionResult> Landing()
        {
            
            return View();
        }
        /*public async Task<IActionResult> Index()
        {
            var res =await _empservice.GetEmployees();
            return View(res);
        }*/

        // GET: Employees/Details/5
        [Route("Employees/Details")]
        public async Task<IActionResult> Details(User user)
        {
            _logger.LogInformation("" + user.Name);
            _logger.LogInformation("" + user.Address.AddressLine1);
            var eag = new EAGClientViewModel();

            eag.Name = user.Name;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            

            eag.AddressLine1 = user.Address.AddressLine1;
            _logger.LogInformation(""+eag.AddressLine1);
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

        // GET: Employees/Create
        [Route("Employees/Create")]
        public IActionResult Create()
        {
          
            ViewBag.GroupName = new SelectList(_context.Groups, "Name", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                        CreatedPersonId = eag.createdPersonID,
                        LastModifiedPersonId = eag.lastModifiedPersonID
                    };
                    address = await _addressService.Create(address);
                    _logger.LogInformation(address.ToString());
                    var user = new User
                    {
                        Name = eag.Name,
                        DateOfJoining = eag.DateOfJoining,
                        Address=address,
                        AddressId = address.Id,
                        IsActive = eag.IsActive,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        
                    };
                    _logger.LogInformation("date of joining:" + user.DateOfJoining);
                    user = await _empservice.Create(user);
                    _logger.LogInformation("" + user.Address.AddressLine1);
                    return RedirectToAction(nameof(Details),user);
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

        // GET: Employees/Edit/5
        /*[Route("Employees/Edit/{id}/{addressid}")]
        public async Task<IActionResult> Edit(int? id, int? addressid)
        {
            ViewBag.GroupName = new SelectList(_context.Groups, "Name", "Name");
            TempData["empEditId"] = id;
            TempData["addEditId"] = addressid;
            _logger.LogInformation("inside edit page");
            if (id == null)
            {

                return NotFound();
            }

            var employee = await _empservice.GetDetails(id);

            if (employee == null)
            {
                return NotFound();
            }
            EAGViewModel eag = new EAGViewModel();

            eag.Name = employee.Name;
            eag.DateOfJoining = employee.DateOfJoining;
            eag.AddressLine1 = employee.Address.AddressLine1;
            eag.AddressLine2 = employee.Address.AddressLine2;
            eag.GroupName = employee.Group.Name;
            eag.IsActive = employee.IsActive;
            eag.State = employee.Address.State;
            eag.Country = employee.Address.Country;

            return View(eag);
        }*/

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Employees/EditClicked")]
        public async Task<IActionResult> EditClicked([Bind("Name,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country")] EAGClientViewModel eag)
        {
                int empEditId = (int)TempData["empEditId"];
                int addEditId = (int)TempData["addEditId"];

                var emp = await _empservice.GetDetails(empEditId);
                var address = emp.Address;

                address.AddressLine1 = eag.AddressLine1;
                address.AddressLine2 = eag.AddressLine2;
                address.State = eag.State;
                address.Country = eag.Country;
                
                address = await _addressService.Edit(address);
                var employee = await _empservice.GetDetails(empEditId);

                    employee.Name = eag.Name;
                    *//*employee.DateOfJoining = eag.DateOfJoining;*//*

                employee.GroupId = _context.Groups
                            .Where(g => g.Name == eag.GroupName)
                            .Select(g => g.Id)
                            .FirstOrDefault();
                employee.IsActive = eag.IsActive;
                

                await _empservice.Edit(empEditId,employee);
                return RedirectToAction(nameof(Index));    
        }*/

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
