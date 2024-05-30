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
        public Employee Create(Employee e)
        {
            _logger.LogInformation("inside service create call "+e.DateOfJoining);
            return _repo.Create(e);
        }
        public List<Employee> GetEmployees()
        {
            _logger.LogInformation("inside service call");
            return _repo.GetEmployees();
        }
        public Employee GetDetails(int? id)
        {
            return _repo.GetDetails(id);
        }
        public Employee Edit(int? id, Employee employee)
        {
            _logger.LogInformation("inside edit service call");
            if (id != employee.Id)
            {
                _logger.LogInformation("inside if");
                _logger.LogInformation("id is:" + id);
                _logger.LogInformation("employee id is:" + employee.Id);
                return null;
            }
            return _repo.Edit(id, employee);

        }

        public bool Delete(int? id)
        {
            return _repo.Delete(id);
        }
    }
}
