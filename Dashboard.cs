using Guna.UI2.WinForms;
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
    public partial class Dashboard : Form
    {
        private List<Guna2Button> sidebarButtons = new List<Guna2Button>();
        private Form currentForm = null;

        public Dashboard()
        {
            InitializeComponent();
            sidebarButtons.Add(dash);
            sidebarButtons.Add(prod);


        }

        private void ChangeColor(Guna2Button clickedButton)
        {
            clickedButton.FillColor = Color.FromArgb(218, 234, 255);
            foreach (Guna2Button button in sidebarButtons)
            {
                if (button != clickedButton)
                {
                    button.FillColor = Color.Transparent;
                }
            }
        }

        private void ShowForm(Form form)
        {
            if (currentForm == form)
            {
                return;
            }

            viewPanel.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(form);

            currentForm = form;

            form.Show();
        }

        private void Load_Dash(object sender, EventArgs e)
        {
            Dash_Page dash = new Dash_Page(); 
            ShowForm(dash);
        }

        private void prod_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            Product dash = new Product();

            ShowForm(dash);
        }

        private void dash_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = (Guna2Button)sender;
            ChangeColor(clickedButton);
            Dash_Page dash = new Dash_Page();
            ShowForm(dash);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoginForm log = new LoginForm();
            log.FormClosed += (s, args) => this.Close();
            log.Show();
            this.Hide();
        }
    }
}
