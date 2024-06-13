using EmployeeApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.ServiceApi.Controllers.GroupsService
{
    public interface IGroupsService
    {
        
        public Task<ActionResult<IEnumerable<string>>> GetGroups();
        public Task<ActionResult<Group>> GetGroup(int id);
        public Task<ActionResult<string>> GetGroupName(int id);
        public Task<ActionResult<int>> GetGroupId(string name);
        public Task<ActionResult<Group>> PostGroup(Group group);//create
        public Task<ActionResult<Group>> PutGroup(Group e);//edit
        public Task<ActionResult<bool>> DeleteGroup(int id);
    }
}
