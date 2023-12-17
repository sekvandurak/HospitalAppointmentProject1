using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; } // Primary key


        public string? FirstName { get; set; } = string.Empty;


        public string? LastName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DateOfBirth { get; set; }

        public bool? IsAppointed { get; set; }

        // Add other relevant properties such as address, contact information, etc.

        // Navigation property for appointments
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
