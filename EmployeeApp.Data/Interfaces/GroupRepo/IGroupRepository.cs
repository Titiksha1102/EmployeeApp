using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.GroupRepo
{
    public interface IGroupRepository
    {
        public Task<Group> Create(Group group);
        public Task<Group> Edit(Group group);
        public Task<bool> Delete(int id);
        public Task<Group> GetGroup(int id);
        public Task<IEnumerable<string>> GetGroups();
        public string GetGroupName(int id);
        public int GetGroupId(string name);
    }
}
