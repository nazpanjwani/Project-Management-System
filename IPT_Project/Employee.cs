using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace IPT_Project
{
    
    public partial class Employee : Form
    {
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        LoginPage lp = new LoginPage();
        public Employee()
        {
            InitializeComponent();
        }

        private void Employee_Load(object sender, EventArgs e)
        {
            ///login id-->user_id from users-->compare userid in task table

            SqlConnection conn = new SqlConnection(myconnstring);
            SqlDataAdapter sda0 = new SqlDataAdapter("SELECT * FROM Users WHERE User_id='" + LoginPage.id + "'", conn);
            DataTable dt0 = new DataTable();
            sda0.Fill(dt0);

            label4.Text = dt0.Rows[0]["User_id"].ToString();
            label5.Text = dt0.Rows[0]["Name"].ToString();
            label6.Text = dt0.Rows[0]["Email"].ToString();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Task WHERE user_id='" + dt0.Rows[0]["User_id"] + "'", conn);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);
            /*
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT project_name FROM Projects WHERE id='" + dt1.Rows[i]["project_id"] + "'", conn);
                DataTable dt = new DataTable();
                sda1.Fill(dt);
                listView1.SelectedItems[0].Text = dt1.Rows[i]["title"].ToString();
                listView1.SelectedItems[0].SubItems[1].Text = dt.Rows[0][0].ToString();
                listView1.SelectedItems[0].SubItems[2].Text = dt1.Rows[i]["createdAt"].ToString();
                listView1.SelectedItems[0].SubItems[3].Text = dt1.Rows[i]["dueDate"].ToString();
                listView1.SelectedItems[0].SubItems[4].Text = dt1.Rows[i]["description"].ToString();
                listView1.SelectedItems[0].SubItems[5].Text = dt1.Rows[i]["status"].ToString();
            }*/

            
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT project_name FROM Projects WHERE id='" + dt1.Rows[i]["project_id"] + "'", conn);
                DataTable dt = new DataTable();
                sda1.Fill(dt);
                DataRow dr = dt1.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["title"].ToString());
                listitem.SubItems.Add(dt.Rows[0][0].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["createdAt"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["dueDate"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["description"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["status"].ToString());
                listView1.Items.Add(listitem);
            }

            comboBox1.Items.Add("Pending");
            comboBox1.Items.Add("Complete");
            comboBox1.Items.Add("InProcess");

        }

        private void label4_Click(object sender, EventArgs e)
        {
             
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("Select a task to change status!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                //SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Task WHERE title='" + listView1.SelectedItems[0].Text + "' ", conn);
                SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Task WHERE title='" + listView1.SelectedItems[0].Text + "' ", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                //using(var contents=new myconnstring()) { }
                //var li = from Task in ProjectsDb where Task.title== listView1.SelectedItems[0].Text select IDataAdapter;
                string sql = "UPDATE Task SET status=@status WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@status", comboBox1.Text);
                cmd.Parameters.AddWithValue("@id", dt.Rows[0]["id"]);
                conn.Open();
                cmd.ExecuteNonQuery();
                listView1.SelectedItems[0].SubItems[5].Text = comboBox1.Text;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage lp = new LoginPage();
            lp.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            All_Tasks at = new All_Tasks();
            at.Show();
        }
    }
}
