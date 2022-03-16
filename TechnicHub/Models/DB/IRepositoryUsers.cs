using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicHub.Models;

namespace TechnicHub.Models.DB
{
    //TODO: asynchrone Programmierung
    // Vertrag (Interface) wurde erzeugt
    interface iRepositoryUsers
    {
        void Connect();
        void Disconnect();

        //CRUD-Operationen  (Create Read Update Delete)
        bool Insert(Profile p);
        bool Delete(int userId);
        bool Update(int userId, Profile newUserData);
        Profile GetUser(int userId);
        List<Profile> GetAllUsers();

        //weitere Methoden

       bool Login(string username, string password);
    }
}
