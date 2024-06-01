using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class Group
    {
        public Group()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
