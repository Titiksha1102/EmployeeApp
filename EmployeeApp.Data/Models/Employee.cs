using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateOfJoining { get; set; }
        public int AddressId { get; set; }
        public int GroupId { get; set; }
        public bool IsActive { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual Group Group { get; set; } = null!;
    }
}
