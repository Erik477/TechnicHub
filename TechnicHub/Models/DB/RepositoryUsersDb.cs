using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;


namespace TechnicHub.Models.DB
{
    public class RepositoryUsersDb : IRepositoryUsers
    {
        private string _connstring = "Server=localhost;database=TechnicHub;user=root;password=admin";

        private DbConnection _conn;

        public void Connect()
        {
            if (this._conn == null)
            {
               this._conn = new MySqlConnection(this._connstring);
            }
            if (this._conn.State != System.Data.ConnectionState.Open)
            {
                this._conn.Open();
            }
        }

        public bool Delete(int userId)
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdDelete = this._conn.CreateCommand();

            cmdDelete.CommandText = "delete from users where user_id = @userId";

            DbParameter paramUI = cmdDelete.CreateParameter();
            paramUI.ParameterName = "userId";
            paramUI.DbType = System.Data.DbType.String;
            paramUI.Value = userId;

            cmdDelete.Parameters.Add(paramUI);


            return cmdDelete.ExecuteNonQuery() == 1;
        }

        public void Disconnect()
        {
            if (this._conn != null && this._conn.State == System.Data.ConnectionState.Open)
            {
                this._conn.Close();
            }
        }

        public List<Profile> GetAllUsers()
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return null;
            }

            List<Profile> users = new List<Profile>();
            DbCommand cmdAllUsers = this._conn.CreateCommand();
            cmdAllUsers.CommandText = "select * from users";

            using (DbDataReader reader = cmdAllUsers.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(
                        new Profile()
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Username = Convert.ToString(reader["username"]),
                            Password = Convert.ToString(reader["password"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            EMail = Convert.ToString(reader["email"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            switch (Convert.ToString(reader["pLanguage"]))
                            {
                                case "JAVA":
                                PLanguages
                                
                            }
                        });
                }


            } 
            return users;
        }

        public Profile GetUser(int userId)
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return null;
            }

            Profile users = new Profile();
            DbCommand cmdAllUsers = this._conn.CreateCommand();
            cmdAllUsers.CommandText = "select * from users";


            using (DbDataReader reader = cmdAllUsers.ExecuteReader())
            {

                users.UserId = Convert.ToInt32(reader["user_id"]);
                users.Username = Convert.ToString(reader["username"]);
                users.Password = Convert.ToString(reader["password"]);
                users.Birthdate = Convert.ToDateTime(reader["birthdate"]);
                users.EMail = Convert.ToString(reader["email"]);



            } 
            return users;
        }

        public bool Insert(Profile user)
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into users values(null, @username, sha2(@password, 512), @bDate, @email)";

            DbParameter paramUN = cmdInsert.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = System.Data.DbType.String;
            paramUN.Value = user.Username;

            DbParameter paramPWD = cmdInsert.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = System.Data.DbType.String;
            paramPWD.Value = user.Password;

            DbParameter paramBDate = cmdInsert.CreateParameter();
            paramBDate.ParameterName = "bDate";
            paramBDate.DbType = System.Data.DbType.Date;
            paramBDate.Value = user.Birthdate;

            DbParameter paramEMail = cmdInsert.CreateParameter();
            paramEMail.ParameterName = "email";
            paramEMail.DbType = System.Data.DbType.String;
            paramEMail.Value = user.EMail;


            cmdInsert.Parameters.Add(paramUN);
            cmdInsert.Parameters.Add(paramPWD);
            cmdInsert.Parameters.Add(paramBDate);
            cmdInsert.Parameters.Add(paramEMail);

            return cmdInsert.ExecuteNonQuery() == 1;
        }

        public bool Login(Profile newUserData)
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdLogin = this._conn.CreateCommand();

            cmdLogin.CommandText = "select * from users where username = @username and password = sha2(@password, 512)";

            DbParameter paramUN = cmdLogin.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = System.Data.DbType.String;
            paramUN.Value = newUserData.Username;

            DbParameter paramPWD = cmdLogin.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = System.Data.DbType.String;
            paramPWD.Value = newUserData.Password;

            cmdLogin.Parameters.Add(paramUN);
            cmdLogin.Parameters.Add(paramPWD);

            return cmdLogin.ExecuteNonQuery() == 1;
        }

        public bool Update(Profile newUserData)
        {
            if ((this._conn == null) && (this._conn.State != System.Data.ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdUpdate = this._conn.CreateCommand();

            cmdUpdate.CommandText = "update users set username = @username, password = @password,birthdate = @bDate, email = @email, gender = @gender where user_id = @userId";

            DbParameter paramUI = cmdUpdate.CreateParameter();

            paramUI.ParameterName = "userId";
            paramUI.DbType = System.Data.DbType.String;
            paramUI.Value = newUserData.UserId;

            DbParameter paramUN = cmdUpdate.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = System.Data.DbType.String;
            paramUN.Value = newUserData.Username;

            DbParameter paramPWD = cmdUpdate.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = System.Data.DbType.String;
            paramPWD.Value = newUserData.Password;

            DbParameter paramBDate = cmdUpdate.CreateParameter();
            paramBDate.ParameterName = "bDate";
            paramBDate.DbType = System.Data.DbType.Date;
            paramBDate.Value = newUserData.Birthdate;

            DbParameter paramEMail = cmdUpdate.CreateParameter();
            paramEMail.ParameterName = "email";
            paramEMail.DbType = System.Data.DbType.String;
            paramEMail.Value = newUserData.EMail;


            cmdUpdate.Parameters.Add(paramUI);
            cmdUpdate.Parameters.Add(paramUN);
            cmdUpdate.Parameters.Add(paramPWD);
            cmdUpdate.Parameters.Add(paramBDate);
            cmdUpdate.Parameters.Add(paramEMail);


            return cmdUpdate.ExecuteNonQuery() == 1;
        }
    }
}
