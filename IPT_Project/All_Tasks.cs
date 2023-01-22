using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IPT_Project
{
    public partial class All_Tasks : Form
    {
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        public All_Tasks()
        {
            InitializeComponent();
        }

        private void All_Tasks_Load(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection(myconnstring);
            /*SqlDataAdapter sda0 = new SqlDataAdapter("SELECT * FROM Users WHERE User_id='" + LoginPage.id + "'", conn);
            DataTable dt0 = new DataTable();
            sda0.Fill(dt0);*/

            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Task", conn);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT project_name FROM Projects WHERE id='" + dt1.Rows[i]["project_id"] + "'", conn);
                DataTable dt = new DataTable();
                sda1.Fill(dt);
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT Name FROM Users WHERE User_id='" + dt1.Rows[i]["user_id"] + "'", conn);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                DataRow dr = dt1.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["title"].ToString());
                listitem.SubItems.Add(dt.Rows[0][0].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["createdAt"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["dueDate"].ToString());
                listitem.SubItems.Add(dt2.Rows[0]["Name"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["description"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["status"].ToString());
                listView1.Items.Add(listitem);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Employee ee = new Employee();
            ee.Show();
        }
    }
}
