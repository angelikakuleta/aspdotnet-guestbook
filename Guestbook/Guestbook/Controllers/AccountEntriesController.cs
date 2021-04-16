using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Guestbook.Controllers
{
    [Authorize]
    [Route("Account/Entries")]
    public class AccountEntriesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _db;

        public AccountEntriesController(IUnitOfWork db, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var entries = await _db.Entries.GetAll(x => x.ApplicationUserId == userId);
            return View(entries);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var entry = await _db.Entries.GetById(id);
            if (entry == null || entry.ApplicationUserId != userId)
            {
                return NotFound();
            }
            _db.Entries.Remove(entry);
            await _db.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
