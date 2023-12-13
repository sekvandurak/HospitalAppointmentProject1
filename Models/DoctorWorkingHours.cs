namespace HospitalAppointmentProject1.Models
{
    public class DoctorWorkingHours
    {
        public int DoctorWorkingHoursId { get; set; }
        public int DoctorId { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Doctor Doctor { get; set; }
    }
}