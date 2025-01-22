//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using WebAppDB.Models;

//namespace WebAppDB.Controllers
//{
//    public class LearnersController : Controller
//    {
//        private readonly DatabaseFinalContext _context;

//        public LearnersController(DatabaseFinalContext context)
//        {
//            _context = context;
//        }

//        // GET: Learners
//        public async Task<IActionResult> Index()
//        {
//            var databaseFinalContext = _context.Learners.Include(l => l.UsernameNavigation);
//            return View(await databaseFinalContext.ToListAsync());
//        }

//        // GET: Learners/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var learner = await _context.Learners
//                .Include(l => l.UsernameNavigation)
//                .FirstOrDefaultAsync(m => m.LearnerId == id);
//            if (learner == null)
//            {
//                return NotFound();
//            }

//            return View(learner);
//        }

//        // GET: Learners/Create
//        public IActionResult Create()
//        {
//            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
//            return View();
//        }

//        // POST: Learners/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground,Username")] Learner learner)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(learner);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", learner.Username);
//            return View(learner);
//        }

//        // GET: Learners/Edit/5
//        public async Task<IActionResult> Edit(int? id, string username = null)
//        {
//            if (id == null && string.IsNullOrEmpty(username))
//            {
//                return NotFound();
//            }

//            Learner learner;
//            if (id != null)
//            {
//                learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == id);
//            }
//            else
//            {
//                learner = await _context.Learners
//                    .FromSqlRaw("SELECT * FROM Learner WHERE Username = {0}", username)
//                    .FirstOrDefaultAsync();
//            }

//            if (learner == null)
//            {
//                return NotFound();
//            }

//            return View(learner);
//        }
//        // POST: Learners/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        //[HttpPost]
//        //[ValidateAntiForgeryToken]

//        //public async Task<IActionResult> Edit([Bind("FirstName,LastName,Gender,BirthDate,Country,CulturalBackground,Username")] Learner learner)
//        //{
//        //   // if (ModelState.IsValid)
//        //   // {
//        //        try
//        //        {
//        //            await _context.Database.ExecuteSqlRawAsync(
//        //                "EXEC ProfileUpdate  @learnerID int = {learner}, @ProfileID int, @PreferedContentType varchar(50), @emotional_state varchar(50), @PersonalityType varchar(50)",
//        //                learner.FirstName, learner.LastName, learner.Gender, learner.BirthDate, learner.Country, learner.CulturalBackground, learner.Username);

//        //            return RedirectToAction(nameof(Index));
//        //        }
//        //        catch (DbUpdateConcurrencyException)
//        //        {
//        //            if (!_context.Learners.Any(e => e.LearnerId == learner.LearnerId))
//        //            {
//        //                return NotFound();
//        //            }
//        //            else
//        //            {
//        //                throw;
//        //            }
//        //        }
//        //   // }
//        //    return View(learner);
//        //}

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground,Username")] Learner learner)
//        {
//         //   if (ModelState.IsValid)
//           // {
//                try
//                {
//                    // Define parameters properly for the stored procedure call
//                    var learnerIdParam = new SqlParameter("@learnerID", learner.LearnerId);
//                    var profileIdParam = new SqlParameter("@ProfileID", DBNull.Value); // Adjust as needed
//                    var contentTypeParam = new SqlParameter("@PreferedContentType", DBNull.Value); // Adjust as needed
//                    var emotionalStateParam = new SqlParameter("@emotional_state", DBNull.Value); // Adjust as needed
//                    var personalityTypeParam = new SqlParameter("@PersonalityType", DBNull.Value); // Adjust as needed

//                    // Call the stored procedure
//                    await _context.Database.ExecuteSqlRawAsync(
//                        "EXEC ProfileUpdate @learnerID, @ProfileID, @PreferedContentType, @emotional_state, @PersonalityType",
//                        learnerIdParam, profileIdParam, contentTypeParam, emotionalStateParam, personalityTypeParam);

//                    return RedirectToAction(nameof(Index));
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!_context.Learners.Any(e => e.LearnerId == learner.LearnerId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//           // }

//            return View(learner);
//        }


//        private bool LearnerExists(int id)
//        {
//            return _context.Learners.Any(e => e.LearnerId == id);
//        }

//        // GET: Learners/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var learner = await _context.Learners
//                .Include(l => l.UsernameNavigation)
//                .FirstOrDefaultAsync(m => m.LearnerId == id);
//            if (learner == null)
//            {
//                return NotFound();
//            }

//            return View(learner);
//        }

//        // POST: Learners/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var learner = await _context.Learners.FindAsync(id);
//            if (learner != null)
//            {
//                _context.Learners.Remove(learner);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }


//    }
//}



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
    public class LearnersController : Controller
    {
        private readonly DatabaseFinalContext _context;

        public LearnersController(DatabaseFinalContext context)
        {
            _context = context;
        }

        // GET: Learners
        public async Task<IActionResult> Index()
        {
            var databaseFinalContext = _context.Learners.Include(l => l.UsernameNavigation);
            return View(await databaseFinalContext.ToListAsync());
        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username");
            return View();
        }

        // POST: Learners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground,Username")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Username"] = new SelectList(_context.Users, "Username", "Username", learner.Username);
            return View(learner);
        }
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int LearnerID, string first_name, string last_name, 
            bool gender, DateTime birth_date, string country , string cultural_background)
        {
            var courses1 = await _context.Courses
              .FromSqlRaw("EXEC UpdateLearner @LearnerID = {0}, @first_name = {1}, @last_name = {2}, @gender = {3}, @birth_date = {4}, @country = {5}, @cultural_background = {6}",
        LearnerID,
        first_name,
        last_name,
        gender,
        birth_date,
        country,
        cultural_background)
              .ToListAsync();

            return View(courses1);
        }
        // GET: Learners/Edit/5
        public IActionResult Edit()
        {
            return View();
        }

        // POST: Learners/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground")] Learner learner)
        {

            try
            {
                var learnerIdParam = new SqlParameter("@LearnerID", learner.LearnerId);
                var firstNameParam = new SqlParameter("@first_name", learner.FirstName ?? (object)DBNull.Value);
                var lastNameParam = new SqlParameter("@last_name", learner.LastName ?? (object)DBNull.Value);
                var genderParam = new SqlParameter("@gender", learner.Gender);
                var birthDateParam = new SqlParameter("@birth_date", learner.BirthDate ?? (object)DBNull.Value);
                var countryParam = new SqlParameter("@country", learner.Country ?? (object)DBNull.Value);
                var culturalBackgroundParam = new SqlParameter("@cultural_background", learner.CulturalBackground ?? (object)DBNull.Value);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateLearner @LearnerID, @first_name, @last_name, @gender, @birth_date, @country, @cultural_background",
                    learnerIdParam, firstNameParam, lastNameParam, genderParam, birthDateParam, countryParam, culturalBackgroundParam);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LearnerExists(learner.LearnerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerId == id);
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



        //public async Task<IActionResult> CurrentPath(int learnerId)
        //{
        //    var progress = await _context.Learners
        //       .FromSqlRaw("EXEC CurrentPath @LearnerID = {0}", learnerId)
        //       .ToListAsync();

        //    return View(progress);
        //}
        public async Task<IActionResult> CurrentPath(int learnerId)
        {
            if (learnerId == null)
            {
                return View(new List<LearningPath>()); // Return an empty list if no ID is provided
            }

            var progress = await _context.Database
                .SqlQuery<LearningPath>($"EXEC CurrentPath @LearnerID = {learnerId}")
                .ToListAsync();

            return View(progress);
        }


        public IActionResult ViewLP()
        {
            return View();
        }

        

       

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.UsernameNavigation)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                _context.Learners.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
