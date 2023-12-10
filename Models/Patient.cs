using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public bool IsAppointed { get; set; }

        // Add other relevant properties such as address, contact information, etc.

        // Navigation property for appointments
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
