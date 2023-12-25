using HospitalAppointmentProject1.Models;
using HospitalAppointmentProject1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

            ViewBag.TimeSlots = timeSlots;
            ViewBag.Specialties = _context.Doctors.Select(d => d.Specialty).Distinct().ToList();

            // Fetch the doctors from the database into memory
            var doctors = _context.Doctors.ToList();

            // Group doctors by specialty using LINQ to Objects
            var doctorsGroupedBySpecialty = doctors
                .GroupBy(d => d.Specialty)
                .ToList(); // Execute the grouping in memory

            var doctorsBySpecialtyDictionary = doctorsGroupedBySpecialty
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(d => new SelectListItem { Value = d.DoctorId.ToString(), Text = d.FirstName }).ToList()
                );

            ViewBag.DoctorsBySpecialty = doctorsBySpecialtyDictionary;

            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel? model)
        {


            // UserId is valid, proceed with creating the Appointment
            var appointment = new Appointment
            {
                StartTime = model.SelectedTimeSlot,
                EndTime = model.SelectedTimeSlot?.AddMinutes(15),
                SelectedTimeSlot = model.SelectedTimeSlot,
                Major = model.Major,
                UserId = model.UserId,
                DoctorId = model.DoctorId
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("AppointmentList");
        }



        public async Task<IActionResult> AppointmentList()
        {
            var appointmentList = await _context.Appointments.ToListAsync();
            return View(appointmentList);
        }

    }

}
