using System.ComponentModel.DataAnnotations;

namespace HospitalAppointmentProject1.Models
{
    public class DoctorWorkingHours
    {
        [Key]
        public int DoctorId { get; set; }
        public int DoctorWorkingHoursId { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Navigation properties
        public Doctor Doctor { get; set; } = null!;
    }
}