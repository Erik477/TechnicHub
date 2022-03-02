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
    public class ProfileController : Controller
   {
   private iRepositoryUsers rep = new RepositoryUsersDB();
        public IActionResult Activity()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Register(Profile userDataFromForm)
        {
            //Parameter überprüfen
            if (userDataFromForm == null)
            {
                return RedirectToAction("Registration");
            }
            //Formulardaten (Registrierungsdaten) überprüfen - Validierung
            ValidateRegistrationData(userDataFromForm);
            //falls alle Daten des Formulars richtig sind
            if (ModelState.IsValid)
            {
                try
                {
                    rep.Connect();
                    if (rep.Insert(userDataFromForm))
                    {
                        return View("_Message", new Message("Registrierung", "Ihre Registrierung war erfolgreich!"));
                    }
                    else {
                        return View("_Message", new Message("Registrierung", "Ihre Registrierung war NICHT erfolgreich!", "" +
                            "Bitte versuchen Sie es später erneut!"));
                    }
                }
                catch (DbException e)
                {
                    return View("_Message", new Message("Registrierung", "Datenbankfehler!", "" +
                          "Bitte versuchen Sie es später erneut!"));
                }
                finally
                {
                    rep.Disconnect();
                }
            }
            return View();
        }
                private void ValidateRegistrationData(Profile p)
                {
                    //Parameter überprüfen
                    if (p == null)
                    {
                        return;
                    }
                    //Username
                    if ((p.Username == null) || (p.Username.Trim().Length < 4))
                    {
                        ModelState.AddModelError("Username", "Der Benutzername muss mindestens 4 Zeichen lang sein!");
                    }
                    //Password
                    if ((p.Password == null) || (p.Password.Length < 8))
                    {
                        ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!");
                    }
                    //+ mind. ein Großbuchstabe, + mind. ein Kleinbuchstabe,
                    //+ mind. ein Sonderzeichen, + // mind. eine Zahl
                    //Birthdate
                    if (p.Birthdate >= DateTime.Now)
                    {
                        ModelState.AddModelError("Birthdate", "Das Geburtsdatum kann nicht in der Zukunft liegen!   ");
                    }
                    //Email

                    //Gender
                }
            }
}