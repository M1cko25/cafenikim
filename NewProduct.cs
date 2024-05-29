using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace JDK_BILLING_SYSTEM
{
    public partial class NewProduct : Form
    {
        string connString = "Server=127.0.0.1;database=cafe-billing-system;user=root;";
        bool isDrink = true;
        
        public NewProduct()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            addPanel();
        }

        private void addPanel()
        {
            Guna2Panel panel = new Guna2Panel();
            panel.BorderRadius = 10;
            panel.FillColor = System.Drawing.Color.White;
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "paneladd";
            panel.ShadowDecoration.Parent = this.paneladd;
            panel.Size = new System.Drawing.Size(508, 81);
            panel.TabIndex = 0;

            Label nameLbl = new Label();
            nameLbl.AutoSize = true;
            nameLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            nameLbl.Location = new System.Drawing.Point(22, 6);
            nameLbl.Name = "label1";
            nameLbl.Size = new System.Drawing.Size(50, 20);
            nameLbl.TabIndex = 1;
            nameLbl.Text = "Name";

            Label priceLbl = new Label();
            priceLbl.AutoSize = true;
            priceLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            priceLbl.Location = new System.Drawing.Point(318, 6);
            priceLbl.Name = "label2";
            priceLbl.Size = new System.Drawing.Size(43, 20);
            priceLbl.TabIndex = 3;
            priceLbl.Text = "Price";

            Guna2TextBox nameTxt = new Guna2TextBox();
            nameTxt.BorderColor = System.Drawing.Color.Black;
            nameTxt.BorderRadius = 10;
            nameTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            nameTxt.DefaultText = "";
            nameTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            nameTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            nameTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            nameTxt.DisabledState.Parent = this.nameTxt;
            nameTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            nameTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            nameTxt.FocusedState.Parent = this.nameTxt;
            nameTxt.ForeColor = System.Drawing.Color.Black;
            nameTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            nameTxt.HoverState.Parent = this.nameTxt;
            nameTxt.Location = new System.Drawing.Point(26, 29);
            nameTxt.Name = "nameTxt";
            nameTxt.PasswordChar = '\0';
            nameTxt.PlaceholderText = "";
            nameTxt.SelectedText = "";
            nameTxt.ShadowDecoration.Parent = this.nameTxt;
            nameTxt.Size = new System.Drawing.Size(248, 36);
            nameTxt.TabIndex = 0;

            Guna2TextBox priceTxt = new Guna2TextBox();
            priceTxt.BorderColor = System.Drawing.Color.Black;
            priceTxt.BorderRadius = 10;
            priceTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            priceTxt.DefaultText = "";
            priceTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            priceTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            priceTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            priceTxt.DisabledState.Parent = this.priceTxt;
            priceTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            priceTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            priceTxt.FocusedState.Parent = this.priceTxt;
            priceTxt.ForeColor = System.Drawing.Color.Black;
            priceTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            priceTxt.HoverState.Parent = this.priceTxt;
            priceTxt.Location = new System.Drawing.Point(322, 29);
            priceTxt.Name = "priceTxt";
            priceTxt.PasswordChar = '\0';
            priceTxt.PlaceholderText = "";
            priceTxt.SelectedText = "";
            priceTxt.ShadowDecoration.Parent = this.priceTxt;
            priceTxt.Size = new System.Drawing.Size(113, 36);
            priceTxt.TabIndex = 2;
            priceTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(priceTxt_KeyPress);

            Guna2ImageButton minusBtn = new Guna2ImageButton();
            minusBtn.CheckedState.Parent = this.minusBtn;
            minusBtn.HoverState.Parent = this.minusBtn;
            minusBtn.Image = global::JDK_BILLING_SYSTEM.Properties.Resources.Subtract__1_;
            minusBtn.Location = new System.Drawing.Point(471, 29);
            minusBtn.Name = "minusBtn";
            minusBtn.PressedState.Parent = this.minusBtn;
            minusBtn.Size = new System.Drawing.Size(25, 25);
            minusBtn.TabIndex = 4;
            minusBtn.Click += new System.EventHandler(minusBtn_Click);

            flowPanel.Controls.Add(panel);
            panel.Controls.Add(nameLbl);
            panel.Controls.Add(priceLbl);
            panel.Controls.Add(nameTxt);
            panel.Controls.Add(priceTxt);
            panel.Controls.Add(minusBtn);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void minusBtn_Click(object sender, EventArgs e)
        {
            if (sender is Guna2ImageButton minus)
            {
                if (minus.Parent is Guna2Panel panel)
                {
                    if (flowPanel.Controls.Contains(panel))
                    {
                        flowPanel.Controls.Remove(panel);
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int addNum = 0;
            LandingForm landing = new LandingForm();
            if (flowPanel.Controls["paneladd"] == null)
            {
                MessageBox.Show("Please add new products");
            } else {
                foreach(Guna2Panel panel in flowPanel.Controls)
                {
                    if (panel.Controls["nameTxt"] is Guna2TextBox name && panel.Controls["priceTxt"] is Guna2TextBox price)
                    {
                        if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(price.Text))
                        {
                            
                        }
                        else
                        {
                            if (isDrink)
                            {
                                MySqlConnection conn = new MySqlConnection(connString);
                                conn.Open();
                                string qry = "INSERT INTO `drinks`(`id`, `drinkName`, `price`) VALUES (?, ?, ?)";
                                MySqlCommand cmd = new MySqlCommand(qry, conn);
                                cmd.Parameters.AddWithValue("param1", null);
                                cmd.Parameters.AddWithValue("param2", name.Text.ToUpper().Trim());
                                cmd.Parameters.AddWithValue("param3", Convert.ToDouble(price.Text.Trim()));
                                cmd.ExecuteNonQuery();
                                addNum++;
                                landing.makePanel(name.Text.Trim(), Convert.ToDouble(price.Text.Trim()), landing.drinksContent);
                            } else
                            {
                                MySqlConnection conn = new MySqlConnection(connString);
                                conn.Open();
                                string qry = "INSERT INTO `cakes`(`id`, `cakeName`, `price`) VALUES (?, ?, ?)";
                                MySqlCommand cmd = new MySqlCommand(qry, conn);
                                cmd.Parameters.AddWithValue("param1", null);
                                cmd.Parameters.AddWithValue("param2", name.Text.ToUpper().Trim());
                                cmd.Parameters.AddWithValue("param3", Convert.ToDouble(price.Text.Trim()));
                                cmd.ExecuteNonQuery();
                                addNum++;
                                landing.makePanel(name.Text.Trim(), Convert.ToDouble(price.Text.Trim()), landing.cakesContent);
                            }
                        }
                    }
                }
            }
            MessageBox.Show(Convert.ToString(addNum) + " Products Added");
            flowPanel.Controls.Clear();
            addPanel();
        }

        private void bunifuDropdown1_onItemSelected(object sender, EventArgs e)
        {
            if (bunifuDropdown1.selectedIndex == 0)
            {
                isDrink = true;
            } else
            {
                isDrink = false;
            }
        }

        private void priceTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2TextBox textb)
            {
                if (char.IsControl(e.KeyChar))
                {
                    return;
                }

                if (char.IsDigit(e.KeyChar))
                {
                    return;
                }

                if (e.KeyChar == '.' && !priceTxt.Text.Contains("."))
                {
                    return;
                }

                if (e.KeyChar == '-' && textb.SelectionStart == 0 && !textb.Text.Contains("-"))
                {
                    return;
                }

                e.Handled = true;
            }
        }
    }
}
