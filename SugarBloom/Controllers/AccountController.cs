using CakeBake.Constants;
using CakeBake.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CakeBake.Interfaces;
using CakeBake.Data;
using CakeBake.ViewModels.Account;

namespace CakeBake.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;
        }

        private string GenerateOtp()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                if (!existingUser.EmailConfirmed)
                {
                    await SendOtpAsync(
                        existingUser,
                        "Cake & Bake - Email Verification",
                        "Welcome Back to Cake & Bake 🍰");

                    TempData["Success"] =
                        "A new verification code has been sent to your email.";

                    return RedirectToAction(nameof(VerifyOtp),
                        new { email = existingUser.Email });
                }

                ModelState.AddModelError("", "This email is already registered.");

                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);

                await SendOtpAsync(
                    user,
                    "Cake & Bake - Email Verification",
                    "Welcome to Cake & Bake 🍰");

                return RedirectToAction(nameof(VerifyOtp), new { email = user.Email });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please verify your email to activate your Cake & Bake account.");

                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.Admin))
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "No account found with this email.");
                return View(model);
            }

            await SendOtpAsync(
                user,
                "Cake & Bake - Password Reset",
                "Password Reset Verification");

            return RedirectToAction(nameof(ResetPassword), new
            {
                email = user.Email
            });
        }

        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            VerifyOtpViewModel model = new VerifyOtpViewModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            var otp = _context.OtpCodes
                .Where(o => o.UserId == user.Id &&
                            o.OTP == model.OTP &&
                            !o.IsUsed &&
                            o.ExpireAt > DateTime.Now)
                .OrderByDescending(o => o.Id)
                .FirstOrDefault();

            if (otp == null)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            otp.IsUsed = true;

            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Email verified successfully.";

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> ResendOtp(string email, string returnAction)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return RedirectToAction("Register");

            await SendOtpAsync(
                user,
                "Cake & Bake - New Verification Code",
                "New Verification Code");

            TempData["Success"] = "A new verification code has been sent.";

            return RedirectToAction(returnAction, new { email = user.Email });
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            var otp = _context.OtpCodes
                .Where(o => o.UserId == user.Id &&
                            o.OTP == model.OTP &&
                            !o.IsUsed &&
                            o.ExpireAt > DateTime.Now)
                .OrderByDescending(o => o.Id)
                .FirstOrDefault();

            if (otp == null)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            otp.IsUsed = true;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Password has been reset successfully.";

            return RedirectToAction(nameof(Login));
        }

        private async Task SendOtpAsync(
        ApplicationUser user,
        string subject,
        string heading)
        {
            var oldCodes = _context.OtpCodes
                .Where(o => o.UserId == user.Id);

            _context.OtpCodes.RemoveRange(oldCodes);

            string otp = GenerateOtp();

            var otpCode = new OtpCode
            {
                UserId = user.Id,
                OTP = otp,
                ExpireAt = DateTime.Now.AddMinutes(10),
                IsUsed = false
            };

            _context.OtpCodes.Add(otpCode);

            await _context.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email!,
                subject,
                $@"
                <h2>{heading}</h2>

                <p>Your verification code is:</p>

                <h1 style='letter-spacing:5px;color:#D10056;'>{otp}</h1>

                <p>This code will expire in 10 minutes.</p>");
        }
    }
}