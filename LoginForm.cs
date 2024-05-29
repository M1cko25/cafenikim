using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace JDK_BILLING_SYSTEM
{
    public partial class LoginForm : Form
    {

        public enum LoginResult
        {
            Success,
            UsernameNotFound,
            IncorrectPassword
        }


        private string connString = "Server=127.0.0.1;database=cafe-billing-system;user=root;";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void dash_Click(object sender, EventArgs e)
        {
            string username = a.Text;
            string password = b.Text;

            LoginResult result = CheckLogin(username, password);

            switch (result)
            {
                case LoginResult.Success:
                    Dashboard dash = new Dashboard();
                    dash.FormClosed += (s, args) => this.Close();
                    dash.Show();
                    this.Hide();
                    break;
                case LoginResult.UsernameNotFound:
                    MessageBox.Show("Username not found.");
                    break;
                case LoginResult.IncorrectPassword:
                    MessageBox.Show("Incorrect password.");
                    break;
            }
        }

        private LoginResult CheckLogin(string username, string password)
        {
            string hashedPassword = GetMD5Hash(password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    string query = "SELECT COUNT(*) FROM user WHERE username = @Username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        connection.Open();

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 0)
                        {
                            return LoginResult.UsernameNotFound;
                        }
                    }

                    query = "SELECT COUNT(*) FROM user WHERE username = @Username AND password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count == 0)
                        {
                            return LoginResult.IncorrectPassword;
                        }
                    }
                }

                return LoginResult.Success;
            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return LoginResult.UsernameNotFound;
            }
            }

        private string GetMD5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void eye_CheckedChanged(object sender, EventArgs e)
        {
            if (eye.Checked)
            {
                b.UseSystemPasswordChar = false;
            } else
            {
                b.UseSystemPasswordChar = true;
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
