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
    public class TakenAssessmentsController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public TakenAssessmentsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: TakenAssessments
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.TakenAssessments.Include(t => t.Assessment).Include(t => t.Learner);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: TakenAssessments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takenAssessment = await _context.TakenAssessments
                .Include(t => t.Assessment)
                .Include(t => t.Learner)
                .FirstOrDefaultAsync(m => m.AssessmentId == id);
            if (takenAssessment == null)
            {
                return NotFound();
            }

            return View(takenAssessment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int learner, int assessmentId)
        {
            var score = await _context.TakenAssessments
                .FromSqlRaw("EXEC ViewScore @LearnerID = {0}, @AssessmentID = {1}", learner, assessmentId)
                .ToListAsync();

            return View(score);
        }
        
        // GET: TakenAssessments/Create
        public IActionResult Create()
        {
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id");
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: TakenAssessments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssessmentId,LearnerId,ScoredPoint")] TakenAssessment takenAssessment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(takenAssessment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", takenAssessment.AssessmentId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", takenAssessment.LearnerId);
            return View(takenAssessment);
        }

        // GET: TakenAssessments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takenAssessment = await _context.TakenAssessments.FindAsync(id);
            if (takenAssessment == null)
            {
                return NotFound();
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", takenAssessment.AssessmentId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", takenAssessment.LearnerId);
            return View(takenAssessment);
        }

        // POST: TakenAssessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssessmentId,LearnerId,ScoredPoint")] TakenAssessment takenAssessment)
        {
            if (id != takenAssessment.AssessmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takenAssessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TakenAssessmentExists(takenAssessment.AssessmentId))
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
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", takenAssessment.AssessmentId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", takenAssessment.LearnerId);
            return View(takenAssessment);
        }

        // GET: TakenAssessments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takenAssessment = await _context.TakenAssessments
                .Include(t => t.Assessment)
                .Include(t => t.Learner)
                .FirstOrDefaultAsync(m => m.AssessmentId == id);
            if (takenAssessment == null)
            {
                return NotFound();
            }

            return View(takenAssessment);
        }

        // POST: TakenAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var takenAssessment = await _context.TakenAssessments.FindAsync(id);
            if (takenAssessment != null)
            {
                _context.TakenAssessments.Remove(takenAssessment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakenAssessmentExists(int id)
        {
            return _context.TakenAssessments.Any(e => e.AssessmentId == id);
        }
    }
}
