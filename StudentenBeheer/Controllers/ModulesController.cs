using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using StudentenBeheer.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Controllers
{
    [Authorize]
    public class ModulesController : ApplicationController
    {

        private readonly IStringLocalizer<ModulesController> _localizer;

        public ModulesController(ApplicationContext context,
                                        IHttpContextAccessor httpContextAccessor,
                                        ILogger<ApplicationController> logger, IStringLocalizer<ModulesController> localizer) : base(context, httpContextAccessor, logger)
        {
            _localizer = localizer;
        }

        // GET: Modules

        public async Task<IActionResult> Index()
        {
            var module = from m in _context.Module
                         where m.Deleted > DateTime.Now
                         select m;

            return View(await module.ToListAsync());
        }

        //[Authorize]
        // GET: Modules/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Docent,Beheerder")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docent,Beheerder")]
        public async Task<IActionResult> Create([Bind("Id,Name,Omschrijving")] Module @module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Docent,Beheerder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docent,Beheerder")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Omschrijving")] Module @module)
        {
            if (id != @module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Docent,Beheerder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Docent,Beheerder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await _context.Module.FindAsync(id);
            module.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
