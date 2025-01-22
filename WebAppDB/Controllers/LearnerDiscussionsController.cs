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
    public class LearnerDiscussionsController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public LearnerDiscussionsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: LearnerDiscussions
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.LearnerDiscussions.Include(l => l.Forum).Include(l => l.Learner);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: LearnerDiscussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerDiscussion = await _context.LearnerDiscussions
                .Include(l => l.Forum)
                .Include(l => l.Learner)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (learnerDiscussion == null)
            {
                return NotFound();
            }

            return View(learnerDiscussion);
        }

        // GET: LearnerDiscussions/Create
        public IActionResult Create()
        {
            ViewData["ForumId"] = new SelectList(_context.DiscussionForums, "ForumId", "ForumId");
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: LearnerDiscussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumId,LearnerId,Post,Time")] LearnerDiscussion learnerDiscussion)
        {
            
                _context.Add(learnerDiscussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ForumId"] = new SelectList(_context.DiscussionForums, "ForumId", "ForumId", learnerDiscussion.ForumId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnerDiscussion.LearnerId);
            return View(learnerDiscussion);
        }

        // GET: LearnerDiscussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerDiscussion = await _context.LearnerDiscussions.FindAsync(id);
            if (learnerDiscussion == null)
            {
                return NotFound();
            }
            ViewData["ForumId"] = new SelectList(_context.DiscussionForums, "ForumId", "ForumId", learnerDiscussion.ForumId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnerDiscussion.LearnerId);
            return View(learnerDiscussion);
        }

        // POST: LearnerDiscussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ForumId,LearnerId,Post,Time")] LearnerDiscussion learnerDiscussion)
        {
            if (id != learnerDiscussion.ForumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learnerDiscussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerDiscussionExists(learnerDiscussion.ForumId))
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
            ViewData["ForumId"] = new SelectList(_context.DiscussionForums, "ForumId", "ForumId", learnerDiscussion.ForumId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", learnerDiscussion.LearnerId);
            return View(learnerDiscussion);
        }

        // GET: LearnerDiscussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerDiscussion = await _context.LearnerDiscussions
                .Include(l => l.Forum)
                .Include(l => l.Learner)
                .FirstOrDefaultAsync(m => m.ForumId == id);
            if (learnerDiscussion == null)
            {
                return NotFound();
            }

            return View(learnerDiscussion);
        }

        // POST: LearnerDiscussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learnerDiscussion = await _context.LearnerDiscussions.FindAsync(id);
            if (learnerDiscussion != null)
            {
                _context.LearnerDiscussions.Remove(learnerDiscussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerDiscussionExists(int id)
        {
            return _context.LearnerDiscussions.Any(e => e.ForumId == id);
        }
    }
}
