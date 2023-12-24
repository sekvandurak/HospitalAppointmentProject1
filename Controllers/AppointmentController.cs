using HospitalAppointmentProject1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalAppointmentProject1.Controllers
{
    public class AppointmentController : Controller
    {

        // private ApplicationDbContext _context = new ApplicationDbContext();


        private readonly ApplicationDbContext _context;
        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult ScheduleAppointment()
        {
            // Example: Generate time slots for a specific date
            DateTime date = DateTime.Now.Date; // Replace with the desired date
            List<DateTime> timeSlots = GenerateTimeSlots(date);

            // Pass the generated time slots to the view
            ViewBag.TimeSlots = timeSlots;



            return View();
        }

        public static List<DateTime> GenerateTimeSlots(DateTime date)
        {
            // Define working hours and break time
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(17, 0, 0);
            TimeSpan breakStartTime = new TimeSpan(12, 30, 0);
            TimeSpan breakEndTime = new TimeSpan(13, 30, 0);
            int slotDurationMinutes = 15;

            // Initialize list to store time slots
            List<DateTime> timeSlots = new List<DateTime>();

            // Generate time slots from start to end time
            DateTime currentSlot = new DateTime(date.Year, date.Month, date.Day, startTime.Hours, startTime.Minutes, startTime.Seconds);

            while (currentSlot.TimeOfDay < endTime)
            {
                // Exclude time slots during the break
                if (currentSlot.TimeOfDay < breakStartTime || currentSlot.TimeOfDay >= breakEndTime)
                {
                    timeSlots.Add(currentSlot);
                }

                // Move to the next time slot
                currentSlot = currentSlot.AddMinutes(slotDurationMinutes);
            }

            return timeSlots;
        }

        public IActionResult CreateAppointment()
        {
            DateTime date = DateTime.Now.Date; // Replace with the desired date
            List<DateTime> timeSlots = GenerateTimeSlots(date);

            // Pass the generated time slots to the view
            ViewBag.TimeSlots = timeSlots;

            var specialties = _context.Doctors.Select(d => d.Specialty).Distinct().ToList();
            ViewBag.Specialties = specialties;

            //doctor id's
            var doctors = _context.Doctors.Select(d => d.DoctorId).Distinct().ToList();
            ViewBag.Doctors = doctors;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.PatientId = userId;
            return View();
        }
        public IActionResult SaveAppointment(Appointment appointment)
        {
            appointment.PatientId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _context.Appointments.Add(appointment);

            _context.SaveChanges();
            return RedirectToAction("AppointmentList");

        }

        public IActionResult AppointmentList()
        {
            return View(_context.Appointments.ToList());
        }





    }

}
