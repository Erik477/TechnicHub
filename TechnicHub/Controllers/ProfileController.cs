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
   private IRepositoryUsers rep = new RepositoryUsersDB();
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

        public async Task<IActionResult> Login(Profile userDataFromForm)
        {
            //Paramenter Prüfen
            if (userDataFromForm == null)
            {
                return RedirectToAction("Registration");
            }

            //Formulardaten (Registriesungsdaten) überprüfen - validieren
            ValidateRegistrationData(userDataFromForm);

            //falls alle daten des Formulars richtig sind
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.LoginAsync(userDataFromForm))
                    {
                        return View("_Message", new Message("LogIn", "Der LogIn war erfolgreich!"));
                    }
                    else
                    {
                        return View("_Message", new Message("LogIn", "Der LogIn war NICHT erfolgreich!", "Bitte versuchen Sie es noch einaml!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("LogIn", "Datenbankfehler", "Bitte versuchen Sie es später noch eimanl!"));
                }
                finally
                {
                    await rep.DisconnectAsync();
                }

                //Redirect zu einer anderen Methode in einem anderem Controller
                // return View("_Message", new Message("Registrierung","Ihre Registrierung war erfolgreich"));
            }
            //Falls das Formular nicht richtig ausgefüllt wurde werden die eingeg. daten erneut angezeigt

            return View(userDataFromForm);
        }

        public async Task<IActionResult> Registration(Profile userDataFromForm)
        {//Paramenter Prüfen
            if (userDataFromForm == null)
            {
                return RedirectToAction("Registration");
            }

            //Formulardaten (Registriesungsdaten) überprüfen - validieren
            ValidateRegistrationData(userDataFromForm);

            //falls alle daten des Formulars richtig sind
            if (ModelState.IsValid)
            {
                try
                {
                    await rep.ConnectAsync();
                    if (await rep.InsertAsync(userDataFromForm))
                    {
                        return View("_Message", new Message("Registrierung", "Ihre Registrierung war erfolgreich!"));
                    }
                    else
                    {
                        return View("_Message", new Message("Registrierung", "Ihre Registrierung war NICHT erfolgreich!", "Bitte versuchen Sie es später noch eimanl!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("Registrierung", "Datenbankfehler", "Bitte versuchen Sie es später noch eimanl!"));
                }
                finally
                {
                    await rep.DisconnectAsync();
                }

                //Redirect zu einer anderen Methode in einem anderem Controller
                // return View("_Message", new Message("Registrierung","Ihre Registrierung war erfolgreich"));
            }
            //Falls das Formular nicht richtig ausgefüllt wurde werden die eingeg. daten erneut angezeigt

            return View(userDataFromForm);
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