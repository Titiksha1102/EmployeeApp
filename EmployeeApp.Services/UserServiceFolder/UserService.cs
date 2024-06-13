using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data.Interfaces.UserRepo;

namespace EmployeeApp.Services.UserServiceFolder
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo, ILogger<UserService> logger)
        {
            _repo = repo;
            _logger = logger;

        }
        public async Task<User> Create(User u)
        {
            return await _repo.Create(u);
        }
        public async Task<IEnumerable<User>> GetEmployees()
        {
            _logger.LogInformation("inside service call");
            return await _repo.GetEmployees();
        }
        public async Task<User> GetDetails(int? id)
        {
            return await _repo.GetDetails(id);
        }
        public async Task<User> Edit(User employee)
        {
            _logger.LogInformation("inside edit service call");
            /*if (id != employee.Id)
            {
                _logger.LogInformation("inside if");
                _logger.LogInformation("id is:" + id);
                _logger.LogInformation("employee id is:" + employee.Id);
                return null;
            }*/
            return await _repo.Edit(employee);

        }

        public async Task<bool> Delete(int? id)
        {
            return await _repo.Delete(id);
        }
    }
}
