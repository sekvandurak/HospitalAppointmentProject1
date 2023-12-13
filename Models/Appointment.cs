using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        // Foreign keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }



    }



}

