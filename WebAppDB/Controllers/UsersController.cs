using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;
using WebAppDB.Services;

namespace WebAppDB.Controllers
{
    public class UsersController : Controller
    {

        private readonly DatabaseFinalContext _context;
        private readonly UserService _userService;


        public UsersController(DatabaseFinalContext context,UserService userService)
        {
            _context = context;
             _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (_context == null) // Defensive programming: Ensure _context is not null
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }

            // Query the database for the user
            var user = await _userService.LoginAsync(username, password);
            
            if (user != null)
            {
                if (user.Role == "Admin")
                {
                    return RedirectToAction("AdminPage", "Home");
                    return RedirectToAction("Edit", "Users", new { id = user.Username });
                }
                if (user.Role == "Learner")
                {
                    ViewBag.Username = username;
                    return RedirectToAction("LearnerPage", "Home");
                    

                }
                if (user.Role == "Instructor")
                {
                    return RedirectToAction("InstructorPage", "Home");
                }

            }

            // If user not found, return with an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View("Index");
        }


        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,PasswordHash,Role,ProfileImage")] User user)
        {
            if (ModelState.IsValid)
            {

                _context.Add(user);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Username", user.Username);
                if (user.Role == "Learner")
                {
                    var usernameParam = new SqlParameter("@Username", user.Username);
                    await _context.Database.ExecuteSqlRawAsync("EXEC InsertLearnerWithNulls @Username", usernameParam);
                   
                    await _context.SaveChangesAsync();
                }
                if (user.Role == "Instructor")
                {
                    var usernameParam = new SqlParameter("@Username", user.Username);
                    await _context.Database.ExecuteSqlRawAsync("EXEC InsertInstructorWithNulls @Username", usernameParam);


                    await _context.SaveChangesAsync();
                }

                if (user.Role == "Admin")
                {
                    return RedirectToAction("AdminPage", "Home");
                }
                if (user.Role == "Learner")
                {
                    return RedirectToAction( "LearnerPage", "Home");


                }
                if (user.Role == "Instructor")
                {
                    return RedirectToAction( "InstructorPage", "Home");
                }
            }


            return View(user);
        }

        
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public IActionResult COmpOne()
        {
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground,Username")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Using FromSqlRaw to call the stored procedure
                    var query = @"
                EXEC ProfileUpdate 
                    @learnerID = {0}, 
                    @ProfileID = {1}, 
                    @PreferedContentType = {2}, 
                    @emotional_state = {3}, 
                    @PersonalityType = {4}";

                    await _context.Database.ExecuteSqlRawAsync(
                        query,
                        learner.LearnerId,            // Parameter for @learnerID
                        DBNull.Value,                 // Placeholder for @ProfileID (adjust as needed)
                        "NETWORKING",                 // Example value for @PreferedContentType
                        "SATISFIED",                  // Example value for @emotional_state
                        "INTROVERT"                   // Example value for @PersonalityType
                    );

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the error and handle it gracefully
     
                    ModelState.AddModelError("", "An error occurred while updating the profile.");
                }
            }

            return View(learner);
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Username == id);
        }
    }
}
