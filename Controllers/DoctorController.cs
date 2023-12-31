using HospitalAppointmentProject1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace HospitalAppointmentProject1.Controllers
{
    [Authorize(Roles = "admin")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private ApplicationDbContext _context = new ApplicationDbContext();
        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        public IActionResult Index()
        {
            List<Doctor>? doctors = _context.Doctors.ToList();
            return View(doctors);
        }
        */

        public async Task<IActionResult> DoctorsList()
        {
            List<Doctor> doctorList = new List<Doctor>();
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://localhost:7277/api/DoctorsApi");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            doctorList = JsonConvert.DeserializeObject<List<Doctor>>(jsonResponse);
            return View(doctorList);
        }
        public IActionResult DoctorsBySpecialty(string specialty)
        {
            // Retrieve doctors based on the selected specialty
            var doctors = _context.Doctors.Where(d => d.Specialty == specialty).ToList();

            return View(doctors);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Doctor model)
        {
            _context.Doctors.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("DoctorsList");
        }

        //Edit Ogrenci

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var ogrenci = _context.Ogrenciler.FirstOrDefaultAsync(i => i.OgrenciID == id);
            //bu sekilde gonderdiginde join yapmiyor ve ogrenci uzerinden diger proplara ulasamiyor
            //var ogrenci = await _context.Ogrenciler.FindAsync(id);

            //1 tane include ile KursKayitlarinin icine gittik. ama burada kursId var baslik yok
            // o zaman burdan 1 tane daha ardisik include yapmam lazim ic ice gibi
            var doctor = await _context.Doctors.FirstOrDefaultAsync(o => o.DoctorId == id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //guvenlik onlemi cross site ataklari icin  formu doldurup gonderdikten sonra baska bir siteye yonlendirmek isteyebilirler
        public async Task<IActionResult> Edit(int id, Doctor model)
        {
            if (id != model.DoctorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Doctors.Update(model);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return RedirectToAction("DoctorsList");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorId == id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, Doctor model)
        {
            if (id != model.DoctorId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Doctors.Remove(model);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return RedirectToAction("DoctorsList");
        }


    }

}
