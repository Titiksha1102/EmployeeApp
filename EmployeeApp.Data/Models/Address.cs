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

        public virtual Employee Employee { get; set; } = null!;
    }
}
