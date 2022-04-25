using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicHub.Models;

namespace TechnicHub.Models.DB
{
    //TODO: asynchrone Programmierung
    // Vertrag (Interface) wurde erzeugt
    interface IRepositoryUsers
    {
        Task ConnectAsync();
        Task DisconnectAsync();

        //CRUD-Operationen  (Create Read Update Delete)
        Task<bool> InsertAsync(Profile p);
        Task<bool> DeleteAsync(int userId);
        Task<bool> UpdateAsync(int userId, Profile newUserData);
        Task<Profile> GetUserAsync(int userId);
        Task<List<Profile>> GetAllUsersAsync();

        //weitere Methoden

       Task<bool> LoginAsync(string username, string password);
    }
}
