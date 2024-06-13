using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.UserServiceFolder
{
    public interface IUserService
    {
        public Task<User> Create(User user);
        public Task<IEnumerable<User>> GetEmployees();
        public Task<User> GetDetails(int? id);
        public Task<User> Edit(User e);
        public Task<bool> Delete(int? id);

    }
}
