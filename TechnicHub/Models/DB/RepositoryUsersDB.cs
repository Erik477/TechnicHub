using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using TechnicHub.Models;

namespace TechnicHub.Models.DB
{
    // diese Klasse implementiert unser Interface
    public class RepositoryUsersDB : IRepositoryUsers
    {
        private string _connString = "Server=localhost;database=technichub;user=root;password=admin";
     
        private DbConnection _conn;
        public async Task ConnectAsync()
        {
           
            if(this._conn == null)
            {
                this._conn = new MySqlConnection(this._connString);
            }
            if(this._conn.State != System.Data.ConnectionState.Open)
            {
             
               await this._conn.OpenAsync();
            }
        }

        public async Task DisconnectAsync()
        {
           if(this._conn != null && this._conn.State == ConnectionState.Open)
            {
                await this._conn.CloseAsync();
            }
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdDelete = this._conn.CreateCommand();

            cmdDelete.CommandText = "delete from users where user_id =@id";
            DbParameter paramId = cmdDelete.CreateParameter();
            paramId.ParameterName = "id";
            paramId.DbType = DbType.Int16;
            paramId.Value = userId;

            cmdDelete.Parameters.Add(paramId);
            int res = await cmdDelete.ExecuteNonQueryAsync();
            return res ==1;


        }

        public async Task<List<Profile>> GetAllUsersAsync()
        {
           if((this._conn ==null) && (this._conn.State != ConnectionState.Open))
            {
                return null;
            }

            List<Profile> users = new List<Profile>();
            DbCommand cmdAllUsers = this._conn.CreateCommand();
            cmdAllUsers.CommandText = "select * from users";
       
               using (DbDataReader reader = await cmdAllUsers.ExecuteReaderAsync())
                {
                while (await reader.ReadAsync())
                {
                    users.Add(new Profile()
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        Username = Convert.ToString(reader["username"]),
                        Password = Convert.ToString(reader["password"]),
                        Birthdate = Convert.ToDateTime(reader["birthdate"]),
                        EMail = Convert.ToString(reader["email"]),
                        Gender = (Gender)Convert.ToInt32(reader["gender"]),

                    });
                }
            }
            return users;
        }

        public async Task<Profile> GetUserAsync(int userId)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return null;
            }
            DbCommand cmdGet = this._conn.CreateCommand();
            cmdGet.CommandText = "select * from users where user_id = id";
            DbParameter paramId = cmdGet.CreateParameter();
            paramId.ParameterName = "id";
            paramId.DbType = DbType.Int16;
            paramId.Value = userId;

            Profile user = null;
            using (DbDataReader reader = await cmdGet.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {

                     user = new Profile()
                    {

                        UserId = Convert.ToInt32(reader["user_id"]),
                        Username = Convert.ToString(reader["username"]),
                        Password = Convert.ToString(reader["password"]),
                        Birthdate = Convert.ToDateTime(reader["birthdate"]),
                        EMail = Convert.ToString(reader["email"]),
                        Gender = (Gender)Convert.ToInt32(reader["gender"])

                    };
                }

            }
            return user;
        }          
        public async Task<bool> InsertAsync(Profile user)
        {
            if( (this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdInsert= this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into users values(null, @username, sha2(@password,512), @bDate, @mail, @gender)";

            DbParameter paramUN = cmdInsert.CreateParameter();
 
            paramUN.ParameterName = "username";
            paramUN.DbType = DbType.String;
            paramUN.Value = user.Username;

            DbParameter paramPWD = cmdInsert.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = DbType.String;
            paramPWD.Value = user.Password;

            DbParameter paramBD = cmdInsert.CreateParameter();
            paramBD.ParameterName = "bDate";
            paramBD.DbType = DbType.DateTime;
            paramBD.Value = user.Birthdate;

            DbParameter paramEmail = cmdInsert.CreateParameter();
            paramEmail.ParameterName = "mail";
            paramEmail.DbType = DbType.String;
            paramEmail.Value = user.EMail;

            DbParameter paramGender =cmdInsert.CreateParameter();
            paramGender.ParameterName = "gender";
            paramGender.DbType = DbType.Int32;
            paramGender.Value = user.Gender;

            cmdInsert.Parameters.Add(paramUN);
            cmdInsert.Parameters.Add(paramPWD);
            cmdInsert.Parameters.Add(paramBD);
            cmdInsert.Parameters.Add(paramEmail);
            cmdInsert.Parameters.Add(paramGender);           

        }
        public async Task<bool> InsertLanguages(PLanguages language)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdInsert = this._conn.CreateCommand();
            //für pLanguages
            cmdInsert.CommandText = "insert into pLanguage values(null, Plang_name)";
            DbParameter paramLanguage = cmdInsert.CreateParameter();
            paramLanguage.ParameterName = "gender";
            paramLanguage.DbType = DbType.Int32;
            paramLanguage.Value = language.;
            return await cmdInsert.ExecuteNonQueryAsync() == 1;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into users values(null, @username, sha2(@password,512), @bDate, @mail, @gender)";
           return await cmdInsert.ExecuteNonQueryAsync() == 1;
           
        }

        public async Task<bool> UpdateAsync(int userId, Profile newUserData)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdUpdate = this._conn.CreateCommand();
            cmdUpdate.CommandText = "update table users set user_id = @id, username = @username," +
                "password = sha2(@password,512), birthdate = @bDate, email = @mail, gender = @gender where user_id = @id";

            DbParameter paramId = cmdUpdate.CreateParameter();
            paramId.ParameterName = "id";
            paramId.DbType = DbType.String;
            paramId.Value = newUserData.UserId;

            DbParameter paramUN = cmdUpdate.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = DbType.String;
            paramUN.Value = newUserData.Username;

            DbParameter paramPWD = cmdUpdate.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = DbType.String;
            paramPWD.Value = newUserData.Password;

            DbParameter paramBD = cmdUpdate.CreateParameter();
            paramBD.ParameterName = "bDate";
            paramBD.DbType = DbType.DateTime;
            paramBD.Value = newUserData.Birthdate;

            DbParameter paramEmail = cmdUpdate.CreateParameter();
            paramEmail.ParameterName = "mail";
            paramEmail.DbType = DbType.String;
            paramEmail.Value = newUserData.EMail;

            DbParameter paramGender = cmdUpdate.CreateParameter();
            paramGender.ParameterName = "gender";
            paramGender.DbType = DbType.Int32;
            paramGender.Value = newUserData.Gender;

            cmdUpdate.Parameters.Add(paramId);
            cmdUpdate.Parameters.Add(paramUN);
            cmdUpdate.Parameters.Add(paramPWD);
            cmdUpdate.Parameters.Add(paramBD);
            cmdUpdate.Parameters.Add(paramEmail);
            cmdUpdate.Parameters.Add(paramGender);

            return await cmdUpdate.ExecuteNonQueryAsync() == 1;
        }
    }
}
