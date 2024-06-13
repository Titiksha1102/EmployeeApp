using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using EmployeeApp.Data.Interfaces.UserRepo;
using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using EmployeeApp.Data.Interfaces.AddressRepo;

namespace EmployeeApp.ServiceApi.Controllers.UsersService
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersServiceController : ControllerBase,IUsersService
    {

        private readonly IUserRepository _emprepo;
        
        private readonly ILogger<UsersServiceController> _logger;

        public UsersServiceController(IUserRepository emprepo,ILogger<UsersServiceController> logger)
        {
            _logger = logger;
            _emprepo = emprepo;
            
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var result = await _emprepo.GetEmployees();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var result = await _emprepo.GetDetails(id);


            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetByName/{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> getUserByName(string name)
        {
            var result = await _emprepo.getUserByName(name);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }

        [HttpGet("GetByNameList/{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> getUsersByNameList(string name)
        {
            var result = await _emprepo.getUserByNameList(name);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }


        [HttpGet("GetByCountry/{country}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> getUsersByCountry(string country)
        {
            var result = await _emprepo.getUsersByCountry(country);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }
        [HttpGet("GetByExperience/{years}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> getUsersByExperience(int years)
        {
            var result = await _emprepo.getUsersByExperience(years);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }
        [HttpGet("GetByGroup/{grpName}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<User>>> getUsersByGroup(string grpName)
        {
            var result = await _emprepo.getUsersByGroup(grpName);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }

        [HttpPut("EditUser")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PutUser(User user)
        {
            try
            {
                
                var result = await _emprepo.Edit(user);
                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogInformation("inside catch block");
                _logger.LogInformation($"Error in editing user details: {e.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                var result = await _emprepo.Create(user);
                if (result is User)
                    return Ok(result);
                else
                    return StatusCode(500, "Internal server error");

            }
            catch (Exception e)
            {
                _logger.LogInformation("Error in creating user " + e.Message);
                return StatusCode(500, "Internal server error");
            }
            
        }


        [HttpDelete("DeleteUser/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> DeleteUser(int? id)
        {
            var result = await _emprepo.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            else
                return Ok(result);
        }
        
        [HttpGet("getUserByMail/{mailid}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> getUserByMail(string mailid)
        {
            
            var result = await _emprepo.getUserByMail(mailid);
            if (result is User)
                return Ok(result);
            else
                return StatusCode(500, "Internal server error");
        }
    }
}
