using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Specialty { get; set; }

        // Add other relevant properties such as contact information, etc.

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }
}
