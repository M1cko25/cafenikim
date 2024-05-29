using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JDK_BILLING_SYSTEM
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LandingForm landing = new LandingForm();
            landing.FormClosed += (s, args) => this.Close();
            landing.Show();
            this.Hide();
        }

        private void dash_Click(object sender, EventArgs e)
        {
            LoginForm log_in = new LoginForm();
            log_in.FormClosed += (s, args) => this.Close();

            log_in.Show();

            this.Hide();
        }
    }
}
