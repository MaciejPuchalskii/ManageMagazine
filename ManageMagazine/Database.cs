using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ManageMagazine
{
    public class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=database.sqlite3");


            if (!File.Exists("database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
            }

        }
        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }
        public void CloseConnection()
        {
            if (myConnection.State == System.Data.ConnectionState.Open)
            {
                myConnection.Close();
            }
        }


        public int GetLastId(string tablename)
        {

            string query = "SELECT ID FROM \"" + tablename + "\" ORDER BY ID DESC LIMIT 1;";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);

            SQLiteDataReader result = command.ExecuteReader();
            int id = 0;
            if (result.HasRows)
            {
                result.Read();
                id = Convert.ToInt32(result["ID"]);
            }

            return id;
        }
    }
}
