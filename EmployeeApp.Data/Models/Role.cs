using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Role1 { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }
    }
}
