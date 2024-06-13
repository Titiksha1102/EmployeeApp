using EmployeeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.ServiceApi.Controllers.UsersService
{
    public interface IUsersService
    {
        //CRUD
        public Task<ActionResult<User>> PostUser(User user);//create
        public Task<ActionResult<IEnumerable<User>>> GetUsers();
        public Task<ActionResult<User>> GetUser(int id);
        public Task<ActionResult<User>> PutUser(User e);//edit
        public Task<ActionResult<bool>> DeleteUser(int? id);
        //Misc
        public Task<ActionResult<User>> getUserByName(string name);
        public Task<ActionResult<User>> getUserByMail(string mailid);
        public Task<ActionResult<IEnumerable<User>>> getUsersByNameList(string name);
        public Task<ActionResult<IEnumerable<User>>> getUsersByCountry(string country);
        public Task<ActionResult<IEnumerable<User>>> getUsersByExperience(int years);
        public Task<ActionResult<IEnumerable<User>>> getUsersByGroup(string grpName);

    }
}
