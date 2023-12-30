using HospitalAppointmentProject1.Models;
using HospitalAppointmentProject1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace HospitalAppointmentProject1.Controllers
{
    [Authorize(Roles = "admin")]
    public class AppointmentController : Controller
    {
        // private ApplicationDbContext _context = new ApplicationDbContext();


        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AppointmentController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public static List<TimeSpan> GenerateTimeSlots(DateTime date)
        {
            // Define working hours and break time
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(17, 0, 0);
            TimeSpan breakStartTime = new TimeSpan(12, 30, 0);
            TimeSpan breakEndTime = new TimeSpan(13, 30, 0);
            int slotDurationMinutes = 15;

            // Initialize list to store time slots
            List<TimeSpan> timeSlots = new List<TimeSpan>();

            // Generate time slots from start to end time
            TimeSpan currentSlot = startTime;

            while (currentSlot < endTime)
            {
                // Exclude time slots during the break
                if (currentSlot < breakStartTime || currentSlot >= breakEndTime)
                {
                    timeSlots.Add(currentSlot);
                }

                // Move to the next time slot
                currentSlot = currentSlot.Add(new TimeSpan(0, slotDurationMinutes, 0));
            }

            return timeSlots;
        }
        [AllowAnonymous]
        public IActionResult CreateAppointment()
        {
            DateTime date = DateTime.Now.Date; // Replace with the desired date
            List<TimeSpan> timeSlots = GenerateTimeSlots(date);

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
            //ViewBag.UserId = _userManager.GetUserId(User);
            ViewBag.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return View();
        }

        private async Task<bool> IsDoctorAvailable(int doctorId, DateTime? selectedTimeSlot)
        {
            // Check if there are any overlapping appointments for the selected doctor at the chosen time slot
            bool isAvailable = await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.StartTime <= selectedTimeSlot &&
                               a.EndTime >= selectedTimeSlot);

            return !isAvailable; // Return true if the doctor is available, false if not
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAppointment(AppointmentViewModel? model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isDoctorAvailable = await IsDoctorAvailable(model.DoctorId, model.SelectedTimeSlot);
            if (!isDoctorAvailable)
            {
                // If the doctor is not available, add a model error and return the view
                ModelState.AddModelError(string.Empty, "The selected doctor is not available at the chosen time slot.");
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            // UserId is valid, proceed with creating the Appointment
            var appointment = new Appointment
            {
                StartTime = model.SelectedTimeSlot,
                EndTime = model.SelectedTimeSlot?.AddMinutes(15),
                SelectedTimeSlot = model.SelectedTimeSlot,
                Major = model.Major,
                UserId = userId,
                DoctorId = model.DoctorId,
                Date = model.Date,
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAppointment");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int AppointmentId)
        {
            if (AppointmentId == 0)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(AppointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewBag.Specialties = _context.Doctors.Select(d => d.Specialty).Distinct().ToList();
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

            DateTime date = DateTime.Now.Date; // Replace with the desired date
            List<TimeSpan> timeSlots = GenerateTimeSlots(date);

            ViewBag.TimeSlots = timeSlots;
            var appointmentModel = new AppointmentViewModel
            {
                AppointmentId = appointment.AppointmentId,
                Major = appointment.Major,
                DoctorId = (int)appointment.DoctorId,
                SelectedTimeSlot = appointment.SelectedTimeSlot,
                UserId = appointment.UserId,
                Date = appointment.Date
            };
            return View(appointmentModel);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(int appointmentId, AppointmentViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            if (appointmentId != model.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var appointment = await _context.Appointments.FindAsync(appointmentId);
                    appointment.Major = model.Major;
                    appointment.Date = model.Date;
                    appointment.SelectedTimeSlot = model.SelectedTimeSlot;
                    appointment.StartTime = model.SelectedTimeSlot;
                    appointment.EndTime = model.SelectedTimeSlot?.AddMinutes(15);
                    appointment.DoctorId = model.DoctorId;
                    appointment.UserId = userId;
                    appointment.AppointmentId = model.AppointmentId;
                    appointment.DoctorId = model.DoctorId;
                    _context.Appointments.Update(appointment);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return RedirectToAction("MyAppointment");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int AppointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(AppointmentId);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("AppointmentList");
        }
        [AllowAnonymous]
        public async Task<IActionResult> MyAppointment()
        {
            // current user id 
            var userId = _userManager.GetUserId(User);

            var userAppointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Doctor)
                .ToListAsync();

            return View(userAppointments);
        }
        public async Task<IActionResult> AppointmentList()
        {
            var appointmentList = await _context
                .Appointments
                .Include(a => a.User)
                .Include(a => a.Doctor)
                .ToListAsync();
            return View(appointmentList);
        }



    }

}
