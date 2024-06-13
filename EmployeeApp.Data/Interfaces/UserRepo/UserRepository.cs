using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeApp.Data.Interfaces.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(EmployeeDB2Context context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> Create(User user)
        {
            int n=0;
            try
            {
                _context.Users.Add(user);
                n = _context.SaveChanges();
                
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx)
            {
                // Log only the SQL exception message
                _logger.LogError("A SQL exception occurred: {Message}", sqlEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle and log other types of DbUpdateExceptions
                _logger.LogError("A database update exception occurred: {Message}", dbEx.Message);
            }
            catch (Exception ex)
            {
                // Handle and log any other exceptions
                _logger.LogError("An unexpected error occurred: {Message}", ex.Message);
            }
            if (n == 1)
            {
                return user;
            }
            else
            {
                return null;
            }

        }
        public async Task<IEnumerable<User>> GetEmployees()
        {
            _logger.LogInformation("inside repo get call");


            var result = await _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .ToListAsync();


            return result;
        }
        public async Task<User> GetDetails(int? id)
        {
            if (id == null)
            {
                throw new Exception("User id not found");
            }

            var user = _context.Users
                .AsNoTracking()
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Id == id)
                ;
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }
        public async Task<User> getUserByName(string name)
        {
            var user= _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Name == name);
            return user;
        }
        public async Task<User> getUserByMail(string mailid)
        {
            var user = _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .FirstOrDefault(m => m.Email == mailid);
            return user;
        }
        public async Task<IEnumerable<User>> getUserByNameList(string name)
        {
            var result = await _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .Where(m => m.Name==name)
                .ToListAsync();
            return result;
        }
        public async Task<IEnumerable<User>> getUsersByCountry(string country)
        {
            var result = await _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .Where(m => m.Address.Country == country)
                .ToListAsync();
            return result;
        }
        public async Task<IEnumerable<User>> getUsersByExperience(int experience)
        {
            DateTime currentDate = DateTime.Now;

            var employees = _context.Users
             .Include(u=>u.Address)
             .Include(u=>u.Group)
             .Where(u => u.DateOfJoining.HasValue) // Ensure DateOfJoining is not null
             .AsEnumerable() // Switch to client-side evaluation
             .Where(u => (int)((currentDate - u.DateOfJoining.Value).TotalDays / 365) == experience)
             .ToList();

            return employees;
        }
        public async Task<IEnumerable<User>> getUsersByGroup(string grpName)
        {
            var result= await _context.Users
                .Include(e => e.Address)
                .Include(e => e.Group)
                .Where(m => m.Group.Name == grpName)
                .ToListAsync();
            return result;
        }
        public async Task<User> Edit(User user)
        {
            _logger.LogInformation("inside edit repo call");
            try
            {

                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("employee name from repo get: " + user.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                /*if (!EmployeeExists(employee.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }*/
            }
            return user;
        }

        public async Task<bool> Delete(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var employee = _context.Users.FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return false;
            }
            //delete operation
            _context.Users.Remove(employee);
            await _context.SaveChangesAsync();
            var deletedobj = _context.Users.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }
    }
}
