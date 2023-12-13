using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Specialty { get; set; } = string.Empty;

        public string degree { get; set; }

        // Add other relevant properties such as contact information, etc.

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorWorkingHours> WorkingHours { get; set; }
    }
}
