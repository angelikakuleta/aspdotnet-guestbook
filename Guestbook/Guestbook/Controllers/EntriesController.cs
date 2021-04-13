using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Guestbook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Guestbook.Controllers
{
    public class EntriesController : Controller
    {       
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _db;

        public EntriesController(GuestbookContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = new UnitOfWork(context);
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
                           
                await _db.Entries.Add(entry);
                await _db.Save();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }
    }
}
