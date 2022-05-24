
using TechnicHub.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TechnicHub.Models.DB;
using Microsoft.AspNetCore.Http;

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


        int chatroom;


        public async Task<IActionResult> Chatforum(int id)
        {
           
                await rep.ConnectAsync();
                List<Chatpost> posts = await rep.GetPostsAsync(id);
                chatroom = id;
                if (posts != null)
                {
                    // alle User an die View übergeben
                    return View(posts);
                }
              
                return View();
        }

    
        public async Task<IActionResult> DeleteRoom(int chatroom_id)
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

        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(Chatpost post)
        {
            post.ChatroomId = chatroom;
            int UserId = 0;
            try
            {
                await rep.ConnectAsync();
                UserId = await rep.GetUserIdAsync(HttpContext.Session.GetString("loggedUser"));
               }
            catch (DbException)
            {
                return View("_Message", new Message("Datenbankfehler", "Datenbankprobleme beim holen der Userid", "Versuchen sie es später erneut!"));
            }
            finally
            {
                await rep.DisconnectAsync();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.InsertPostAsync(post,UserId))
                    {
                        return RedirectToAction("Chatrooms");
                    }
                    else
                    {
                        return View("_Message", new Message("Chatroom", "Post konnte nicht erstellt werden!", "Versuchen sie es später erneut!"));
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
