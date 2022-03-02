using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicHub.Models.DB
{
   interface IRepositoryUsers
    {
        void Connect();
        void Disconnect();

        bool Insert(Profile user);
        bool Delete(int userId);
        bool Update(Profile newUserData);
        Profile GetUser(int userId);
        List<Profile> GetAllUsers();

        bool Login(Profile newUserData);
    }
}
