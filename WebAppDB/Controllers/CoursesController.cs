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
    public class CoursesController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public CoursesController(DatabaseFinalContext context)
        {
            _context = context;
        }
        // GET: Courses
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        public IActionResult BackButton()
        {

            var username = HttpContext.Session.GetString("Username");

            if (username == "Admin")
            {
                return RedirectToAction("AdminPage", "Home");
            }
            if (username == "Learner")
            {
                return RedirectToAction("LearnerPage", "Home");
            }
            if (username == "Instructor")
            {
                return RedirectToAction("InstructorPage", "Home");
            }
            return RedirectToAction("Home");
        }



        // POST: Courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int learner)
        {
            var courses = await _context.Courses
                .FromSqlRaw("EXEC EnrolledCourses @LearnerID = {0}", learner)
                .ToListAsync();

            return View(courses);
        }
        [HttpGet]
        public async Task<IActionResult> Index2()
        {
            return View(await _context.Courses.ToListAsync());
        }
        // POST: Courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index2(int learner)
        {
            var courses = await _context.Courses
                .FromSqlRaw("EXEC TakenCourses @LearnerID = {0}", learner)
                .ToListAsync();

            return View(courses);
        }
        [HttpGet]
        public async Task<IActionResult> Index3()
        {
            return View(await _context.Courses.ToListAsync());
        }
        // POST: Courses
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index3(int course)
        {
            var courses1 = await _context.Courses
                .FromSqlRaw("EXEC CourseRemove @courseID = {0}", course)
                .ToListAsync();

            return View(courses1);
        }


        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,LearningObjective,CreditPoints,DifficultyLevel,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,LearningObjective,CreditPoints,DifficultyLevel,Description")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
