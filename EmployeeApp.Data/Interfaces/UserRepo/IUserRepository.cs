using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.UserRepo
{
    public interface IUserRepository
    {
        public Task<User> Create(User user);
        public Task<IEnumerable<User>> GetEmployees();
        public Task<User> GetDetails(int? id);
        public Task<User> Edit(User e);
        public Task<bool> Delete(int? id);


        //get service calls
        public Task<User> getUserByName(string name);
        public Task<User> getUserByMail(string mailid);
        public Task<IEnumerable<User>> getUserByNameList(string name);
        public Task<IEnumerable<User>> getUsersByCountry(string name);
        public Task<IEnumerable<User>> getUsersByExperience(int experience);
        public Task<IEnumerable<User>> getUsersByGroup(string grpName);
    }
}
