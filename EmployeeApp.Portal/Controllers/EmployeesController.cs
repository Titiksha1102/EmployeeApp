using System;
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
        public async Task<IActionResult> Index()
        {
            var res = _empservice.GetEmployees();
            return View(res);
        }

        // GET: Employees/Details/5
        [Route("Employees/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var employee = _empservice.GetDetails(id);

            return View(employee);
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
        public async Task<IActionResult> CreateSave([Bind("Name,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country")] EAGViewModel eag)
        {
            _logger.LogInformation(""+eag.DateOfJoining);
       
            if (ModelState.IsValid)
            {
                var address = new Address
                {

                    AddressLine1 = eag.AddressLine1,
                    AddressLine2 = eag.AddressLine2,
                    State = eag.State,
                    Country = eag.Country
                };
                address=_addressService.Create(address);
                var employee = new Employee
                {
                    Name = eag.Name,
                    DateOfJoining = eag.DateOfJoining,
                    AddressId = address.Id,
                    GroupId = _context.Groups
                                .Where(g => g.Name == eag.GroupName)
                                .Select(g => g.Id)
                                .FirstOrDefault(),
                    IsActive = eag.IsActive
                };
                _logger.LogInformation("" + employee.DateOfJoining);
                _logger.LogInformation("address id: " + employee.AddressId);
                employee = _empservice.Create(employee);
                _logger.LogInformation("" + employee.DateOfJoining);
                _logger.LogInformation("emp id: " + employee.Id);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }
                
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        [Route("Employees/Edit/{id}/{addressid}")]
        public async Task<IActionResult> Edit(int? id,int? addressid)
        {
            ViewBag.GroupName = new SelectList(_context.Groups, "Name", "Name");
            TempData["empEditId"]=id;
            TempData["addEditId"] = addressid;
            _logger.LogInformation("inside edit page");
            if (id == null)
            {

                return NotFound();
            }

            var employee = _empservice.GetDetails(id);
            
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
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Employees/EditClicked")]
        public async Task<IActionResult> EditClicked([Bind("Name,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country")] EAGViewModel eag)
        {
                int empEditId = (int)TempData["empEditId"];
                int addEditId = (int)TempData["addEditId"];

                var address = _empservice.GetDetails(empEditId).Address;


                address.AddressLine1 = eag.AddressLine1;
                address.AddressLine2 = eag.AddressLine2;
                address.State = eag.State;
                address.Country = eag.Country;
                
                address = _addressService.Edit(address);
                var employee = _empservice.GetDetails(empEditId);

                    employee.Name = eag.Name;
                    employee.DateOfJoining = eag.DateOfJoining;

                employee.GroupId = _context.Groups
                            .Where(g => g.Name == eag.GroupName)
                            .Select(g => g.Id)
                            .FirstOrDefault();
                employee.IsActive = eag.IsActive;
                

                _empservice.Edit(empEditId,employee);
                return RedirectToAction(nameof(Index));    
        }

        // GET: Employees/Delete/5
        [Route("Employees/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.DelId = id;
            _logger.LogInformation("inside del");
            var employee=_empservice.GetDetails(id);
            return View(employee);
        }

        // POST: Employees/Delete/5
        
        
        [Route("Employees/DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("inside del confirmed");
            _empservice.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
