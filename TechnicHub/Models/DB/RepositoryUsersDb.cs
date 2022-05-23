using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using TechnicHub.Models;
using Microsoft.AspNetCore.Http;


namespace TechnicHub.Models.DB
{
    // diese Klasse implementiert unser Interface
    public class RepositoryUsersDB : IRepositoryUsers
    {
        private string _connString = "Server=localhost;database=technichub;user=root;password=admin";

        private DbConnection _conn;
        public async Task ConnectAsync()
        {

            if (this._conn == null)
            {
                this._conn = new MySqlConnection(this._connString);
            }
            if (this._conn.State != System.Data.ConnectionState.Open)
            {

                await this._conn.OpenAsync();
            }
        }
        public async Task DisconnectAsync()
        {
            if (this._conn != null && this._conn.State == ConnectionState.Open)
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
            return res == 1;


        }
        public async Task<List<Profile>> GetAllUsersAsync()
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
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
            cmdGet.CommandText = "select * from users where user_id = @id";
            
            DbParameter paramId = cmdGet.CreateParameter();
            paramId.ParameterName = "id";
            paramId.DbType = DbType.Int32;
            paramId.Value = userId;

            cmdGet.Parameters.Add(paramId);

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
        public async Task<List<string>> GetLanguagesAsync(int userId)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return null;
            }

            List<String> plang = new List<String>();
            DbCommand cmdAllUsers = this._conn.CreateCommand();
            cmdAllUsers.CommandText = "SELECT plang_name from planguage, users, zwtable where users.user_id = zwtable.user_id and zwtable.plang_id = planguage.plang_id and users.user_id = @userId ";

            DbParameter paramId = cmdAllUsers.CreateParameter();
            paramId.ParameterName = "userId";
            paramId.DbType = DbType.Int16;
            paramId.Value = userId;

            cmdAllUsers.Parameters.Add(paramId);

            using (DbDataReader reader = await cmdAllUsers.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    plang.Add(reader["Plang_name"].ToString());
                }
            }
            return plang;
        }
        public async Task<List<String>> GetAllPLanguagesAsync()
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return null;
            }

            List<String> plang = new List<String>();
            DbCommand cmdAllUsers = this._conn.CreateCommand();
            cmdAllUsers.CommandText = "select * from planguage";

            using (DbDataReader reader = await cmdAllUsers.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    plang.Add(reader["Plang_name"].ToString());
                }
            }
            return plang;
        }
        public async Task<bool> InsertUserAsync(Profile user)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }
            DbCommand cmdInsert = this._conn.CreateCommand();
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

            DbParameter paramGender = cmdInsert.CreateParameter();
            paramGender.ParameterName = "gender";
            paramGender.DbType = DbType.Int32;
            paramGender.Value = user.Gender;

            cmdInsert.Parameters.Add(paramUN);
            cmdInsert.Parameters.Add(paramPWD);
            cmdInsert.Parameters.Add(paramBD);
            cmdInsert.Parameters.Add(paramEmail);
            cmdInsert.Parameters.Add(paramGender);

            return await cmdInsert.ExecuteNonQueryAsync() == 1;
        }
        public async Task<bool> InsertLanguagesAsync(List<string> language, int user_id)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            bool result = false;
            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into planguage values(@user_id, @language_id)";

            DbParameter paramUserId = cmdInsert.CreateParameter();
            paramUserId.ParameterName = "user_id";
            paramUserId.DbType = DbType.Int16;
            paramUserId.Value = user_id;

            DbParameter paramLanguageId = cmdInsert.CreateParameter();
            paramLanguageId.ParameterName = "language_id";
            paramLanguageId.DbType = DbType.String;

            foreach (string p in language)
            {
                paramLanguageId.Value = p;

                cmdInsert.Parameters.Add(paramUserId);
                cmdInsert.Parameters.Add(paramLanguageId);
                result = await cmdInsert.ExecuteNonQueryAsync() == 1;
            }

            return result;
        }
        public async Task<int> GetLanguageIdAsync(string Plang_name)
        {
            DbCommand cmdGet = this._conn.CreateCommand();
            cmdGet.CommandText = "select Plang_id from planguage where Plang_name = @Plang_name";
            DbParameter paramUN = cmdGet.CreateParameter();
            paramUN.ParameterName = "Plang_name";
            paramUN.DbType = DbType.String;
            paramUN.Value = Plang_name;

            cmdGet.Parameters.Add(paramUN);

            int lang_id = 0;
            using (DbDataReader reader = await cmdGet.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    lang_id = Convert.ToInt32(reader["Plang_id"]);
                }
            }
            return lang_id;
        }
        public async Task<bool> InsertZwAsync(int lang_id, int user_id)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }


            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into zwtable values(@user_id, @Plang_id)";

            DbParameter paramUserId = cmdInsert.CreateParameter();
            paramUserId.ParameterName = "user_id";
            paramUserId.DbType = DbType.Int16;
            paramUserId.Value = user_id;

            DbParameter paramLanguageId = cmdInsert.CreateParameter();
            paramLanguageId.ParameterName = "Plang_id";
            paramLanguageId.DbType = DbType.Int16;
            paramLanguageId.Value = lang_id;

            cmdInsert.Parameters.Add(paramUserId);
            cmdInsert.Parameters.Add(paramLanguageId);


            return await cmdInsert.ExecuteNonQueryAsync() == 1;
        }
        public async Task<bool> InsertAsync(ProfileAndLanguages pl)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            bool user = await InsertUserAsync(pl.Profile);

            if (user)
            {
                bool zw = false;
                int user_id = await GetUserIdAsync(pl.Profile.Username);

                foreach (string s in pl.Languages) {
                    int lang_id = await GetLanguageIdAsync(s); // Da kömma jetzt doch s sagen statt pl.languages 

                    Console.WriteLine(pl.Languages.ToString());
                    zw = await InsertZwAsync(lang_id, user_id);
                }
                return zw;
            }
            else
            {
                return false;
            }
        }
        public async Task<int> GetUserIdAsync(string username)
        {
            DbCommand cmdGet = this._conn.CreateCommand();
            cmdGet.CommandText = "select user_id from users where username = @username";
            DbParameter paramUN = cmdGet.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = DbType.String;
            paramUN.Value = username;

            cmdGet.Parameters.Add(paramUN);

            int user_id = 0;
            using (DbDataReader reader = await cmdGet.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    user_id = Convert.ToInt32(reader["user_id"]);
                }
            }
            return user_id;
        }
        public async Task<bool> LoginAsync(Profile p)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "select user_id from users where username = @username and password = sha2(@password,512)";

            DbParameter paramUN = cmdInsert.CreateParameter();
            paramUN.ParameterName = "username";
            paramUN.DbType = DbType.String;
            paramUN.Value = p.Username;

            DbParameter paramPWD = cmdInsert.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.DbType = DbType.String;
            paramPWD.Value = p.Password;

            cmdInsert.Parameters.Add(paramUN);
            cmdInsert.Parameters.Add(paramPWD);


            return ((Int32)await cmdInsert.ExecuteScalarAsync()) != 0;
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
        public async Task<bool> InsertPostAsync(Chatpost post, int UserId)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            post.ChatpostId = 1; //Nicht Final nur um die anderen Systeme zu testen

            DbCommand cmdInsert = this._conn.CreateCommand();
            
            cmdInsert.CommandText = "insert into posts values(null, @Message, @Post_date, @UserId, @ChatroomId)";

            DbParameter paramMessage = cmdInsert.CreateParameter();
            paramMessage.ParameterName = "Message";
            paramMessage.DbType = DbType.String;
            paramMessage.Value = post.ChatpostMessage;

            DbParameter paramDate = cmdInsert.CreateParameter();
            paramDate.ParameterName = "Post_date";
            paramDate.DbType = DbType.DateTime;
            paramDate.Value = DateTime.Now;

            DbParameter paramUser = cmdInsert.CreateParameter();
            paramUser.ParameterName = "UserId";
            paramUser.DbType = DbType.Int32;
            paramUser.Value = UserId;

            DbParameter paramChatroomId = cmdInsert.CreateParameter();
            paramChatroomId.ParameterName = "ChatroomId";
            paramChatroomId.DbType = DbType.Int32;
            paramChatroomId.Value = post.ChatroomId;

            cmdInsert.Parameters.Add(paramMessage);
            cmdInsert.Parameters.Add(paramDate);
            cmdInsert.Parameters.Add(paramUser);
            cmdInsert.Parameters.Add(paramChatroomId);

            return await cmdInsert.ExecuteNonQueryAsync() == 1;

        }
        public async Task<bool> InsertRoomAsync(Chatroom room)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdInsert = this._conn.CreateCommand();
            cmdInsert.CommandText = "insert into chatroom values(null, @Name, @Description)";

            DbParameter paramName = cmdInsert.CreateParameter();
            paramName.ParameterName = "Name";
            paramName.DbType = DbType.String;
            paramName.Value = room.Name;

            DbParameter paramDescription = cmdInsert.CreateParameter();
            paramDescription.ParameterName = "Description";
            paramDescription.DbType = DbType.String;
            paramDescription.Value = room.Description;

            cmdInsert.Parameters.Add(paramName);
            cmdInsert.Parameters.Add(paramDescription);

            return await cmdInsert.ExecuteNonQueryAsync() == 1;
        }
        public async Task<int> GetMessageCountAsync(int id)
        {
            int MsgCount = 0;

            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return MsgCount;
            }

            DbCommand cmdSelect = this._conn.CreateCommand();
            cmdSelect.CommandText = "select count(*) from posts where chatroomID = @chatroom_id";

            DbParameter paramChatroomId = cmdSelect.CreateParameter();
            paramChatroomId.ParameterName = "chatroom_id";
            paramChatroomId.DbType = DbType.Int32;
            paramChatroomId.Value = id;

            cmdSelect.Parameters.Add(paramChatroomId);

            MsgCount = (int)(long)(await cmdSelect.ExecuteScalarAsync());

            return MsgCount;

        }
        public async Task<List<Chatpost>> GetPostsAsync(int id)
        {
            List<Chatpost> MsgList = new List<Chatpost>();

            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return MsgList;
            }
            int ChatpostUserId = 0;
            Profile user = new Profile();

            DbCommand cmdSelect = this._conn.CreateCommand();
            cmdSelect.CommandText = "select * from posts where chatroomID = @chatroom_id";

            DbParameter paramChatroomId = cmdSelect.CreateParameter();
            paramChatroomId.ParameterName = "chatroom_id";
            paramChatroomId.DbType = DbType.Int32;
            paramChatroomId.Value = id;

            cmdSelect.Parameters.Add(paramChatroomId);

            DbDataReader reader = await cmdSelect.ExecuteReaderAsync();

            while (reader.Read())
            {
                Chatpost msg = new Chatpost();
                msg.ChatpostId = (int)reader["PostId"];
                msg.ChatpostMessage = (string)reader["Message"];
                msg.ChatpostDate = (DateTime)reader["Post_date"];
                ChatpostUserId = (int)reader["UserId"];
                user = await GetUserAsync(ChatpostUserId);
                msg.ChatpostUser = user.Username;
                msg.ChatroomId = (int)reader["ChatroomId"];

                MsgList.Add(msg);
            }

            return MsgList;
        }
        public async Task<List<Chatroom>> GetRoomsAsync()
        {
            List<Chatroom> RoomList = new List<Chatroom>();

            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return RoomList;
            }

            DbCommand cmdSelect = this._conn.CreateCommand();
            cmdSelect.CommandText = "select * from chatroom";

            DbDataReader reader = await cmdSelect.ExecuteReaderAsync();

            while (reader.Read())
            {
                Chatroom room = new Chatroom();
                room.Id = (int)reader["Id"];
                room.Name = (string)reader["name"];
                room.Description = (string)reader["description"];

                RoomList.Add(room);
            }

            await DisconnectAsync();
            
            await ConnectAsync();

            for (int i = 0; i < RoomList.Count; i++)
            {
                RoomList[i].MessageCount = (int)(await GetMessageCountAsync(RoomList[i].Id));
            }



            return RoomList;
        }
        public async Task<bool> DeletePostAsync(int id)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdDelete = this._conn.CreateCommand();
            cmdDelete.CommandText = "delete from posts where PostId = @post_id";

            DbParameter paramPostId = cmdDelete.CreateParameter();
            paramPostId.ParameterName = "post_id";
            paramPostId.DbType = DbType.Int32;
            paramPostId.Value = id;

            cmdDelete.Parameters.Add(paramPostId);

            return await cmdDelete.ExecuteNonQueryAsync() == 1;
        }
        public async Task<bool> DeleteRoomAsync(int id)
        {
            if ((this._conn == null) && (this._conn.State != ConnectionState.Open))
            {
                return false;
            }

            DbCommand cmdDelete = this._conn.CreateCommand();
            cmdDelete.CommandText = "delete from chatroom where Id = @chatroom_id";

            DbParameter paramChatroomId = cmdDelete.CreateParameter();
            paramChatroomId.ParameterName = "chatroom_id";
            paramChatroomId.DbType = DbType.Int32;
            paramChatroomId.Value = id;

            cmdDelete.Parameters.Add(paramChatroomId);

            return await cmdDelete.ExecuteNonQueryAsync() == 1;
        }

    }
}