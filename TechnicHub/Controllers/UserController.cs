
using TechnicHub.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TechnicHub.Models.DB;

namespace TechnicHub.Controllers
{
    public class UserController : Controller
    {
        private IRepositoryUsers rep = new RepositoryUsersDB();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Chatforum()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChatforumAsync(Chatpost post)
        {
            if (post == null)
            {
                return RedirectToAction("Chatforum");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.InsertPostAsync(post))
                    {
                        // return View("_Message", new Message("Posting", "Kommentar wurde veröffentlicht!"));
                        return View(post);
                    }
                    else
                    {
                        return View("_Message", new Message("Posting", "Kommentar konnte nicht veröffentlicht werden!", "Bitte versuchen Sie es später noch eimanl!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Posting", "Datenbankfehler", "Bitte versuchen Sie es später noch eimal!"));
                }
                finally
                {
                    await rep.DisconnectAsync();
                    
                }                
            }
            return View(post);
        }
        public IActionResult Newsletter()
        {
            return View();
        }
        public IActionResult Infopage()
        {
            return View();
        }
    }
}
