using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Date { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? StartTime { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? EndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
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

