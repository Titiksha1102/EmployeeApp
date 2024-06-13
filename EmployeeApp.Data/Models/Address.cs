using System;
using System.Collections.Generic;

namespace EmployeeApp.Data.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int CreatedPersonId { get; set; }
        public DateTime LastModified { get; set; }
        public int LastModifiedPersonId { get; set; }

        /*public virtual User User { get; set; } = null!;*/
    }
}
