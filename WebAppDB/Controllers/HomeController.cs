using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using WebAppDB.Models;
using WebAppDB.Services;

namespace WebAppDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseFinalContext _context;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger, DatabaseFinalContext context, UserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminPage()
        {
            return View();
        }
        public IActionResult LearnerPage()
        {
            return View();
        }
        public IActionResult InstructorPage()
        {
            return View();
        }
        public IActionResult RedirectToAE()
        {
            return View();
        }
        public  IActionResult RedirectToUserDetails()
        {
            // Retrieve the username from session storage
            var username = HttpContext.Session.GetString("Username");
            //var id = await _context.FromSQLraw("EXEC GetLearnerIDByUsername @username", username);
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            return RedirectToAction("Edit", "Users", new { id = username });
        }

        public IActionResult RedirectToEditUserDetails()
        {
            // Retrieve the username from session storage
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            return RedirectToAction("Edit", "Learners");
        }

        public IActionResult RedirectToCreateQuest()
        {
            return RedirectToAction("Create", "Quests");
        }
        


        [HttpPost]
        public IActionResult RedirectToAssessmentDetails(int assId)
        {
            // Redirect to the Details action in the Assessments controller with the entered ID
            return RedirectToAction("Details", "Assessments", new { id = assId });
        }


        public IActionResult RedirectToDeleteUser()
        {
            // Retrieve the username from session storage
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            return RedirectToAction("Delete", "Users", new { id = username });
        }


        public IActionResult Privacy()
        {
            return View();
        }
        

        public async Task<IActionResult> Login(string username, string password)
        {
            // Check if the user exists in the database
            /* var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);*/
            var user = await _userService.LoginAsync(username, password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("AdminPage", "Home");
                }
                if (user.Role == "Learner")
                {
                    
                    return RedirectToAction("LearnerPage", "Home");
                   
                   // return RedirectToAction("Edit", "Users", new { id = user.Username });


                }
                if (user.Role == "Instructor")
                {
                    return RedirectToAction("InstructorPage", "Home");
                }

            }

            // If the user doesn't exist, return to the login page with an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View("Index"); // Return the same login page
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
