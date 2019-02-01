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
    public class MainController: Controller
    {
        private HouseContext dbContext;
        public MainController(HouseContext context)
        {
            
            dbContext = context;
        }
        
        [HttpGet("main")]

        public IActionResult Main()
        {
            ViewData["Message"] = "Your contact page.";
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            return View();
        }

        [HttpGet("secret")]
        public IActionResult Secret()
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            return View("SecretLore");
        }

        [HttpGet("account")]
        public IActionResult Account()
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Reg user = dbContext.Users
                .Include(a=>a.CreatedComments)
                .Include(b=>b.Messages)
                .ThenInclude(c=>c.Creator)
                .FirstOrDefault(p => p.UserId == HttpContext.Session.GetInt32("UId"));
            ViewBag.User = user;
            return View("Account");
        }

        [HttpGet("community")]
        public IActionResult Community()
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            List<Message> msg = dbContext.Messages
                .Include(a => a.Creator)
                .Include(b => b.Comments)
                .ThenInclude(c=> c.Creator)
                .OrderByDescending(d=>d.CreatedAt)
                .ToList();
                ViewBag.Message = msg;  
                foreach(var m in msg)
                {
                    System.Console.WriteLine("----------------------");
                    System.Console.WriteLine(m.Comments);
                }
                    
            return View("Community");
        }

        [HttpGet("team")]
        public IActionResult Team()
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");            
            return View("Team");
        }

        [HttpPost("pic")]
        public IActionResult Pic(string ProfilePic)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Reg user = dbContext.Users
                .FirstOrDefault(p => p.UserId == HttpContext.Session.GetInt32("UId"));
            user.ProfilePic = ProfilePic;
            System.Console.WriteLine("-------------------------------");
            System.Console.WriteLine(user.ProfilePic);
            dbContext.SaveChanges();
            return RedirectToAction("Account");
        }

        [HttpGet("edit/{UserId}")]
        public IActionResult EditUser(int UserId)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Reg user = dbContext.Users
                .FirstOrDefault(p => p.UserId == HttpContext.Session.GetInt32("UId"));
            return View("Edit");
        }
        [HttpPost("editpic")]
        public IActionResult EditPic(int UserId, Reg editUser)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Reg user = dbContext.Users
                .FirstOrDefault(p => p.UserId == HttpContext.Session.GetInt32("UId"));
            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.Username = editUser.Username;
            user.ProfilePic = editUser.ProfilePic;
            dbContext.SaveChanges();
            return RedirectToAction("Account");
        }

        [HttpPost("message")]
        public IActionResult Message(Message newMsg)
        {
            dbContext.Add(newMsg);
            dbContext.SaveChanges();
            return RedirectToAction("Community");
        }
        
        [HttpPost("comment")]
        public IActionResult Comment(Comment newComnt)
        {
            dbContext.Add(newComnt);
            dbContext.SaveChanges();
            return RedirectToAction("Community");
        }
        [HttpPost("delete")]
        public IActionResult DELETE(int MsgId, int UserId)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Message msg = dbContext.Messages
                .Include(a=> a.Comments)
                .ThenInclude(b=>b.Creator)
                .FirstOrDefault(c=> c.MsgId == MsgId);
                System.Console.WriteLine(MsgId);
                System.Console.WriteLine(UserId);
                dbContext.Remove(msg);
                dbContext.SaveChanges();
            return RedirectToAction("Community");
        }
        [HttpPost("deleted")]
        public IActionResult DeleteComnt(int ComntId, int UserId)
        {
            ViewBag.Id = HttpContext.Session.GetInt32("UId");
            Comment cmt = dbContext.Comments
                .Include(a=> a.Creator)
                .ThenInclude(b=>b.Messages)
                .FirstOrDefault(c=> c.ComntId == ComntId);
                System.Console.WriteLine("------------------------------");
                System.Console.WriteLine(ComntId);
                System.Console.WriteLine(UserId);
                dbContext.Remove(cmt);
                dbContext.SaveChanges();
            return RedirectToAction("Community");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}