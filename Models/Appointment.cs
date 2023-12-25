using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        public DateTime? SelectedTimeSlot { get; set; }
        public string? Major { get; set; }

        // Foreign keys
        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        // Navigation properties

        public int? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }

}

