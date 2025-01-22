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
    public class LearnersMasteriesController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public LearnersMasteriesController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: LearnersMasteries
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.LearnersMasteries.Include(l => l.Learner).Include(l => l.SkillMastery);
            return View(await databaseFinalContext.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int learner)
        {
            var courses = await _context.LearnersMasteries
                .FromSqlRaw("EXEC QuestProgress @LearnerID = {0}", learner)
                .ToListAsync();

            return View(courses);
        }

        // GET: LearnersMasteries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersMastery = await _context.LearnersMasteries
                .Include(l => l.Learner)
                .Include(l => l.SkillMastery)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learnersMastery == null)
            {
                return NotFound();
            }

            return View(learnersMastery);
        }

        // GET: LearnersMasteries/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            ViewData["QuestId"] = new SelectList(_context.SkillMasteries, "QuestId", "Skill");
            return View();
        }

        // POST: LearnersMasteries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,QuestId,Skill,CompletionStatus")] LearnersMastery learnersMastery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learnersMastery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersMastery.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.SkillMasteries, "QuestId", "Skill", learnersMastery.QuestId);
            return View(learnersMastery);
        }

        // GET: LearnersMasteries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersMastery = await _context.LearnersMasteries.FindAsync(id);
            if (learnersMastery == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersMastery.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.SkillMasteries, "QuestId", "Skill", learnersMastery.QuestId);
            return View(learnersMastery);
        }

        // POST: LearnersMasteries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,QuestId,Skill,CompletionStatus")] LearnersMastery learnersMastery)
        {
            if (id != learnersMastery.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnersMastery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnersMasteryExists(learnersMastery.LearnerId))
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
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnersMastery.LearnerId);
            ViewData["QuestId"] = new SelectList(_context.SkillMasteries, "QuestId", "Skill", learnersMastery.QuestId);
            return View(learnersMastery);
        }

        // GET: LearnersMasteries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnersMastery = await _context.LearnersMasteries
                .Include(l => l.Learner)
                .Include(l => l.SkillMastery)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learnersMastery == null)
            {
                return NotFound();
            }

            return View(learnersMastery);
        }

        // POST: LearnersMasteries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learnersMastery = await _context.LearnersMasteries.FindAsync(id);
            if (learnersMastery != null)
            {
                _context.LearnersMasteries.Remove(learnersMastery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnersMasteryExists(int id)
        {
            return _context.LearnersMasteries.Any(e => e.LearnerId == id);
        }
    }
}
