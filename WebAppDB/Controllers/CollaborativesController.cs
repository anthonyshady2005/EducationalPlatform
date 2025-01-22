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
    public class CollaborativesController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public CollaborativesController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: Collaboratives
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.Collaboratives.Include(c => c.Quest);
            return View(await databaseFinalContext.ToListAsync());
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



        public IActionResult edits()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDL(int QuestID, DateTime deadline)
        {
            var score = await _context.Collaboratives
                .FromSqlRaw("EXEC UpdateDeadlineProc @QuestID = {0}, @deadline = {1}", QuestID, deadline)
                .ToListAsync();

            return RedirectToAction("InstructorPage", "Home");
        }
        // GET: Collaboratives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborative = await _context.Collaboratives
                .Include(c => c.Quest)
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (collaborative == null)
            {
                return NotFound();
            }

            return View(collaborative);
        }

        // GET: Collaboratives/Create
        public IActionResult Create()
        {
            ViewData["QuestId"] = new SelectList(_context.Quests, "QuestId", "QuestId");
            return View();
        }

        // POST: Collaboratives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestId,Deadline,MaxNumParticipants")] Collaborative collaborative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collaborative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestId"] = new SelectList(_context.Quests, "QuestId", "QuestId", collaborative.QuestId);
            return View(collaborative);
        }

        // GET: Collaboratives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborative = await _context.Collaboratives.FindAsync(id);
            if (collaborative == null)
            {
                return NotFound();
            }
            ViewData["QuestId"] = new SelectList(_context.Quests, "QuestId", "QuestId", collaborative.QuestId);
            return View(collaborative);
        }

        // POST: Collaboratives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestId,Deadline,MaxNumParticipants")] Collaborative collaborative)
        {
            if (id != collaborative.QuestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collaborative);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollaborativeExists(collaborative.QuestId))
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
            ViewData["QuestId"] = new SelectList(_context.Quests, "QuestId", "QuestId", collaborative.QuestId);
            return View(collaborative);
        }

        // GET: Collaboratives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collaborative = await _context.Collaboratives
                .Include(c => c.Quest)
                .FirstOrDefaultAsync(m => m.QuestId == id);
            if (collaborative == null)
            {
                return NotFound();
            }

            return View(collaborative);
        }

        // POST: Collaboratives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collaborative = await _context.Collaboratives.FindAsync(id);
            if (collaborative != null)
            {
                _context.Collaboratives.Remove(collaborative);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult UpdateDeadline()
        {
            // Return a new Collaborative instance for the user to fill in.
            // User will input QuestId and Deadline in the form.
            return View(new Collaborative());
        }

        // POST: Collaboratives/UpdateDeadline
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDeadline(Collaborative collaborative)
        {
            if (ModelState.IsValid)
            {
                if (collaborative.Deadline.HasValue)
                {
                    // Convert DateOnly to DateTime. Choose a default time component, e.g. 23:59:59.
                    DateTime dateTimeDeadline = collaborative.Deadline.Value.ToDateTime(new TimeOnly(23, 59, 59));

                    var questIdParam = new SqlParameter("@QuestID", collaborative.QuestId);
                    var deadlineParam = new SqlParameter("@Deadline", dateTimeDeadline);

                    await _context.Database.ExecuteSqlRawAsync($"EXEC DeadlineUpdate @QuestID={questIdParam}, @Deadline={deadlineParam}" );

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "A valid deadline must be provided.");
                }
            }

            return View(collaborative);
        }

        private bool CollaborativeExists(int id)
        {
            return _context.Collaboratives.Any(e => e.QuestId == id);
        }
    }
}
