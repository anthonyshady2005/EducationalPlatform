using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Controllers
{
    public class CourseEnrollmentsController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public CourseEnrollmentsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: CourseEnrollments
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.CourseEnrollments.Include(c => c.Course).Include(c => c.Learner);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: CourseEnrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrollment = await _context.CourseEnrollments
                .Include(c => c.Course)
                .Include(c => c.Learner)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (courseEnrollment == null)
            {
                return NotFound();
            }

            return View(courseEnrollment);
        }

        // GET: CourseEnrollments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: CourseEnrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,CourseId,LearnerId,CompletionDate,EnrollmentDate,Status")] CourseEnrollment courseEnrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", courseEnrollment.CourseId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", courseEnrollment.LearnerId);
            return View(courseEnrollment);
        }

        // GET: CourseEnrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrollment = await _context.CourseEnrollments.FindAsync(id);
            if (courseEnrollment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", courseEnrollment.CourseId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", courseEnrollment.LearnerId);
            return View(courseEnrollment);
        }

        // POST: CourseEnrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,CourseId,LearnerId,CompletionDate,EnrollmentDate,Status")] CourseEnrollment courseEnrollment)
        {
            if (id != courseEnrollment.EnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseEnrollmentExists(courseEnrollment.EnrollmentId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", courseEnrollment.CourseId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", courseEnrollment.LearnerId);
            return View(courseEnrollment);
        }

        // GET: CourseEnrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrollment = await _context.CourseEnrollments
                .Include(c => c.Course)
                .Include(c => c.Learner)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (courseEnrollment == null)
            {
                return NotFound();
            }

            return View(courseEnrollment);
        }

        // POST: CourseEnrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseEnrollment = await _context.CourseEnrollments.FindAsync(id);
            if (courseEnrollment != null)
            {
                _context.CourseEnrollments.Remove(courseEnrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseEnrollmentExists(int id)
        {
            return _context.CourseEnrollments.Any(e => e.EnrollmentId == id);
        }
        // GET: CourseEnrollments/CheckPrerequisites
        public IActionResult CheckPrerequisites()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: CourseEnrollments/CheckPrerequisites
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckPrerequisites(int learnerId, int courseId)
        {
            var prerequisitesCompleted = await CheckPrerequisitesAsync(learnerId, courseId);
            ViewData["PrerequisitesCompleted"] = prerequisitesCompleted;
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", courseId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnerId);
            return View();
        }
        public IActionResult BackButton()
        {

            var Role = HttpContext.Session.GetString("Role");
            if (Role == "Admin")
            {
                return RedirectToAction("AdminPage", "Home");
            }
            if (Role == "Learner")
            {
                return RedirectToAction("LearnerPage", "Home");
            }
            if (Role == "Instructor")
            {
                return RedirectToAction("InstructorPage", "Home");
            }
            return RedirectToAction("Home");
        }


        private async Task<bool> CheckPrerequisitesAsync(int learnerId, int courseId)
        {
            var connection = _context.Database.GetDbConnection();
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = "Prerequisites";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@LearnerID", learnerId));
                command.Parameters.Add(new SqlParameter("@CourseID", courseId));

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                await connection.CloseAsync();

                return result != null && result.ToString() == "All prerequisites are completed.";
            }
        }

    }
}
