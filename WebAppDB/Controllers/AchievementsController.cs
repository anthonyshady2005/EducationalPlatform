﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Controllers
{
    public class AchievementsController : Controller
    {


        private readonly DatabaseFinalContext _context;

        public AchievementsController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: Achievements
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.Achievements.Include(a => a.Badge).Include(a => a.Learner);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: Achievements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements
                .Include(a => a.Badge)
                .Include(a => a.Learner)
                .FirstOrDefaultAsync(m => m.AchievementId == id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        // GET: Achievements/Create
        public IActionResult Create()
        {
            ViewData["BadgeId"] = new SelectList(_context.Badges, "BadgeId", "BadgeId");
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
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

        // POST: Achievements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AchievementId,LearnerId,BadgeId,Description,DateEarned,Type")] Achievement achievement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(achievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BadgeId"] = new SelectList(_context.Badges, "BadgeId", "BadgeId", achievement.BadgeId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", achievement.LearnerId);
            return View(achievement);
        }

        // GET: Achievements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }
            ViewData["BadgeId"] = new SelectList(_context.Badges, "BadgeId", "BadgeId", achievement.BadgeId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", achievement.LearnerId);
            return View(achievement);
        }

        // POST: Achievements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AchievementId,LearnerId,BadgeId,Description,DateEarned,Type")] Achievement achievement)
        {
            if (id != achievement.AchievementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AchievementExists(achievement.AchievementId))
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
            ViewData["BadgeId"] = new SelectList(_context.Badges, "BadgeId", "BadgeId", achievement.BadgeId);
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", achievement.LearnerId);
            return View(achievement);
        }

        // GET: Achievements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements
                .Include(a => a.Badge)
                .Include(a => a.Learner)
                .FirstOrDefaultAsync(m => m.AchievementId == id);
            if (achievement == null)
            {
                return NotFound();
            }

            return View(achievement);
        }

        // POST: Achievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AchievementExists(int id)
        {
            return _context.Achievements.Any(e => e.AchievementId == id);
        }
    }
}
