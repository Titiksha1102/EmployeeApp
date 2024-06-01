using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Interfaces.EmployeeRepo;

namespace EmployeeApp.Services.EmployeeServiceFolder
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo, ILogger<EmployeeService> logger)
        {
            _repo = repo;
            _logger = logger;

        }
        public async Task<User> Create(User u)
        {
            return await _repo.Create(u);
        }
        /*public async Task<IEnumerable<Employee>> GetEmployees()
        {
            _logger.LogInformation("inside service call");
            return await _repo.GetEmployees();
        }*/
        /*public async Task<Employee> GetDetails(int? id)
        {
            return await _repo.GetDetails(id);
        }*/
        /*public async Task<Employee> Edit(int? id, Employee employee)
        {
            _logger.LogInformation("inside edit service call");
            if (id != employee.Id)
            {
                _logger.LogInformation("inside if");
                _logger.LogInformation("id is:" + id);
                _logger.LogInformation("employee id is:" + employee.Id);
                return null;
            }
            return await _repo.Edit(id, employee);

        }*/

        /*public async Task<bool> Delete(int? id)
        {
            return await _repo.Delete(id);
        }*/
    }
}
