using Microsoft.AspNetCore.Identity;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class EAGClientViewModel
    {

        public DateTime? DateOfJoining { get; set; } = null;
        public bool IsActive { get; set; } = true; 
        public string Name { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Password { get; set; }

        // Address properties

        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        

        // Group properties
        
        public string? GroupName { get; set; }

        //common properties
        

    }
}
