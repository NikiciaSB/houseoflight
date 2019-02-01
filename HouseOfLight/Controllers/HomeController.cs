using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HouseOfLight.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
namespace HouseOfLight.Controllers
{
    public class HomeController : Controller
    {
        private HouseContext dbContext;
        public HomeController(HouseContext context)
        {
            dbContext = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("reg")]

        public IActionResult Reg()
        {
            return View("Log-Reg");
        }

        [HttpPost("reg")]
        public IActionResult Register(Reg newUser )
        {
            System.Console.WriteLine("-----------------------------------");
            System.Console.WriteLine(newUser);
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Log-Reg");
                    // You may consider returning to the View at this point
                }
                else
                {
                    // Initializing a PasswordHasher object, providing our User class as its
                    PasswordHasher<Reg> Hasher = new PasswordHasher<Reg>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UId",newUser.UserId);
                    HttpContext.Session.SetString("UName",newUser.Username);

                }
                return RedirectToAction("Main", "Main");
            }
            else
            {
                System.Console.WriteLine("-----------------------------------");
                System.Console.WriteLine("User not created");
                return View("Log-Reg");
            }
            
        }

        [HttpPost("login")]
        public IActionResult Login(Login user)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == user.Email1);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email1", "Email doesn't exist, try registering instead");
                    return View("Log-Reg");
                }
                var hasher = new PasswordHasher<Login>();
                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password1);
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Pasword1", "That's not the right password, try the other one");
                    return View("Log-Reg");
                }
                else
                {
                    HttpContext.Session.SetInt32("UId",userInDb.UserId);
                    return RedirectToAction("Main", "Main");
                }
            }
            return RedirectToAction("Log-Reg");
        }
        [HttpGet("logout")]
        public IActionResult Logout(int UserId)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
