using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentenBeheer.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Controllers
{
    [Authorize(Roles = "Docent, Beheerder")]
    public class StudentsController : Controller
    {
        private readonly ApplicationContext _context;

        public StudentsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string nameFilter, char genderFilter, string orderBy)
        {

            var filteredStudents = from m in _context.Student
                                   where m.Deleted > DateTime.Now
                                   select m;

            if (genderFilter != 0)
            {
                filteredStudents = from s in _context.Student
                                   where s.GenderId == genderFilter
                                   select s;
            }


            if (!string.IsNullOrEmpty(nameFilter))
            {
                filteredStudents = from s in filteredStudents
                                   where s.Lastname.Contains(nameFilter) || s.Name.Contains(nameFilter)
                                   orderby s.Lastname, s.Name
                                   select s;
            }

            ViewData["NameField"] = orderBy == "Name" ? "Name_Desc" : "Name";
            ViewData["LastName"] = orderBy == "Lastname" ? "LastName_Desc" : "Lastname";
            ViewData["BirthDay"] = string.IsNullOrEmpty(orderBy) ? "Date_Desc" : "";
            ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");

            switch (orderBy)
            {
                case "LastName":
                    filteredStudents = filteredStudents.OrderBy(m => m.Lastname);
                    break;
                case "LastName_Desc":
                    filteredStudents = filteredStudents.OrderByDescending(m => m.Lastname);
                    break;
                case "Name_Desc":
                    filteredStudents = filteredStudents.OrderByDescending(m => m.Name);
                    break;
                case "Name":
                    filteredStudents = filteredStudents.OrderBy(m => m.Name);
                    break;
                case "Date":
                    filteredStudents = filteredStudents.OrderBy(m => m.Birthday);
                    break;
                case "Date_Desc":
                    filteredStudents = filteredStudents.OrderByDescending(m => m.Birthday);
                    break;


                default:
                    filteredStudents = filteredStudents.OrderBy(m => m.Birthday);
                    break;
            }

            IQueryable<Student> groupsToSelect = from g in _context.Student orderby g.Name select g;

            StudentsIndexViewModel studentviewmodel = new StudentsIndexViewModel()
            {
                NameFilter = nameFilter,
                GenderFilter = genderFilter,
                FilteredStudents = await filteredStudents.Include(s => s.Gender).ToListAsync(),
                ListGenders = new SelectList(await groupsToSelect.ToListAsync(), "ID", "Name", genderFilter)
            };
            return View(studentviewmodel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Lastname,Birthday,GenderId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", student.GenderId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", student.GenderId);
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Lastname,Birthday,GenderId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["GenderId"] = new SelectList(_context.Gender, "ID", "Name", student.GenderId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            student.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
