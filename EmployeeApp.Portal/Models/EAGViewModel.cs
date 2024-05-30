namespace EmployeeApp.Portal.Models
{
    public class EAGViewModel
    {
        
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; } = true; 
        public string Name { get; set; }

        // Address properties
        
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }

        // Group properties
        
        public string GroupName { get; set; }
    }
}
