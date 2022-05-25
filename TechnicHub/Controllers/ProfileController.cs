
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

    public class ProfileController : Controller
    {
        private IRepositoryUsers rep = new RepositoryUsersDB();
        public IActionResult Activity()
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
                        HttpContext.Session.SetInt32("logged", 1);
                        HttpContext.Session.SetString("loggedUser", userDataFromForm.Username);

                        return View("_Message", new Message("LogIn", "Der LogIn war erfolgreich!"));

                    }
                    else
                    {
                        HttpContext.Session.SetInt32("logged", 0);
                        return View("_Message", new Message("LogIn", "Der LogIn war NICHT erfolgreich!", "Bitte versuchen Sie es noch einmall!"));
                    }
                }
                catch (DbException)
                {
                    return View("_Message", new Message("LogIn", "Datenbankfehler", "Bitte versuchen Sie es später noch einmal!"));
                }
                catch (NullReferenceException)
                {
                    return View("_Message", new Message("LogIn", "LogIn NICHT erfolgreich", "Bitte geben Sie die richtigen Daten ein!"));
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

        public async Task<ProfileAndLanguages> getProfileInfo(int user_id)
        {
            ProfileAndLanguages p = new ProfileAndLanguages();
            p.Languages = await rep.GetLanguagesAsync(user_id);
            p.Profile = await rep.GetUserAsync(user_id);

            return p;
        }

        public async Task<IActionResult> ProfileData()
        {
            await rep.ConnectAsync();
            int id = await rep.GetUserIdAsync(HttpContext.Session.GetString("loggedUser"));
            Console.WriteLine(id);
            if (id != null)
            {
                ProfileAndLanguages p = await getProfileInfo(id);
                if (p != null)
                {
                    // alle User an die View übergeben
                    return View(p);
                }
            }

            await rep.DisconnectAsync();
            return View("_Message", new Message("Profile", "Es ist ein Fehler aufgetreten!", "Bitte versuchen Sie es später noch einmal!"));

        }

        public IActionResult LogOut()
        {
            HttpContext.Session.SetInt32("logged", 0);
            HttpContext.Session.SetString("loggedUser", "");
            return View("_Message", new Message("Profile", "Sie wurden erfolgreich abgemeldet", "Auf Wiedersehen!"));
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

        [HttpGet]
        public async Task <IActionResult>UpdateProfile(int id)
        {
            try { 
            await rep.ConnectAsync();
                 ProfileAndLanguages p =await  getProfileInfo(id);
                p.Languages = await GetLanguages();
            return View(p);
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
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int id, ProfileAndLanguages userDatafromForm)
        {
            try { 
                await rep.ConnectAsync();
              
                Profile p = userDatafromForm.Profile;
                p.UserId = id;
                List<string> languages = userDatafromForm.Languages;
            
               if(  await rep.UpdateAsync(id, userDatafromForm))
              {
                    return View("_Message", new Message("Update erfolgreich", "User wurde bearbeitet!", ""));
                }
                else
                {
                    return View("_Message", new Message("Update nicht erfolgreich", "User konnte nicht bearbeitet werden", "Versuchen sie später erneut!"));

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

       
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await rep.ConnectAsync();
                bool working =  await rep.DeleteZwAsync(id);
                if (working) {

                    bool working1 = await rep.DeleteUserAsync(id);
                    HttpContext.Session.SetInt32("logged", 0);
                    HttpContext.Session.SetString("loggedUser", "");
                    if (!working1)
                    {
                        return View("_Message", new Message("Löschungsfehler", "User konnte nicht gelöscht werden", "Versuchen sie später erneut!"));
                        
                    }
                    return View("_Message", new Message("Löschung", "User Löschung war erfolgreich"));
                }
                else
                {
                    return View("_Message", new Message("Löschungsfehler", "User konnte nicht gelöscht werden", "Versuchen sie später erneut!"));

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

            if ((p.EMail == null) )
            {
                ModelState.AddModelError("Email", "Eine Email wird benötigt!");
            }
        }

        public IActionResult CheckEMail(string email)
        {
            //mit DB verbinden und überprüfen, ob Mail vorhanden
            if (email == "pedro@gmx.at")
            {
                //Rückgabewert nach JSON konvertieren
                // hier kann auch z.B. die Instant einer Klasse übergeben werden
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult(false);
            }
        }

    }
}