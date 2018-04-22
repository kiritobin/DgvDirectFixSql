using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        SqlDataAdapter sda;
        private void Form1_Load(object sender, EventArgs e)
        {
            string testDB = ConfigurationManager.ConnectionStrings["testDB"].ConnectionString;
            conn = new SqlConnection(testDB);
            sda = new SqlDataAdapter("select * from T_StuInfo",conn);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            for (int i=0;i<dataGridView1.ColumnCount;i++)
            {
                dataGridView1.Columns[i].Width = 84;
            }
        }
        private DataTable dbconn(string strSql)
        {
            conn.Open();
            sda = new SqlDataAdapter(strSql,conn);
            DataTable stSelect = new DataTable();
            int rnt = sda.Fill(stSelect);
            conn.Close();
            return stSelect;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dbUpdate())
            {
                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }
        private Boolean dbUpdate()
        {
            string strSql = "select * from T_StuInfo";
            DataTable stUpdate = new DataTable();
            stUpdate = this.dbconn(strSql);
            stUpdate.Rows.Clear();
            DataTable dtShow = new DataTable();
            dtShow = (DataTable)dataGridView1.DataSource;
            for (int i =0;i<dtShow.Rows.Count;i++)
            {
                stUpdate.ImportRow(dtShow.Rows[i]);
            }
            try
            {
                conn.Open();
                sda = new SqlDataAdapter("select * from T_StuInfo",conn);
                DataTable ds = new DataTable();
                SqlCommandBuilder CommandBuilder = new SqlCommandBuilder(sda);
                sda.UpdateCommand = CommandBuilder.GetUpdateCommand();
                ds = (DataTable)dataGridView1.DataSource;
                sda.Update(ds);
                stUpdate.AcceptChanges();
                conn.Close();
            }
            catch
            {
                return false;
            }
            stUpdate.AcceptChanges();
            return true;
        }
    }
}
