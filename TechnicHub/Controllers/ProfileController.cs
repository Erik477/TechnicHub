

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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Profile userDataFromForm)
        {
            //Paramenter Prüfen
            if (userDataFromForm == null)
            {
                return RedirectToAction("Registration");
            }

            //Formulardaten (Registriesungsdaten) überprüfen - validieren
            ValidateLoginData(userDataFromForm);

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
        
        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            ProfileAndLanguages pl = new ProfileAndLanguages();
            pl.Profile = new Profile();
            pl.Languages = await GetLanguages();

            return View(pl);
        }
        private async Task<List<string>> GetLanguages()
        {
            await rep.ConnectAsync();
            return await rep.GetAllPLanguagesAsync();
              
        }
        
        [HttpPost]
        public async Task<IActionResult> Registration(ProfileAndLanguages userDataFromForm)
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
        private void ValidateRegistrationData(ProfileAndLanguages p)
        {
                    //Parameter überprüfen
                    if (p == null)
                    {
                        return;
                    }
                    //Username
                    if ((p.Profile.Username == null) || (p.Profile.Username.Trim().Length < 4))
                    {
                        ModelState.AddModelError("Username", "Der Benutzername muss mindestens 4 Zeichen lang sein!");
                    }
                    //Password
                    if ((p.Profile.Password == null) || (p.Profile.Password.Length < 8))
                    {
                        ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein!");
                    }
                    //+ mind. ein Großbuchstabe, + mind. ein Kleinbuchstabe,
                    //+ mind. ein Sonderzeichen, + // mind. eine Zahl
                    //Birthdate
                    if (p.Profile.Birthdate >= DateTime.Now)
                    {
                        ModelState.AddModelError("Birthdate", "Das Geburtsdatum kann nicht in der Zukunft liegen!   ");
                    }
                    //Email
                    if ((p.Profile.EMail == null) || (p.Profile.EMail.Contains("@.")))
                    {
                        ModelState.AddModelError("EMail", "Die E-Mail Adresse ist ungültig!");
                    }
                    //Gender
                    if(p.Profile.Gender == null)
                    {
                        ModelState.AddModelError("Gender","Sie müssen ein Gener auswählen! Ich weiß es ist schwer :)"); 
                    }                    
                    //Languages
                    if (p.Languages.Count == 0)
                    {
                        ModelState.AddModelError("Languages", "Sie müssen mindestens eine Sprache auswählen!");
                    }
        }
        private void ValidateLoginData(Profile p)
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
        }

    }
}