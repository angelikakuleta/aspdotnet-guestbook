using DataAccessLayer.Entities;
using Guestbook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Guestbook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(AccountEntriesController.Index), "AccountEntriesController");
        }

        public IActionResult Register(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userID = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    var pathToTempalte = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                        + "templates" + Path.DirectorySeparatorChar.ToString() + "ConfirmEmail.html";
                    string htmlBody = "";

                    using (StreamReader sr = System.IO.File.OpenText(pathToTempalte))
                    {
                        htmlBody = sr.ReadToEnd();
                    }
                    htmlBody = string.Format(htmlBody, callbackUrl);

                    await _emailSender.SendEmailAsync(model.Email, 
                        "Welcome to Guestbook! Confirm Your Email",
                        htmlBody);
 
                    return LocalRedirect(returnUrl);
                }
                AddError(result);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl=null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure:true);

                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "This account has been locked out, please try again later.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "You have to confirm your email.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
