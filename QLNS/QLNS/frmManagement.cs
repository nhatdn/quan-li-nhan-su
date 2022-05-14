using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNS
{
    public partial class frmManagement : Form
    {
        private SQLiteConnection con;
        public frmManagement()
        {
            InitializeComponent();
            this.con = new SQLiteConnection();
        }
       
        private void frmManagement_Load(object sender, EventArgs e)
        {
           
        }
        public void createConection()
        {
            string strConnect = string.Format(@"Data Source={0}\db\mydb.db;Version=3;", Application.StartupPath); 
            this.con.ConnectionString = strConnect;
            this.con.Open();
        }
        public void closeConnection()
        {
            this.con.Close();
        }

        public void loadData()
        {
            DataSet ds = new DataSet();
            createConection();
            SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * FROM chucvu", this.con);
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Console.WriteLine("OK");
            closeConnection();
        }
    }
}
