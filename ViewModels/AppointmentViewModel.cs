using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.ViewModels
{
    public class AppointmentViewModel
    {

        public int AppointmentId { get; set; }
        public string? Major { get; set; }

        public int DoctorId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? SelectedTimeSlot { get; set; }
        // Foreign keys
        public string? UserId { get; set; }

    }
}
