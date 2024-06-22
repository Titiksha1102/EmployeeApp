using EmployeeApp.Data.Interfaces.GroupRepo;
using EmployeeApp.Data.Models;
using EmployeeApp.ServiceApi.Controllers.UsersService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace EmployeeApp.ServiceApi.Controllers.GroupsService
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsServiceController : ControllerBase,IGroupsService
    {
        private readonly IGroupRepository _grprepo;

        private readonly ILogger<GroupsServiceController> _logger;

        public GroupsServiceController(IGroupRepository grprepo, ILogger<GroupsServiceController> logger)
        {
            _logger = logger;
            _grprepo = grprepo;

        }
        [HttpDelete("DeleteGroup")]
        public async Task<ActionResult<bool>> DeleteGroup(int id)
        {
            return await _grprepo.Delete(id);
        }
        [HttpGet("GetGroup")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            return await _grprepo.GetGroup(id);
        }
        [HttpGet("GetGroupId/{name}")]
        public async Task<ActionResult<int>> GetGroupId(string name)
        {
            return _grprepo.GetGroupId(name);
        }
        [HttpGet("GetGroupName/{id}")]
        public async Task<ActionResult<string>> GetGroupName(int id)
        {
            return await _grprepo.GetGroupName(id);
        }
        [HttpGet("GetGroups")]
        public async Task<ActionResult<IEnumerable<string>>> GetGroups()
        {
            var result=await _grprepo.GetGroups();
            return Ok(result);
        }
        [HttpPost("CreateGroup")]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            var result = await _grprepo.Create(group);
            if (result is Group)
                return Ok(result);
            else
                return StatusCode(500, "Internal server error");
        }
        [HttpPut("EditGroup")]
        public async Task<ActionResult<Group>> PutGroup(Group g)
        {
            var result = await _grprepo.Edit(g);
            if (result is Group)
                return Ok(result);
            else
                return StatusCode(500, "Internal server error");
        }
    }
}
