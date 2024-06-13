using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.PortalWithAuth.Models
{
    public class EAGViewModel
    {
        //User properties
        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; } = null;
        public bool IsActive { get; set; } = true; 
        public string Name { get; set; }
        
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        [DataType(DataType.PhoneNumber)]
        public long Phone { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // Address properties

        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        

        // Group properties
        
        public string? GroupName { get; set; }

        //common properties

        PhoneAttribute number;
    }
}
