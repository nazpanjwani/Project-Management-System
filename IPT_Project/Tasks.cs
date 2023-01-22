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
    public partial class Tasks : Form
    {
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        
        public Tasks()
        {
            InitializeComponent();
            
            SqlConnection conn = new SqlConnection(myconnstring);
            //SqlDataAdapter sda0 = new SqlDataAdapter("SELECT id FROM Managers WHERE login_id='"+ LoginPage.id+"'", conn);
            //DataTable dt0 = new DataTable();
            //sda0.Fill(dt0);

            //SqlDataAdapter sda = new SqlDataAdapter("SELECT project_name FROM Projects WHERE manager_id='"+dt0.Rows[0][0]+"'", conn);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT id, project_name FROM Projects WHERE manager_id='"+ LoginPage.id + "'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["id"].ToString()+"-"+dt.Rows[i]["project_name"]);
            }

            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT User_id, Name FROM Users WHERE manager_id = '" + LoginPage.id + "'", conn);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            try
            {
                conn.Open();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string opt = dt1.Rows[i]["User_id"].ToString()+"-"+dt1.Rows[i]["Name"].ToString();
                    comboBox2.Items.Add(opt);
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
        private void Tasks_Load(object sender, EventArgs e)
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
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM Projects WHERE id='" + dt1.Rows[i]["project_id"] + "'", conn);
                DataTable dt = new DataTable();
                sda1.Fill(dt);
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM Users WHERE User_id='" + dt1.Rows[i]["user_id"] + "'", conn);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                DataRow dr = dt1.Rows[i];
                ListViewItem listitem = new ListViewItem(dr["title"].ToString());
                listitem.SubItems.Add(dt.Rows[0]["project_name"].ToString());
                //listitem.SubItems.Add(dt.Rows[i]["id"].ToString() + "-" + dt.Rows[i]["project_name"]);

                listitem.SubItems.Add(dt1.Rows[i]["createdAt"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["dueDate"].ToString());
                //string opt = 
                //listitem.SubItems.Add(dt2.Rows[i]["User_id"].ToString() + "-" + dt2.Rows[i]["Name"].ToString());

                listitem.SubItems.Add(dt2.Rows[0]["Name"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["description"].ToString());
                listitem.SubItems.Add(dt1.Rows[i]["status"].ToString());
                listView1.Items.Add(listitem);
            }

           
        }
        void txtClear()
        {
            // comboBox1.Clear();
           // dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm tt";
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now.Date;


        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Field Missing. Please fill all data!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                try
                {

                    conn.Open();
                    string[] arr = new string[7];
                    string[] val0 = comboBox1.Text.Split('-');
                    string[] val = comboBox2.Text.Split('-');
                    arr[0] = textBox1.Text;
                    arr[1] = val0[1];
                    arr[2] = dateTimePicker1.Text;
                    arr[3] = dateTimePicker2.Text;
                    arr[4] = val[1];
                    arr[5] = textBox3.Text;
                    arr[6] = "Pending";

                    //arr[5]=
                    //SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Projects WHERE project_name='" + comboBox1.Text + "' ", conn);
                    //DataTable dt = new DataTable();
                    //sda.Fill(dt);
                    ListViewItem lv = new ListViewItem(arr);
                    listView1.Items.Add(lv);
                    string sql = "INSERT INTO Task (title, description, status, createdAt, dueDate, project_id, user_id) VALUES (@title, @description, @status, @createdAt, @dueDate, @project_id, @user_id)";
                    //string[] val0 = arr[1].Split('-');
                    //string[] val = arr[4].Split('-');
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@title", textBox1.Text);
                    cmd.Parameters.AddWithValue("@description", textBox3.Text);
                    cmd.Parameters.AddWithValue("@status", "Pending");
                    cmd.Parameters.AddWithValue("@createdAt", dateTimePicker1.Text);
                    cmd.Parameters.AddWithValue("@dueDate", dateTimePicker2.Text);
                    cmd.Parameters.AddWithValue("@project_id", val0[0]);
                    cmd.Parameters.AddWithValue("@user_id", val[0]);

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
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("First select a row to delete!");
            }
            else
            {
                if (textBox1.Text == "" || comboBox1.Text == "" || comboBox2.Text=="")
                {
                    MessageBox.Show("Field Missing. Please fill all data!");
                }
                else
                {
                    string[] val0 = comboBox1.Text.Split('-');
                    string[] val = comboBox2.Text.Split('-');
                    string tmp = listView1.SelectedItems[0].Text;
                    listView1.SelectedItems[0].Text = textBox1.Text;
                    listView1.SelectedItems[0].SubItems[1].Text = val0[1];
                    listView1.SelectedItems[0].SubItems[2].Text = dateTimePicker1.Text;
                    listView1.SelectedItems[0].SubItems[3].Text = dateTimePicker2.Text;
                    listView1.SelectedItems[0].SubItems[4].Text = comboBox2.Text;
                    listView1.SelectedItems[0].SubItems[5].Text = textBox3.Text;



                    SqlConnection conn = new SqlConnection(myconnstring);
                    try
                    {
                        conn.Open();
                        //SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Projects WHERE project_name='"+ listView1.SelectedItems[0].SubItems[1].Text + "' ", conn);
                        SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Task WHERE title='" + tmp + "' ", conn);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        string sql = "UPDATE Task SET title=@title, description=@description, createdAt=@createdAt, dueDate=@dueDate, user_id=@user_id ,project_id=@project_id WHERE id='" + dt.Rows[0][0] + "'";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@title", textBox1.Text);
                        cmd.Parameters.AddWithValue("@description", textBox3.Text);
                        //cmd.Parameters.AddWithValue("@status", "Incomplete");
                        cmd.Parameters.AddWithValue("@createdAt", dateTimePicker1.Text);
                        cmd.Parameters.AddWithValue("@dueDate", dateTimePicker2.Text);
                        cmd.Parameters.AddWithValue("@project_id", val0[0]);
                        cmd.Parameters.AddWithValue("@user_id", val[0]);

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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Projects where manager_id='" + LoginPage.id + "' and project_name='"+ listView1.SelectedItems[0].SubItems[1].Text+"'", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM Users WHERE Manager_id='" + LoginPage.id + "' and Name='"+ listView1.SelectedItems[0].SubItems[4].Text + "'", conn);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                textBox1.Text = listView1.SelectedItems[0].Text;
                comboBox1.Text = dt.Rows[0]["id"].ToString() + "-" + dt.Rows[0]["project_name"].ToString();
                //comboBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
                //dateTimePicker1 = listView1.SelectedItems[0].SubItems;
                //dateTimePicker2 = listView1.SelectedItems[3].Text;
                comboBox2.Text = dt2.Rows[0]["User_id"].ToString() + "-" + dt2.Rows[0]["Name"].ToString();
                //comboBox2.Text = listView1.SelectedItems[0].SubItems[4].Text;

                textBox3.Text = listView1.SelectedItems[0].SubItems[5].Text;
            }
            catch
            {

            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy hh:mm:ss";
        }

        private void button3_Click(object sender, EventArgs e)
        {


            if(listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("First select a row to delete!");
            }
            else 
            {
                SqlConnection conn = new SqlConnection(myconnstring);
                try
                {
                    conn.Open();
                    /*SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Projects WHERE project_name='" + listView1.SelectedItems[0].SubItems[1].Text + "' ", conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);*/
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT id FROM Task WHERE title='" + listView1.SelectedItems[0].Text + "' ", conn);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string sql = "DELETE FROM Task WHERE id=@id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
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

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Manager mm = new Manager();
            mm.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage lp = new LoginPage();
            lp.Show();
        }
    }
}
