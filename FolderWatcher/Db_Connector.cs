using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;

namespace FolderWatcher
{
    public class Db_Connector
    {
        public SqliteConnection sqlite_conn { get; set; }
        public Db_Connector()
        {
        
            sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);
           
        }

        public SqliteConnection CreateConnection()
        {


            SqliteConnection sqlite_conn;
           // sqlite_conn.cre
            // Create a new database connection:
            sqlite_conn = new SqliteConnection("Data Source= SQLITE.db;  ");

            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        public void CreateTable(SqliteConnection conn)
        {

            SqliteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE logs  (State VARCHAR(225),FilePath VARCHAR(225), Data VARCHAR(225))";

           // string Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            try
            {
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {  }
           
           // sqlite_cmd.CommandText = Createsql1;
           // sqlite_cmd.ExecuteNonQuery();

        }

        public async void  InsertData(string state, string path, string data)
        {
            SqliteCommand sqlite_cmd;
            //sqlite_cmd = conn.CreateCommand();
            sqlite_cmd = this.sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = $"INSERT INTO logs  (State, FilePath, Data) VALUES('{state}', '{path}','{data}'); "; //VALUES('Test Text ', 1); 
            await sqlite_cmd.ExecuteNonQueryAsync(); //sqlite_cmd.ExecuteNonQuery();
        }

        public void ReadData()
        {
            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            sqlite_cmd = this.sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM logs";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                string myreader1 = sqlite_datareader.GetString(1);
                string myreader2 = sqlite_datareader.GetString(2);
                Console.WriteLine($"{myreader}  {myreader1} {myreader2}");
            }
            this.sqlite_conn.Close();
        }

    }
}
