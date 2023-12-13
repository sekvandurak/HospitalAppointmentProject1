using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        // Add other relevant properties such as address, contact information, etc.

        // Navigation property for appointments
        public ICollection<Appointment> Appointments { get; set; }
    }
}
