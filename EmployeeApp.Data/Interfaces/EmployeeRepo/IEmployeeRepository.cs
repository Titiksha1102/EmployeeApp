using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.EmployeeRepo
{
    public interface IEmployeeRepository
    {
        public Employee Create(Employee employee);
        public List<Employee> GetEmployees();
        public Employee GetDetails(int? id);
        public Employee Edit(int? id, Employee e);
        public bool Delete(int? id);
    }
}
