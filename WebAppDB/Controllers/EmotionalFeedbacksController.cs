using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Controllers
{
    public class EmotionalFeedBackController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public EmotionalFeedBackController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: EmotionalFeedBack
        public ActionResult Index()
        {
            var activities = _context.LearningActivities
                            .Include(c => c.EmotionalFeedbacks)
                                .ThenInclude(c => c.Learner)
                            .Include(c => c.Module)
                            .OrderByDescending(c => c.ActivityId)
                            .ToList();

            return View(activities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,EmotionalState")] EmotionalFeedback feedback)
        {
            var username = HttpContext.Session.GetString("Username");

            feedback.LearnerId = _context.Learners.Where(c => c.Username == username).Select(c => c.LearnerId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }
    }
}