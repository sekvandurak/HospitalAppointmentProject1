using HospitalAppointmentProject1.Models;
using HospitalAppointmentProject1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentProject1.Controllers
{
    [Authorize(Roles = "admin")]

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabiniz Dogrulayiniz ");
                        return View(model);

                    }

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEnabledAsync(user, true);
                        return RedirectToAction("Index", "Home");

                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutTime = await _userManager.GetLockoutEndDateAsync(user);
                        var remainingTime = lockoutTime.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Your account is locked out. Please try again after {remainingTime.Minutes} minutes");
                    }
                    ModelState.AddModelError("", "Invalid Login");
                }
                ModelState.AddModelError("", "Invalid Login");
            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new AppUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", new { Id = user.Id, token = token });


                    //email
                    await _emailSender.EmailSenderAsync(user.Email, "Hesap Onayi", $"Lutfen email hesabinizi onaylamak icin linke <a href='https://localhost:7277{url}'>tiklayiniz.</a>");

                    TempData["message"] = "Email hesabinizdaki onay mailini tiklayiniz";

                    return RedirectToAction("Login", "Account");


                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            if (Id == null || token == null)
            {
                TempData["message"] = "Invalid Token";
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabiniz Onaylandi";
                    return RedirectToAction("Login", "Account");
                }
            }
            TempData["message"] = "Kullanici Bulunamadi";
            return View();

        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData["message"] = "Epostaniza adresiniz giriniz.";

                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                TempData["message"] = "Eposta adresi ile eslesen bir mail bulunamadi.";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = Url.Action("ResetPassword", "Account", new { user.Id, token });
            await _emailSender.EmailSenderAsync(Email, "Parola Sifirlama", $"Parolanizı sifirlamak icin linke <a href='https://localhost:7277{url}'>tiklayiniz.</a>");

            TempData["message"] = "Epostaniza gonderilen link ile parolanizi sifirlayabilirsiniz.";
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string id, string token)
        {
            if (id == null || token == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordModel { Token = token };
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["message"] = "Eposta adresi ile eslesen bir mail bulunamadi.";

                    return RedirectToAction("Login");

                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Parolaniz degistirildi.";
                    return RedirectToAction("Login");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
