using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.EmployeeRepo
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDB2Context _context;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(EmployeeDB2Context context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> Create(User user)
        {
            
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
            return user;
        }
        /*public async Task<IEnumerable<Employee>> GetEmployees()
        {
            _logger.LogInformation("inside repo get call");


            var result = await _context.Employees
            .Include(e => e.Address)
            .Include(e => e.Group)
            .ToListAsync();


            return result;
        }*/
        /*public async Task<Employee> GetDetails(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var employee = _context.Employees
                .Include(e => e.Address)
                .Include(e=>e.Group)
                .FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return null;
            }

            return employee;
        }*/

        public async Task<Employee> Edit(int? id, Employee employee)
        {
            _logger.LogInformation("inside edit repo call");
            try
            {

                _context.Update(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation("employee name from repo get: " + employee.Name);
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
            return employee;
        }

        /*public async Task<bool> Delete(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var employee = _context.Employees.FirstOrDefault(m => m.Id == id);
            if (employee == null)
            {
                return false;
            }
            //delete operation
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            var deletedobj = _context.Employees.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }*/
    }
}
