using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? DateOfJoining { get; set; }
        public string Email { get; set; } = null!;
        public long Phone { get; set; }
        public string Password { get; set; } = null!;
        public int AddressId { get; set; }
        public int? GroupId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual Group? Group { get; set; }

        
    }
}
