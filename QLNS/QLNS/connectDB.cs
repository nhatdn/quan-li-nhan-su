using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNS
{
    class connectDB
    {
        public string pathApp;
        private SQLiteConnection con;
        public connectDB(string path)
        {
            this.pathApp = path;
            this.con = new SQLiteConnection(string.Format(@"Data Source={0}\db\mydb.db;Version=3;", pathApp));
        }
        private void createConection()
        {
            this.con.Open();
            
        }
        private void closeConnection()
        {
            this.con.Close();
        }
        public void InsertUsers(string query)
        {
            using (con)
            {
                createConection();
                //SQLiteDataAdapter da = new SQLiteDataAdapter(query, this.con);
                SQLiteCommand command = new SQLiteCommand(query, this.con);
                command.ExecuteNonQuery();
                closeConnection();
            }
         }
        public DataTable SelectUsers(string query)
        {
            using (con)
            {
                DataTable ds = new DataTable();
                createConection();
                var cmd = new SQLiteCommand(query, con);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                ds.Load(rdr);
                closeConnection();
                return ds;
            }
        }
        
    }
}
