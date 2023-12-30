using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Doctor
    {
        [Key]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Specialty")]
        public string? Specialty { get; set; }

        [Display(Name = "Degree")]
        public string? Degree { get; set; }

        // Add other relevant properties such as contact information, etc.

        // Navigation property for appointments

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<DoctorWorkingHours>? WorkingHours { get; set; }
        public ICollection<AppUser>? Users { get; set; }

    }
}
