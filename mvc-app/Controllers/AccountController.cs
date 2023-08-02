using Microsoft.AspNetCore.Mvc;
using mvc_app.Data;
using mvc_app.Models;

namespace mvc_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username already existed in database.
                if (_context.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "Username is existed.");
                    return View(user);
                }

                // Create new User object
                var newUser = new User
                {
                    UserName = user.UserName,
                    Password = user.Password
                };

                // Add new user to database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(User user)
        { 
            if (ModelState.IsValid)
            {
                // Find user in the database
                var userInDb = _context.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (userInDb != null)
                {
                    // Login successfully
                    HttpContext.Session.SetString("UserName", userInDb.UserName);
                    return RedirectToAction("Index", "Home");
                }

                TempData["LoginError"] = "Username or password is invalid";

            }            
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
