using HospitalAppointmentProject1.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentProject1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View();
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        //login
        public IActionResult Login(String email, String password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null && user.Email == email && user.Password == password)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Email", "Email or password is wrong");
            return View();
        }

    }
}
