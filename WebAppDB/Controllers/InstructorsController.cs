using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public InstructorsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.Instructors.Include(i => i.UsernameNavigation);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,LatestQualification,ExpertiseArea,Email,Username")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", instructor.Username);
            return View(instructor);
        }

        public IActionResult Edit()
        {
            return View();
        }
        // GET: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("InstructorId,Name,LatestQualification,ExpertiseArea,Email")] Instructor instructor)
        {

            try
            {
                var instructorIdParam = new SqlParameter("@InstructorID", instructor.InstructorId);
                var nameParam = new SqlParameter("@name", instructor.Name ?? (object)DBNull.Value);
                var latestQualificationParam = new SqlParameter("@latest_qualification", instructor.LatestQualification ?? (object)DBNull.Value);
                var expertiseAreaParam = new SqlParameter("@expertise_area", instructor.ExpertiseArea ?? (object)DBNull.Value);
                var emailParam = new SqlParameter("@email", instructor.Email ?? (object)DBNull.Value);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateInstructor @InstructorID, @name, @latest_qualification, @expertise_area, @email",
                    instructorIdParam, nameParam, latestQualificationParam, expertiseAreaParam, emailParam);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(instructor.InstructorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(instructor);
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorId == id);
        }
        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
