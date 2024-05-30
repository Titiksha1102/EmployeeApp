using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class Group
    {
        public Group()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
