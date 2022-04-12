using AltV.Net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltV
{
    class Database : Server
    {
        public static bool DatabaseConnection = false;
        public static MySqlConnection Connection;
        public string Host { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String DataB { get; set; }
        public Database ()
        {
            this.Host = "localhost";
            this.Username = "root";
            this.Password = "";
            this.DataB = "altv";
        }
        public static String GetConnectionString()
        {
            Database sql = new Database();
            String SQLConnection = $"SERVER={sql.Host}; DATABASE={sql.DataB}; UID={sql.Username}; Password={sql.Password}";
            return SQLConnection;
        }

        public static void InitConnection()
        {
            String SQLConnection = GetConnectionString();
            Connection = new MySqlConnection(SQLConnection);
            try
            {
                Connection.Open();
                DatabaseConnection = true;
                Alt.Log("MySQL baglantisi basarili!");
            } catch(Exception e)
            {
                DatabaseConnection = false;
                Alt.Log("MySQL baglantisi basarisiz!");
                Alt.Log(e.ToString());
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static bool DoesAccountAlreadyExists(string name)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);
            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CreateNewAccount(String name, string password)
        {
            string saltedPw = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            try
            {
                MySqlCommand command = Connection.CreateCommand();
                command.CommandText = "INSERT INTO accounts (password, name) VALUES (@password, @name)";

                command.Parameters.AddWithValue("@password", saltedPw);
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();

                return (int)command.LastInsertedId;
            }
            catch(Exception e)
            {
                Alt.Log("Hesap oluşturmada hata: " + e.ToString());
                return -1;
            }
        }

        public static void LoadAccount(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";

            command.Parameters.AddWithValue("@name", tplayer.PlayerName);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    tplayer.PlayerID = reader.GetInt32("id");
                    tplayer.AdminLevel = reader.GetInt16("adminlevel");
                    tplayer.Money = reader.GetInt32("money");
                    tplayer.Payday = reader.GetInt16("payday");

                }
            }
        }

        public static void SaveAccount(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE accounts SET adminlevel=@adminlevel, money=@money, payday=@payday WHERE id=@id";

            command.Parameters.AddWithValue("@adminlevel", tplayer.AdminLevel);
            command.Parameters.AddWithValue("@money", tplayer.Money);
            command.Parameters.AddWithValue("@payday", tplayer.Payday);
            command.Parameters.AddWithValue("id", tplayer.PlayerID);
        }

        public static bool PasswordCheck(string name, string passwordinput)
        {
            string password = "";
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT password FROM accounts where name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    password = reader.GetString("password");
                }
            }

            if (BCrypt.CheckPassword(passwordinput, password)) return true;
            return false;
        }
    }
}
