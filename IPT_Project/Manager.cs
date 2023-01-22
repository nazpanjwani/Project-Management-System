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
    public partial class Manager : Form
    {
        LoginPage lp = new LoginPage();
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        public Manager()
        {
            InitializeComponent();
            //label7.Text = LoginPage.id;
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlDataAdapter sda0 = new SqlDataAdapter("SELECT * FROM Managers WHERE id='" + LoginPage.id + "'", conn);
            DataTable dt0 = new DataTable();
            sda0.Fill(dt0);
            comboBox1.Items.Add(dt0.Rows[0]["id"].ToString() + "-" + dt0.Rows[0]["Name"].ToString());

            
            /*SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM Managers", conn);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt0);
            for (int i = 0; i < dt0.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt0.Rows[i]["id"].ToString() + "-" + dt0.Rows[i]["Name"].ToString());
            }*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Projects where manager_id='" + LoginPage.id + "'", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                //string sql = "DELETE FROM Task WHERE project_id=@project_id";
                //SqlCommand cmd = new SqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("@project_id", dt.Rows[0]["id"].ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ListViewItem listitem = new ListViewItem(dr["project_name"].ToString());
                    listitem.SubItems.Add(dr["start_date"].ToString());
                    listitem.SubItems.Add(dr["end_date"].ToString());
                    listView1.Items.Add(listitem);
                }
            }
            catch
            {
            }
        }
            

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox1.Text = listView1.SelectedItems[0].Text;
            //comboBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].Text;
                //comboBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Field Missing. Please fill all data!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                try
                {
                    conn.Open();
                    string[] arr = new string[4];
                    arr[0] = textBox1.Text;
                    arr[1] = dateTimePicker1.Text;
                    arr[2] = dateTimePicker2.Text;
                    arr[3] = comboBox1.Text;
                    string[] val = arr[3].Split('-');
                    ListViewItem lv = new ListViewItem(arr);
                    listView1.Items.Add(lv);
                    string sql = "INSERT INTO Projects (project_name, start_date, end_date, manager_id) VALUES (@project_name, @start_date, @end_date, @manager_id)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@project_name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@start_date", dateTimePicker1.Text);
                    cmd.Parameters.AddWithValue("@end_date", dateTimePicker2.Text);
                    cmd.Parameters.AddWithValue("@manager_id", val[0]);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*SqlConnection conn = new SqlConnection(myconnstring);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT m_id FROM Manager", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["m_id"]);
            }*/
            

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage lp = new LoginPage();
            lp.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("First select a row to delete!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                try
                {
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Projects WHERE project_name='" + listView1.SelectedItems[0].Text + "' ", conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string sql = "DELETE FROM Projects WHERE id=@id";//; + listView1.SelectedItems[0].Text + "' ";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //cmd.Parameters.AddWithValue("@project_id", dt.Rows[0]["id"].ToString());
                    cmd.Parameters.AddWithValue("@id", dt.Rows[0]["id"].ToString());
                    cmd.ExecuteNonQuery();
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        listView1.Items.Remove(item);
                    }
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            
            if (listView1.SelectedItems.Count < 1)
            {
                 MessageBox.Show("First select a row to delete!");
            }
            else
            {
                if (textBox1.Text == "" || comboBox1.Text == "")
                {
                    MessageBox.Show("Field Missing. Please fill all data!");
                }
                else
                {
                    string tmp = listView1.SelectedItems[0].Text;
                    listView1.SelectedItems[0].Text = textBox1.Text;
                    //listView1.SelectedItems[0].SubItems[1].Text = comboBox1.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = dateTimePicker1.Text;
                    listView1.SelectedItems[0].SubItems[2].Text = dateTimePicker2.Text;
                    //listView1.SelectedItems[0].SubItems[4].Text = comboBox2.Text;
                    //listView1.SelectedItems[0].SubItems[5].Text = textBox3.Text;

                    string[] val = comboBox1.Text.Split('-');
                    SqlConnection conn = new SqlConnection(myconnstring);
                    try
                    {
                        conn.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Projects WHERE project_name='" + tmp + "' ", conn);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        //label7.Text = tmp;
                        //button4.Text = dt.Rows[0][0].ToString();
                        string sql = "UPDATE Projects SET project_name=@project_name, start_date=@start_date, end_date=@end_date, manager_id=@manager_id WHERE id='" + dt.Rows[0][0] + "'";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@project_name", listView1.SelectedItems[0].Text);
                        cmd.Parameters.AddWithValue("@start_date", listView1.SelectedItems[0].SubItems[1].Text);
                        cmd.Parameters.AddWithValue("@end_date", listView1.SelectedItems[0].SubItems[2].Text);
                        cmd.Parameters.AddWithValue("@manager_id", val[0]);

                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tasks tt = new Tasks();
            tt.Show();
        }
    }
}
