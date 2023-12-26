using HospitalAppointmentProject1.Models;
using HospitalAppointmentProject1.ViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public AppointmentController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        private async Task<bool> IsDoctorAvailable(int doctorId, DateTime? selectedTimeSlot)
        {
            // Check if there are any overlapping appointments for the selected doctor at the chosen time slot
            bool isAvailable = await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.StartTime <= selectedTimeSlot &&
                               a.EndTime >= selectedTimeSlot);

            return !isAvailable; // Return true if the doctor is available, false if not
        }


        public async Task<IActionResult> AppointmentList()
        {
            var appointmentList = await _context
                .Appointments
                .Include(a => a.Doctor) // Include the Doctor navigation property
                .Include(a => a.User)
                .ToListAsync();
            return View(appointmentList);
        }

        public async Task<IActionResult> Edit(int AppointmentId)
        {
            if (AppointmentId == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == AppointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                return View(new Appointment
                {
                    StartTime = appointment.SelectedTimeSlot,
                    EndTime = appointment.SelectedTimeSlot?.AddMinutes(15),
                    SelectedTimeSlot = appointment.SelectedTimeSlot,
                    Major = appointment.Major,
                    UserId = appointment.UserId,
                    DoctorId = appointment.DoctorId
                });
            }
            return RedirectToAction("AppointmentList");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int AppointmentId, Appointment model)
        {
            if (AppointmentId != model.AppointmentId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Appointments.Update(model);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return RedirectToAction("AppointmentList");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int AppointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(AppointmentId);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("AppointmentList");
        }

        public async Task<IActionResult> MyAppointment()
        {
            // current user id 
            var userId = _userManager.GetUserId(User);

            // Linq query to get the appointments of the current user
            var userAppointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.User) // Include the User navigation property
                .Include(a => a.Doctor)
                .ToListAsync();

            return View(userAppointments);
        }



    }

}
