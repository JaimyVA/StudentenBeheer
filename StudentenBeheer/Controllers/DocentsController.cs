#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentenBeheer.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class DocentsController : Controller
    {
        private readonly ApplicationContext _context;

        public DocentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Docents
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Docent.Include(d => d.Gender_docent).Include(d => d.user_docent);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Docents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                .Include(d => d.Gender_docent)
                .Include(d => d.user_docent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // GET: Docents/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Docents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Birthday,UserId,GenderId,CreatedDate,DeletedAt")] Docent docent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", docent.GenderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", docent.UserId);
            return View(docent);
        }

        // GET: Docents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent.FindAsync(id);
            if (docent == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", docent.GenderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", docent.UserId);
            return View(docent);
        }

        // POST: Docents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Birthday,UserId,GenderId,CreatedDate,DeletedAt")] Docent docent)
        {
            if (id != docent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocentExists(docent.Id))
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
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", docent.GenderId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", docent.UserId);
            return View(docent);
        }

        // GET: Docents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                .Include(d => d.Gender_docent)
                .Include(d => d.user_docent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // POST: Docents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docent = await _context.Docent.FindAsync(id);
            _context.Docent.Remove(docent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocentExists(int id)
        {
            return _context.Docent.Any(e => e.Id == id);
        }
    }
}
