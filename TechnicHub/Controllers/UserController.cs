
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

    
        public async Task<IActionResult> Chatrooms()
        {
            try
            {
                await rep.ConnectAsync();
                List<Chatroom> rooms = await rep.GetRoomsAsync();
                if (rooms != null)
                {
                    // alle Chaträume an die View übergeben
                    return View(rooms);
                }
                else
                {
                    return View("_Message", new Message("Datenbankfehler", "Keine Verbindung zur Datenbank!", "Versuchen sie es später ernuet!"));
                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler", "Datenbankprobleme", "Versuchen sie es später ernuet!"));
            }
            finally
            {
                await rep.DisconnectAsync();
            }
        }
   
        [HttpPost]
        public async Task<IActionResult> Chatforum(Chatpost post)
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

        [HttpGet]
        public async Task<IActionResult> Chatforum(int chatroom_id)
        {
            try
            {
                await rep.ConnectAsync();
                List<Chatpost> posts = await rep.GetPostsAsync(chatroom_id);
                if (posts != null)
                {
                    // alle User an die View übergeben
                    return View(posts);
                }
                else
                {
                    return View("_Message", new Message("Datenbankfehler", "Keine Verbindung zur Datenbank!", "Versuchen sie es später ernuet!"));
                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler", "Datenbankprobleme", "Versuchen sie es später ernuet!"));
            }
            finally
            {
                await rep.DisconnectAsync();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int chatroom_id)
        {
            try
            {
                
                await rep.ConnectAsync();
                if (await rep.DeleteRoomAsync(chatroom_id))
                {
                    return View("_Message", new Message("Löschung", "Room Löschung war erfolgreich"));
                }
                else
                {
                    return View("_Message", new Message("Löschungsfehler", "Room konnte nicht gelöscht werden", "Versuchen sie später erneut!"));

                }
            }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler", "Keine Verbindung zur Datenbank!", "Versuchen sie es später ernuet!"));
            }
            finally
            {
                await rep.DisconnectAsync();
            }
        }

        [HttpGet]
        public IActionResult AddChatroom()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChatroom(Chatroom room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.InsertRoomAsync(room))
                    {
                        return View("_Message", new Message("Chatroom", "Chatroom wurde erstellt!", ""));
                    }
                    else
                    {
                        return View("_Message", new Message("Chatroom", "Chatroom konnte nicht erstellt werden!", "Versuchen sie es später erneut!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Datenbankfehler", "Datenbankprobleme", "Versuchen sie es später erneut!"));
                }
                finally
                {
                   await rep.DisconnectAsync();
                }
            }
            return View(room);
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
