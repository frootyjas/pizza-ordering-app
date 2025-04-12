using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace pizza_ordering_app
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //string connStr = "server=localhost;user id=root;password=;database=pizza_app;";
        string connStr = "server=sql12.freesqldatabase.com;user id=sql12772758;password=ZfPWDQ2dFW;database=sql12772758;";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM users WHERE username = @username AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string role = reader["role"].ToString();
                        MessageBox.Show("Login successful!");

                        if (role == "admin")
                        {
                            AdminDashboard adminForm = new AdminDashboard();
                            adminForm.Show();
                            this.Hide();
                        }
                        else if (role == "staff")
                        {
                            // open Staff Dashboard
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
