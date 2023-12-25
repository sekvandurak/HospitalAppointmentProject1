using Microsoft.AspNetCore.Identity;

namespace HospitalAppointmentProject1.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
