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
    public class LearnersCollaborationsController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public LearnersCollaborationsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: LearnersCollaborations
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.LearnersCollaborations.Include(l => l.Learner).Include(l => l.Quest);
            return View(await databaseFinalContext.ToListAsync());
        }
        // POST: Courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int learner)
        {
            var quests = await _context.LearnersCollaborations
                .FromSqlRaw("EXEC QuestMembers @LearnerID = {0}", learner)
                .ToListAsync();

            return View(quests);
        }

        // GET: LearnersCollaborations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersCollaboration = await _context.LearnersCollaborations
                .Include(l => l.Learner)
                .Include(l => l.Quest)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learnersCollaboration == null)
            {
                return NotFound();
            }

            return View(learnersCollaboration);
        }




        public IActionResult participate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDL(int LearnerID, int QuestID)
        {
            var score = await _context.LearnersCollaborations
                .FromSqlRaw("EXEC JoinQuestAmir @LearnerID = {0}, @QuestID = {1}", LearnerID, QuestID)
                .ToListAsync();

            return RedirectToAction("InstructorPage", "Home");
        }


        // GET: LearnersCollaborations/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            ViewData["QuestId"] = new SelectList(_context.Collaboratives, "QuestId", "QuestId");
            return View();
        }

        // POST: LearnersCollaborations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,QuestId,Completion")] LearnersCollaboration learnersCollaboration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learnersCollaboration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersCollaboration.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.Collaboratives, "QuestId", "QuestId", learnersCollaboration.QuestId);
            return View(learnersCollaboration);
        }

        // GET: LearnersCollaborations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersCollaboration = await _context.LearnersCollaborations.FindAsync(id);
            if (learnersCollaboration == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersCollaboration.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.Collaboratives, "QuestId", "QuestId", learnersCollaboration.QuestId);
            return View(learnersCollaboration);
        }

        // POST: LearnersCollaborations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,QuestId,Completion")] LearnersCollaboration learnersCollaboration)
        {
            if (id != learnersCollaboration.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnersCollaboration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnersCollaborationExists(learnersCollaboration.LearnerId))
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
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersCollaboration.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.Collaboratives, "QuestId", "QuestId", learnersCollaboration.QuestId);
            return View(learnersCollaboration);
        }

        // GET: LearnersCollaborations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersCollaboration = await _context.LearnersCollaborations
                .Include(l => l.Learner)
                .Include(l => l.Quest)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learnersCollaboration == null)
            {
                return NotFound();
            }

            return View(learnersCollaboration);
        }

        // POST: LearnersCollaborations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learnersCollaboration = await _context.LearnersCollaborations.FindAsync(id);
            if (learnersCollaboration != null)
            {
                _context.LearnersCollaborations.Remove(learnersCollaboration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnersCollaborationExists(int id)
        {
            return _context.LearnersCollaborations.Any(e => e.LearnerId == id);
        }
    }
}
