namespace HospitalAppointmentProject1.ViewModels
{
    public class AppointmentViewModel
    {
        public DateTime? SelectedTimeSlot { get; set; }
        public string? Major { get; set; }

        // Foreign keys
        public string? UserId { get; set; }
        public int? DoctorId { get; set; }
    }
}
