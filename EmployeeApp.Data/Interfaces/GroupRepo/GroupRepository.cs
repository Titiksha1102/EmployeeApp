using EmployeeApp.Data.Data;
using EmployeeApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Data.Interfaces.GroupRepo
{
    public class GroupRepository : IGroupRepository
    {
        private readonly EmployeeDB2Context _context;
        public GroupRepository(EmployeeDB2Context context)
        {
            _context = context;
        }
        public async Task<Group> Create(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> Delete(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(a => a.Id == id);
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            var deletedobj = _context.Groups.FirstOrDefault(m => m.Id == id);
            if (deletedobj == null) return true;
            else return false;
        }

        public async Task<Group> Edit(Group group)
        {
            _context.Update(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> GetGroup(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(a => a.Id == id);
            return group;
        }

        public int GetGroupId(string name)
        {
            var groupId= _context.Groups
                                .Where(g => g.Name == name)
                                .Select(g => g.Id)
                                .FirstOrDefault();
            return groupId;
        }

        public string GetGroupName(int id)
        {
            var groupName = _context.Groups
                                .Where(g => g.Id == id)
                                .Select(g => g.Name)
                                .FirstOrDefault();
            return groupName;
        }

        public async Task<IEnumerable<string>> GetGroups()
        {
            List<string> groupNames = _context.Groups.Select(g => g.Name).ToList();
           
            return groupNames;
        }
    }
}
