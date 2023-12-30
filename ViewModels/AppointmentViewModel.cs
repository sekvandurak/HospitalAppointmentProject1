namespace HospitalAppointmentProject1.ViewModels
{
    public class AppointmentViewModel
    {
        public string? Major { get; set; }

        public int DoctorId { get; set; }
        public DateTime? Date { get; set; }

        public DateTime? SelectedTimeSlot { get; set; }
        // Foreign keys
        public string? UserId { get; set; }

    }
}
