
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.Data.Interfaces;

using EmployeeApp.Services.UserServiceFolder;
using EmployeeApp.Services.AddressServiceFolder;
using System.Text.RegularExpressions;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.PortalWithAuth.Models;
using EmployeeApp.ServiceApi.Controllers;
using EmployeeApp.Data.Interfaces.UserRepo;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using EmployeeApp.ServiceApi.Controllers.AdressesService;
using Newtonsoft.Json;
using EmployeeApp.ServiceApi.Controllers.GroupsService;

namespace EmployeeApp.PortalWithAuth.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly EmployeeDB2Context _context;
        private readonly IUsersService _empService;
        private readonly IAddressesService _addressService;
        private readonly IGroupsService _grpService;
        private readonly ILogger<UsersController> _logger;
        private readonly List<string> _adminUsers;

        

        public UsersController(EmployeeDB2Context context, 
            IUsersService empservice, 
            IAddressesService addressService,
            IGroupsService grpService,
            ILogger<UsersController> logger,
            List<string> adminUsers )
        {
            _context = context;
            _empService = empservice;
            _addressService = addressService;
            _grpService = grpService;
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

        [Route("/LandingAdminView")]
        public async Task<IActionResult> LandingAdminView()
        {
            return View();
        }

        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [Route("LoginClicked")]
        public async Task<IActionResult> LoginClicked([Bind("Email","Password")]EAGViewModel eag)
        {
            _logger.LogInformation("" + eag.Email);

            ActionResult<User> actionResult = await _empService.getUserByMail(eag.Email);

            // Convert ActionResult<User> to User
            User user = await ConvertActionResultToUserAsync(actionResult);

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
                return View("LoginInvalid");
            }
            
        }

        
        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            // Await the async call to get the result
            ActionResult<IEnumerable<User>> actionResult = await _empService.GetUsers();

            // Convert the ActionResult to IEnumerable<User>
            IEnumerable<User> listOfUsers = await ConvertActionResultToUsersAsync(actionResult);

            // Pass the list of users to the view
            return View(listOfUsers);
        }

        private async Task<IEnumerable<User>> ConvertActionResultToUsersAsync(ActionResult<IEnumerable<User>> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as IEnumerable<User>;
            }
            return Enumerable.Empty<User>(); // Handle other status codes as needed
        }


        [Route("IndexAjax")]

        public async Task<IActionResult> IndexAjax()
        {
            

            return View();
        }
        [Route("IndexAjax1")]

        public async Task<IActionResult> IndexAjax1()
        {
            

            return View();
        }

        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {

            ActionResult<User> actionResult = await _empService.GetUser(id);

            // Convert ActionResult<User> to User
            User user = await ConvertActionResultToUserAsync(actionResult);

            ViewBag.empEditId = user.Id;
            ViewBag.addressEditId = user.Address.Id;
            var eag = new EAGViewModel();

            eag.Name = user.Name;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            eag.DateOfJoining = user.DateOfJoining;
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
        private async Task<string> ConvertActionResultToStringAsync(ActionResult<string> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as string;
            }
            return null; // Handle other status codes as needed
        }

        [Route("DetailsAdminView/{id}")]
        public async Task<IActionResult> DetailsAdminView(int id)
        {
            ActionResult<User> actionResult = await _empService.GetUser(id);

            // Convert ActionResult<User> to User
            User user = await ConvertActionResultToUserAsync(actionResult);
            ViewBag.empEditId = user.Id;
            ViewBag.addressEditId = user.Address.Id;
            var eag = new EAGViewModel();

            eag.Name = user.Name;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            eag.DateOfJoining = user.DateOfJoining;
            eag.IsActive = user.IsActive;
            eag.AddressLine1 = user.Address.AddressLine1;
            eag.AddressLine2 = user.Address.AddressLine2;
            eag.State = user.Address.State;
            eag.Country = user.Address.Country;
            if (user.GroupId!=null)
            {
                int grpid = (int)user.GroupId;
                ActionResult<string> groupNameActionResult = await _grpService.GetGroupName(grpid);
                eag.GroupName = await ConvertActionResultToStringAsync(groupNameActionResult);

            }
            else
            {
                eag.GroupName = "Not assigned";
            }    
            return View(eag);
        }
        [Route("Register")]
        public IActionResult Register()
        {
            List<string> groupNames = _context.Groups.Select(g => g.Name).ToList();
            groupNames.Insert(0, "Select");
            List<SelectListItem> groupSelectListItems = groupNames.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();
            
            ViewBag.GroupName = groupSelectListItems;
                
            if (HttpContext.User.IsInRole("Administrator"))
            {
                return View("RegisterAdminView");
            }
            else
            {
                return View("Register");
            }
            
        }
        [Route("RegisterAdminView")]
        public IActionResult RegisterAdminView()
        {
            List<string> groupNames = _context.Groups.Select(g => g.Name).ToList();
            groupNames.Insert(0, "Select");
            List<SelectListItem> groupSelectListItems = groupNames.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();

            ViewBag.GroupName = groupSelectListItems;
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("RegisterSave")]
        public async Task<IActionResult> RegisterSave
            ([Bind("Name,Email,Phone,Password,ConfirmPassword,AddressLine1,AddressLine2,State,Country")] EAGViewModel eag)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                   
                    var address = new Address
                    {
                        AddressLine1 = eag.AddressLine1,
                        AddressLine2 = eag.AddressLine2,
                        State = eag.State,
                        Country = eag.Country,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1
                    };
                    ActionResult<Address> actionResult = await _addressService.PostAddress(address);

                    // Convert ActionResult<User> to User
                    address = await ConvertActionResultToAddressAsync(actionResult);

                    var user = new User
                    {
                        Name = eag.Name.ToUpper(),
                        AddressId = address.Id,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        CreatedPersonId = 1,
                        LastModifiedPersonId = 1

                    };

                    ActionResult<User> actionResult1 = await _empService.PostUser(user);

                    User emp = await ConvertActionResultToUserAsync(actionResult1);
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
        [Route("RegisterSaveAdmin")]
        public async Task<IActionResult> RegisterSaveAdmin
            ([Bind("Name,Email,Phone,Password,DateOfJoining,GroupName,IsActive,AddressLine1,AddressLine2,State,Country,Password,ConfirmPassword")] EAGViewModel eag)
        {
            
            if (ModelState.IsValid)
            {
                try
                {

                    var address = new Address
                    {
                        AddressLine1 = eag.AddressLine1,
                        AddressLine2 = eag.AddressLine2,
                        State = eag.State,
                        Country = eag.Country,
                        CreatedPersonId = 2,
                        LastModifiedPersonId = 2
                    };
                    ActionResult<Address> actionResult = await _addressService.PostAddress(address);

                    // Convert ActionResult<User> to User
                    address = await ConvertActionResultToAddressAsync(actionResult);
                    

                    var user = new User
                    {
                        Name = eag.Name.ToUpper(),
                        DateOfJoining = eag.DateOfJoining,
                        AddressId = address.Id,
                        IsActive = eag.IsActive,
                        Email = eag.Email,
                        Phone = eag.Phone,
                        Password = eag.Password,
                        CreatedPersonId = 2,
                        LastModifiedPersonId = 2

                    };
                    ActionResult<User> actionResult1 = await _empService.PostUser(user);

                    User emp = await ConvertActionResultToUserAsync(actionResult1);
                    
                    return RedirectToAction(nameof(DetailsAdminView), new { id = user.Id });
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


        [Route("Edit/{id}/{addressid}")]
        public async Task<IActionResult> Edit(int id, int addressid)
        {

            TempData["empEditId"] = id;
            TempData["addEditId"] = addressid;
            
            if (id == null)
            {

                return NotFound();
            }

            ActionResult<User> actionResult = await _empService.GetUser(id);

            User user = await ConvertActionResultToUserAsync(actionResult);

            if (user == null)
            {
                return NotFound();
            }
            EAGViewModel eag = new EAGViewModel();

            eag.Name = user.Name;
            eag.AddressLine1 = user.Address.AddressLine1;
            eag.AddressLine2 = user.Address.AddressLine2;
            eag.State = user.Address.State;
            eag.Country = user.Address.Country;
            eag.Email = user.Email;
            eag.Phone = user.Phone;
            return View(eag);
        }

        private async Task<User> ConvertActionResultToUserAsync(ActionResult<User> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                // Serialize the Value to JSON
                string json = JsonConvert.SerializeObject(okResult.Value);

                // Deserialize the JSON to a User object
                User user = JsonConvert.DeserializeObject<User>(json);

                return user;
            }
            else
            {
                throw new InvalidOperationException("The action did not return a valid user.");
            }
        }
        private async Task<Address> ConvertActionResultToAddressAsync(ActionResult<Address> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                // Serialize the Value to JSON
                string json = JsonConvert.SerializeObject(okResult.Value);

                // Deserialize the JSON to a User object
                Address address = JsonConvert.DeserializeObject<Address>(json);

                return address;
            }
            else
            {
                throw new InvalidOperationException("The action did not return a valid address.");
            }
        }

        [Route("EditAdminView/{id}/{addressid}")]
        public async Task<IActionResult> EditAdminView(int id, int addressid)
        {
            TempData["empEditId"] = id;
            TempData["addEditId"] = addressid;
            // Fetch the user
            ActionResult<User> actionResult = await _empService.GetUser(id);
            User user = await ConvertActionResultToUserAsync(actionResult);

            if (user == null)
            {
                return NotFound();
            }

            EAGViewModel eag = new EAGViewModel
            {
                Name = user.Name,
                DateOfJoining = user.DateOfJoining ?? default,
                GroupName = user.Group?.Name ?? "Not assigned",
                AddressLine1 = user.Address.AddressLine1,
                AddressLine2 = user.Address.AddressLine2,
                State = user.Address.State,
                Country = user.Address.Country,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive,
            };

            // Fetch the groups
            ActionResult<IEnumerable<string>> groupActionResult = await _grpService.GetGroups();

            // Convert the ActionResult to IEnumerable<string>
            IEnumerable<string> groupNames = await ConvertActionResultToStringEnumerableAsync(groupActionResult);

            // Insert "Select" at the beginning of the list
            List<string> groupNamesList = groupNames.ToList();
            groupNamesList.Insert(0, "Select");

            // Convert to List<SelectListItem>
            List<SelectListItem> groupSelectListItems = groupNamesList.Select(name => new SelectListItem
            {
                Value = name,
                Text = name
            }).ToList();

            ViewBag.GroupName = groupSelectListItems;

            return View(eag);
        }

        private async Task<IEnumerable<string>> ConvertActionResultToStringEnumerableAsync(ActionResult<IEnumerable<string>> actionResult)
        {
            if (actionResult.Result is OkObjectResult okResult)
            {
                return okResult.Value as IEnumerable<string>;
            }
            return Enumerable.Empty<string>(); // Handle other status codes as needed
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("EditClicked")]
        public async Task<IActionResult> EditClicked([Bind("Name,AddressLine1,AddressLine2,State,Country,Email,Phone")] EAGViewModel eag)
        {
            int empEditId = (int)TempData["empEditId"];
            int addEditId = (int)TempData["addEditId"];

            ActionResult<User> actionResult = await _empService.GetUser(empEditId);

            // Convert ActionResult<User> to User
            User emp = await ConvertActionResultToUserAsync(actionResult);
            var address = emp.Address;

            address.AddressLine1 = eag.AddressLine1;
            address.AddressLine2 = eag.AddressLine2;
            address.State = eag.State;
            address.Country = eag.Country;
            address.LastModifiedPersonId = 1;
                
            _context.Update(address);
            _context.SaveChanges();

            emp.Name = eag.Name.ToUpper();
            emp.Email = eag.Email;
            emp.Phone = eag.Phone;
            emp.LastModifiedPersonId = 1;
            await _empService.PutUser(emp);
            return RedirectToAction(nameof(Details), new { id = emp.Id });
        }
        [Route("EditClickedByAdmin")]
        public async Task<IActionResult> EditClickedByAdmin([Bind("Name,DateOfJoining,AddressLine1,AddressLine2,State,Country,Email,Phone,GroupName,IsActive")] EAGViewModel eag)
        {
            int empEditId = (int)TempData["empEditId"];
            int addEditId = (int)TempData["addEditId"];

            // Await the task to get ActionResult<User>
            ActionResult<User> actionResult = await _empService.GetUser(empEditId);

            // Convert ActionResult<User> to User
            User emp = await ConvertActionResultToUserAsync(actionResult);

            // Check if the address is already being tracked
            var existingAddress = await _context.Addresses.FindAsync(emp.Address.Id);
            if (existingAddress != null)
            {
                _context.Entry(existingAddress).State = EntityState.Detached;
            }

            // Update the address properties
            var address = emp.Address;
            address.AddressLine1 = eag.AddressLine1;
            address.AddressLine2 = eag.AddressLine2;
            address.State = eag.State;
            address.Country = eag.Country;
            address.LastModifiedPersonId = 2;

            // Attach and update the address
            _context.Attach(address);
            _context.Update(address);
            await _context.SaveChangesAsync();

            // Update the user properties
            emp.Name = eag.Name.ToUpper();
            emp.Email = eag.Email;
            emp.Phone = eag.Phone;
            emp.DateOfJoining = eag.DateOfJoining;
            emp.IsActive = eag.IsActive;
            if (eag.GroupName != "Select")
            {
                emp.GroupId = _context.Groups
                            .Where(g => g.Name == eag.GroupName)
                            .Select(g => g.Id)
                            .FirstOrDefault();
            }
            else
            {
                emp.GroupId = null;
            }
            emp.LastModifiedPersonId = 2;

            // Update the user using the service
            await _empService.PutUser(emp);

            return RedirectToAction(nameof(DetailsAdminView), new { id = emp.Id });
        }
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.DelId = id;
            _logger.LogInformation("inside del");
            ActionResult<User> actionResult = await _empService.GetUser(id);

            // Convert ActionResult<User> to User
            User employee = await ConvertActionResultToUserAsync(actionResult);
            return View(employee);
        }

        [Route("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("inside del confirmed");
            await _empService.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
