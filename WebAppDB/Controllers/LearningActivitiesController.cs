using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Controllers
{
    public class LearningActivitiesController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public LearningActivitiesController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: LearningActivities
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.LearningActivities.Include(l => l.Module);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: LearningActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningActivity = await _context.LearningActivities
                .Include(l => l.Module)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (learningActivity == null)
            {
                return NotFound();
            }

            return View(learningActivity);
        }

        // GET: LearningActivities/Create
        // GET: LearningActivities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LearningActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,ModuleId,CourseId,ActivityType,InstructionDetails,MaxPoints")] LearningActivity learningActivity)
        {
            if (ModelState.IsValid)
            {
                // Check if the ModuleId and CourseId exist in the database
                var moduleExists = await _context.Modules.AnyAsync(m => m.ModuleId == learningActivity.ModuleId && m.CourseId == learningActivity.CourseId);
                if (!moduleExists)
                {
                    ModelState.AddModelError(string.Empty, "The specified ModuleId and CourseId do not exist.");
                    return View(learningActivity);
                }

                _context.Add(learningActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(learningActivity);
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

        // GET: LearningActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningActivity = await _context.LearningActivities.FindAsync(id);
            if (learningActivity == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", learningActivity.ModuleId);
            return View(learningActivity);
        }

        // POST: LearningActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,ModuleId,CourseId,ActivityType,InstructionDetails,MaxPoints")] LearningActivity learningActivity)
        {
            if (id != learningActivity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningActivityExists(learningActivity.ActivityId))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleId", learningActivity.ModuleId);
            return View(learningActivity);
        }

        // GET: LearningActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningActivity = await _context.LearningActivities
                .Include(l => l.Module)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (learningActivity == null)
            {
                return NotFound();
            }

            return View(learningActivity);
        }

        // POST: LearningActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningActivity = await _context.LearningActivities.FindAsync(id);
            if (learningActivity != null)
            {
                _context.LearningActivities.Remove(learningActivity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningActivityExists(int id)
        {
            return _context.LearningActivities.Any(e => e.ActivityId == id);
        }
    }
}
