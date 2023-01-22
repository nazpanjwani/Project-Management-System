using IPT_Project.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPT_Project
{
    public partial class LoginPage : Form
    {
        //public string id;
        public static string id;
        public LoginPage()
        {
            InitializeComponent();
        }
        //Class1 obj = new Class1();
        //string mail, pass;
        
        private void email_TextChanged(object sender, EventArgs e)
        {

            //mail = email.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        /*private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = obj.Select();
            listBox1.DataSource=dt.Rows;
            SqlConnection conn = new SqlConnection(myconnstring);
            //string V = "SELECT * FROM Users WHERE [Email] @Email AND [Password] @Password";
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Email = 'mail' AND Password = 'pass'", conn);
             //= cmd.ExecuteReader();
            //cmd.Parameters.Add(new SqlParameter("mail", DbType.String.mail));
            //SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Users WHERE Email = 'mail' AND Password = 'pass'");
            conn.Open();

        }*/

        private void password_TextChanged(object sender, EventArgs e)
        {
            
            //pass = password.Text;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            //DataTable dt = obj.Select();
            //listBox1.DataSource = dt.Rows;
            SqlConnection conn = new SqlConnection(myconnstring);
            //SqlDataAdapter sda = new SqlDataAdapter("SELECT Count(*) FROM Login WHERE username ='"+ email.Text+"' AND Password ='"+password.Text+"'", conn);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [ProjectsDb].[dbo].[Login] L join [ProjectsDb].[dbo].[Roles] R on R.Login_id=L.id where username ='" + email.Text + "' AND Password ='" + password.Text + "' AND RoleName='Manager'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM [ProjectsDb].[dbo].[Login] L join [ProjectsDb].[dbo].[Roles] R on R.Login_id=L.id where username ='" + email.Text + "' AND Password ='" + password.Text + "' AND RoleName='Employee'", conn);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            //label5.Text = dt1.Rows.Count.ToString();
            if (dt.Rows.Count >0)
            {
                this.Hide();
                //label4.Text= dt.Rows[0]["Login_id"].ToString();
                string id1 = dt.Rows[0]["Login_id"].ToString();
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT * FROM Managers WHERE login_id='" + id1 + "'", conn);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                id = dt2.Rows[0]["id"].ToString();

                //id = dt.Rows[0]["Login_id"].ToString();
                Manager ss = new Manager();
                ss.Show();
            }
            else if (dt1.Rows.Count > 0)
            {
                this.Hide();
                string id1 = dt1.Rows[0]["Login_id"].ToString();
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT User_id FROM Users WHERE login_id='" + id1 + "'", conn);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
                id = dt2.Rows[0]["User_id"].ToString();
                //id = dt1.Rows[0]["Login_id"].ToString();
                Employee ss = new Employee();
                ss.Show();
            }
            else
            {
                MessageBox.Show("Please Check your Username or password");
            }

        }
        //password.GotFocus += OnFocus;
        
        private void OnFocus(object sender, EventArgs e)
        {
            //txtPassword.useSystemPasswordChar = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //password.GotFocus;
            //Manager ss = new Manager();
            //ss.Show();

            //Tasks ss1 = new Tasks();
            //ss1.Show();

            //Employee ss2 = new Employee();
            //ss2.Show();

        }
    }
}
