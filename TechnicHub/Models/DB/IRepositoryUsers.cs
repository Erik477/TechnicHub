using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicHub.Models;

namespace TechnicHub.Models.DB
{

    interface IRepositoryUsers
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> InsertUserAsync(Profile user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> DeleteZwAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, Profile newUserData);
        Task<bool> UpdateZwAsync(int langid, int userId);
        Task<bool> UpdateAsync(int userId, ProfileAndLanguages newUserData);
        Task<Profile> GetUserAsync(int userId);
        Task<List<Profile>> GetAllUsersAsync();
        Task<List<string>> GetAllPLanguagesAsync();
        Task<bool> InsertAsync(ProfileAndLanguages pl);
        Task<bool> InsertLanguagesAsync(List<string> language, int user_id);
        Task<int> GetUserIdAsync(string username);
        Task<bool> LoginAsync(Profile p);
        Task<bool> InsertPostAsync(Chatpost post, int UserId);
        Task<List<string>> GetLanguagesAsync(int user_id);
        Task<bool> InsertRoomAsync(Chatroom room);
        Task<int> GetMessageCountAsync(int id);
        Task<List<Chatpost>> GetPostsAsync(int id);
        Task<List<Chatroom>> GetRoomsAsync();
        Task<bool> DeletePostAsync(int id);
        Task<bool> DeleteRoomAsync(int id);
    }
}
