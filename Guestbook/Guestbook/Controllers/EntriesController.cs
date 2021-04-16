using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Guestbook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utility.Service;

namespace Guestbook.Controllers
{
    public class EntriesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _db;

        public EntriesController(IUnitOfWork db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;         
        }

        public async Task<IActionResult> Index(string? searchString, int pageNumber = 1, int pageSize = 5)
        {
            if (searchString != null && searchString.Length < 3)
            {
                searchString = null;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPageSize"] = pageSize;
            ViewData["CurrentPageNumber"] = pageNumber;

            var count = await _db.Entries.Count(searchString);
            var entries = await _db.Entries.GetAll(searchString, pageNumber, pageSize);

            return View(new PaginatedList<Entry>(entries, count, pageNumber, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _db.Entries.GetById((int)id);

            if (entry == null)
            {
                return NotFound();
            }
            
            if (entry.IsConfirmed == false)
            {
                return View("ConfirmEntry");
            }

            return View(entry);
        }

        public async Task<ActionResult> Create()
        {

            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId != null)
            {
                var applicationUser = await _db.ApplicationUsers.GetById(userId);
                ViewData["UserEmail"] = applicationUser.Email;
                ViewData["UserName"] = applicationUser.Name;
            }  

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,Email,Comment")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                entry.EntryTime = DateTime.Now;

                if (_signInManager.IsSignedIn(User))
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    if (userId != null)
                    {
                        entry.ApplicationUserId = userId;
                        entry.IsConfirmed = true;
                    }
                }
                else if((await _db.ApplicationUsers.GetAll()).Select(x => x.Email.ToLower()).Contains(entry.Email.ToLower()))
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
                           
                await _db.Entries.Add(entry);
                await _db.Save();

                if (entry.IsConfirmed)
                {
                    return RedirectToAction(nameof(Details), "Entries", new { id = entry.Id });    
                }
                else
                {
                    var code = _tokenService.GenerateNewToken(entry);
                    var callbackUrl = Url.Action("ConfirmEntry", "Entries", new { code }, protocol: HttpContext.Request.Scheme);
                    string htmlBody = $"Please confirm your Entry <blockquote>{entry.Comment}</blockquote> clicking on <a href=\"{callbackUrl}\">this link</a>.";
                    htmlBody = string.Format(htmlBody, callbackUrl);

                    await _emailSender.SendEmailAsync(entry.Email,
                        "Welcome guest! Confirm Your Entry",
                        htmlBody);

                    return RedirectToAction(nameof(Details), "Entries", new { id = entry.Id });
                }              
            }
            return RedirectToAction(nameof(Create));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEntry(string? code)
        {
            if (code == null)
            {
                return View("Error");
            }

            var token = new JwtSecurityTokenHandler().ReadJwtToken(code);
            int entryId = int.Parse(token.Payload.Claims.ToList()
                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value);
            
            var entry = await _db.Entries.GetById(entryId);
            if (entry == null)
            {
                return View("Error");
            }
            else if(entry.IsConfirmed == false)
            {
                entry.IsConfirmed = true;
                await _db.Save();
            }
  
            return RedirectToAction(nameof(Details), "Entries", new { id = entry.Id });
        }
    }
}
