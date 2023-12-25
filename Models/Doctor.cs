using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Specialty { get; set; }

        public string? Degree { get; set; }

        // Add other relevant properties such as contact information, etc.

        // Navigation property for appointments

        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<DoctorWorkingHours>? WorkingHours { get; set; }
        public ICollection<AppUser>? Users { get; set; }

    }
}
