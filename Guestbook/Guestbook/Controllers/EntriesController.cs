﻿using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guestbook.Controllers
{
    public class EntriesController : Controller
    {
        private readonly IEntryRepository _entries;

        public EntriesController(GuestbookContext context)
        {
            _entries = new EntryRepository(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _entries.GetAll());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _entries.GetById((int)id);

            if (entry == null)
            {
                return NotFound();
            }

            return View(entry);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name,Email,Comment")] Entry entry)
        {
            if (ModelState.IsValid)
            {
                entry.EntryTime = DateTime.Now;
                await _entries.Add(entry);
                await _entries.Save();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }
    }
}