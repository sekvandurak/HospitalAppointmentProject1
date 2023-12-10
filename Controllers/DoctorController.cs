using HospitalAppointmentProject1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentProject1.Controllers
{
    public class DoctorController : Controller
    {


        private ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult WorkingHours(int doctorId)
        {
            var doctor = _context.Doctors
                .Include(d => d.WorkingHours)
                .FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // return all doctors
        public IActionResult Doctors()
        {
            List<Doctor> doctors = _context.Doctors.ToList();
            return View(doctors);
        }


    }

}
