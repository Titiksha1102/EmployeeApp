using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.EmployeeServiceFolder
{
    public interface IEmployeeService
    {
        public Task<User> Create(User user);
/*        public Task<IEnumerable<Employee>> GetEmployees();
        public Task<Employee> GetDetails(int? id);
        public Task<Employee> Edit(int? id, Employee e);
        public Task<bool> Delete(int? id);*/

    }
}
