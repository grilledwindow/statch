using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using Statch.Models;

namespace Statch.DAL
{
    public class UsersDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UsersDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "StatchConnectionString");
            conn = new SqlConnection(strConn);
        }

        public List<Users> GetAllUsers()
        {

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Users ORDER BY UserID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            List<Users> userList = new List<Users>();
            while (reader.Read())
            {
                userList.Add(
                new Users
                {
                    UserID = reader.GetInt32(0), //0: 1st column
                    UserName = reader.GetString(1),
                    Salutation = reader.GetString(2),
                    EmailAddr = reader.GetString(3),
                    Password = reader.GetString(4),
                    DateCreated = reader.GetDateTime(5),
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return userList;
        }

        public bool IsUserExist(string email, int userId) //Create new validation (True means email already used)
        {
            bool emailFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT UserID FROM Users WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records Found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != userId)
                    {
                        emailFound = true;
                    }
                }
            }
            else
            {
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }
        public bool ValidUserLogin(string email, string pass)
        {
            bool validLogin = false;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT UserID FROM Users WHERE EmailAddr=@email AND Password=@pass";
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", pass);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                validLogin = true;
            }
            else
            {
                validLogin = false;
            }
            reader.Close();
            conn.Close();

            return validLogin;
        }
        public int Add(Users user)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Users (UserName, Salutation, EmailAddr, Password, Score, Badges, DateCreated)
                                OUTPUT INSERTED.UserID
                                VALUES(@name, @salutation, @email, @password, '0', '0', @date)";

            cmd.Parameters.AddWithValue("@name", user.UserName);
            cmd.Parameters.AddWithValue("@salutation", user.Salutation);
            cmd.Parameters.AddWithValue("@email", user.EmailAddr);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));

            conn.Open();
            user.UserID = (int)cmd.ExecuteScalar();
            conn.Close();

            return user.UserID;
        }
        public int GetUserID(string email)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT UserID FROM Users WHERE EmailAddr = @email";

            cmd.Parameters.AddWithValue("@email", email);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            int userID = 0;
            while (reader.Read())
            {
                userID = reader.GetInt32(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return userID;
        }
        public Users GetDetails(int UserId)
        {
            Users user = new Users();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Users                       
                                WHERE UserID = @selectedUserID";

            cmd.Parameters.AddWithValue("@selectedUserID", UserId);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user.UserID = UserId;
                    user.UserName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    user.Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    user.EmailAddr = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    user.Password = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    user.DateCreated = reader.GetDateTime(5);
                }
            }
            reader.Close();
            conn.Close();

            return user;
        }
        public string GetUserName(string userId)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT UserName FROM Users WHERE UserID = @userId";

            cmd.Parameters.AddWithValue("@userId", userId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            string userName = "";
            while (reader.Read())
            {
                userName = reader.GetString(0);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return userName;
        }

        public int AddGamePoint(string userID, int point)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"Update Users
                                Set Score = @selectedScore+Score
                                WHERE UserID = @selectedUserID";

            cmd.Parameters.AddWithValue("@selectedScore", point);
            cmd.Parameters.AddWithValue("@selectedUserID", userID);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return Convert.ToInt32(userID);
        }

        public int Update(Users user)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Users SET UserName = @name,
                                Salutation = @sal
                                WHERE UserID = @userID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", user.UserName);
            cmd.Parameters.AddWithValue("@sal", user.Salutation);
            cmd.Parameters.AddWithValue("@userID", user.UserID);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }
    }
}
