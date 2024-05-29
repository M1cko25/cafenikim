using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace JDK_BILLING_SYSTEM
{
    public partial class LandingForm : Form
    {
        public class Order
        {
            public string OrderName { get; set; }
            public int Quantity { get; set; }

            public Order(string orderName, int quantity)
            {
                OrderName = orderName;
                Quantity = quantity;
            }
        }
        string connString = "Server=127.0.0.1;database=cafe-billing-system;user=root;";
        private Dictionary<Guna.UI2.WinForms.Guna2CustomCheckBox, Guna.UI2.WinForms.Guna2Panel> checkBoxPanelMap = new Dictionary<Guna.UI2.WinForms.Guna2CustomCheckBox, Guna.UI2.WinForms.Guna2Panel>();
        public List<Guna.UI2.WinForms.Guna2Panel> drinksContent = new List<Guna.UI2.WinForms.Guna2Panel>();
        public List<Guna.UI2.WinForms.Guna2Panel> cakesContent = new List<Guna.UI2.WinForms.Guna2Panel>();
        bool isCakeAdded = false;
        private NewProduct addForm = new NewProduct();
        public int receiptNumber = 7483483;
        public LandingForm()
        {
            InitializeComponent();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void LandingForm_Load(object sender, EventArgs e)
        {
            contentPanel.Controls.Clear();
            orderPanel.Controls.Clear();
            try
            {
                MySqlConnection con = new MySqlConnection(connString);
                con.Open();
                string qry = "SELECT * FROM `drinks` WHERE stock != 0";
                MySqlCommand cmd = new MySqlCommand(qry, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    makePanel(reader.GetString("drinkName"), reader.GetDouble("price"), drinksContent);
                }
                
                reader.Close();
                con.Close();
            } catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server. Contact administrator.");
                        break;
                    case 1042:
                        MessageBox.Show("Can't connect To server");
                        break;
                    default:
                        MessageBox.Show($"MySQL error number: {ex.Number}. Message: {ex.Message}");
                        break;
                }
            }
        }

        public void makePanel(string Name, double Price, List<Guna.UI2.WinForms.Guna2Panel> list)
        {
            Guna.UI2.WinForms.Guna2Panel panel = new Guna.UI2.WinForms.Guna2Panel();
            panel.BackColor = System.Drawing.Color.Transparent;
            panel.BorderRadius = 35;
            panel.FillColor = System.Drawing.Color.White;
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "panel";
            panel.ShadowDecoration.BorderRadius = 35;
            panel.ShadowDecoration.Enabled = true;
            panel.ShadowDecoration.Parent = panel;
            panel.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 5, 5);
            panel.Size = new System.Drawing.Size(274, 81);
            panel.Margin = new System.Windows.Forms.Padding(5);
            panel.TabIndex = 0;
            panel.Click += new System.EventHandler(panelfood_Click);

            Guna.UI2.WinForms.Guna2CustomCheckBox checkBox = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            checkBox.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            checkBox.CheckedState.BorderRadius = 2;
            checkBox.CheckedState.BorderThickness = 0;
            checkBox.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            checkBox.CheckedState.Parent = checkBox;
            checkBox.Location = new System.Drawing.Point(31, 26);
            checkBox.Name = "checkBox";
            checkBox.ShadowDecoration.Parent = checkBox;
            checkBox.Size = new System.Drawing.Size(25, 25);
            checkBox.TabIndex = 2;
            checkBox.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            checkBox.UncheckedState.BorderRadius = 2;
            checkBox.UncheckedState.BorderThickness = 0;
            checkBox.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            checkBox.UncheckedState.Parent = this.guna2CustomCheckBox1;
            checkBox.CheckedChanged += new System.EventHandler(checkbox_CheckedChanged);

            Label name = new Label();
            name.AutoSize = true;
            name.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            name.Location = new System.Drawing.Point(69, 30);
            name.Name = "name";
            name.Size = new System.Drawing.Size(52, 17);
            name.TabIndex = 3;
            name.Text = Name;
            if (name.Text.Length > 15)
            {
                name.AutoSize = false;
                name.Size = new Size(135, 51);
                int x = name.Location.X;
                int y = (panel.Height / 2) - (name.Height / 2);
                name.Location = new Point(x, y);
            }
            name.Click += new System.EventHandler(nameLbl_Click);

            Label price = new Label();
            price.AutoSize = true;
            price.Font = new System.Drawing.Font("Segoe UI Symbol", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            price.Location = new System.Drawing.Point(209, 27);
            price.Name = "price";
            price.Size = new System.Drawing.Size(43, 20);
            price.TabIndex = 1;
            price.Text = "₱ " + Price;
            price.Click += new System.EventHandler(priceLbl_Click);

            contentPanel.Controls.Add(panel);
            panel.Controls.Add(checkBox);
            panel.Controls.Add(name);
            panel.Controls.Add(price);
            list.Add(panel);
        }

        private void panelfood_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                Control checkbox = panel.Controls["checkBox"];
                if (checkbox is Guna.UI2.WinForms.Guna2CustomCheckBox checkBox)
                {
                    if (checkBox.Checked)
                    {
                        checkBox.Checked = false;
                    } else
                    {
                        checkBox.Checked = true;
                    }
                }

            }
        }

        private void priceLbl_Click(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                Control panel = label.Parent;
                Control checkbox = panel.Controls["checkBox"];
                if (checkbox is Guna.UI2.WinForms.Guna2CustomCheckBox checkBox)
                {
                    if (checkBox.Checked)
                    {
                        checkBox.Checked = false;
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                }

            }
        }

        private void nameLbl_Click(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                Control panel = label.Parent;
                Control checkbox = panel.Controls["checkBox"];
                if (checkbox is Guna.UI2.WinForms.Guna2CustomCheckBox checkBox)
                {
                    if (checkBox.Checked)
                    {
                        checkBox.Checked = false;
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                }

            }
        }

        private void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2CustomCheckBox checkbox = sender as Guna.UI2.WinForms.Guna2CustomCheckBox;
            Guna.UI2.WinForms.Guna2Panel parentPanel = (Guna.UI2.WinForms.Guna2Panel)checkbox.Parent;
            Control name = parentPanel.Controls["name"];
            Control priceorder = parentPanel.Controls["price"];
            Label orderNameLbl = new Label();
            Label priceNumLbl = new Label();
            if (name is Label nameLbl)
            {
                orderNameLbl = nameLbl;
            } 
            if (priceorder is Label orderPrice)
            {
                string[] orderPricesplit = orderPrice.Text.Split(' ');
                priceNumLbl.Text = orderPricesplit[1];
            }
            string[] priceLblSplit = parentPanel.Controls["price"].Text.Split(' ');
            double price = Convert.ToDouble(priceLblSplit[1]);
            if (checkbox != null)
            {
                if (checkbox.Checked)
                {
                    if (!checkBoxPanelMap.ContainsKey(checkbox))
                    {
                        Guna.UI2.WinForms.Guna2Panel panel = new Guna.UI2.WinForms.Guna2Panel();
                        panel.BackColor = System.Drawing.Color.Transparent;
                        panel.BorderRadius = 8;
                        panel.FillColor = System.Drawing.Color.White;
                        panel.Location = new System.Drawing.Point(3, 3);
                        panel.Name = "order";
                        panel.ShadowDecoration.BorderRadius = 8;
                        panel.ShadowDecoration.Enabled = true;
                        panel.ShadowDecoration.Parent = panel;
                        panel.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0, 0, 5, 5);
                        panel.Size = new System.Drawing.Size(231, 78);
                        panel.TabIndex = 0;

                        Label ordername = new Label();
                        ordername.Text = orderNameLbl.Text;
                        ordername.AutoEllipsis = true;
                        ordername.Font = new System.Drawing.Font("Calisto MT", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        ordername.Location = new System.Drawing.Point(7, 8);
                        ordername.Name = "orderName";
                        ordername.Size = new System.Drawing.Size(174, 22); ;
                        ordername.TabIndex = 4;
                        

                        Label priceLbl = new Label();
                        priceLbl.AutoSize = true;
                        priceLbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        priceLbl.Location = new System.Drawing.Point(11, 45);
                        priceLbl.Name = "priceLbl";
                        priceLbl.Size = new System.Drawing.Size(44, 17);
                        priceLbl.TabIndex = 5;
                        priceLbl.Text = "Price: ₱";

                        Label priceNum = new Label();
                        priceNum.AutoSize = true;
                        priceNum.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        priceNum.Location = new System.Drawing.Point(62, 45);
                        priceNum.Name = "pricenum";
                        priceNum.Size = new System.Drawing.Size(16, 17);
                        priceNum.TabIndex = 6;
                        priceNum.Text = priceNumLbl.Text;

                        Guna.UI2.WinForms.Guna2NumericUpDown quanNum = new Guna.UI2.WinForms.Guna2NumericUpDown();
                        quanNum.BackColor = System.Drawing.Color.Transparent;
                        quanNum.BorderColor = System.Drawing.Color.Transparent;
                        quanNum.BorderThickness = 0;
                        quanNum.Cursor = System.Windows.Forms.Cursors.IBeam;
                        quanNum.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
                        quanNum.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
                        quanNum.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
                        quanNum.DisabledState.Parent = this.guna2NumericUpDown1;
                        quanNum.DisabledState.UpDownButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(177)))), ((int)(((byte)(177)))));
                        quanNum.DisabledState.UpDownButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
                        quanNum.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
                        quanNum.FocusedState.Parent = this.guna2NumericUpDown1;
                        quanNum.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        quanNum.ForeColor = System.Drawing.Color.Black;
                        quanNum.Location = new System.Drawing.Point(179, 6);
                        quanNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
                        quanNum.Name = "quanNum";
                        quanNum.ShadowDecoration.Parent = this.guna2NumericUpDown1;
                        quanNum.Size = new System.Drawing.Size(45, 25);
                        quanNum.TabIndex = 4;
                        quanNum.UpDownButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(26)))), ((int)(((byte)(16)))));
                        quanNum.UpDownButtonForeColor = System.Drawing.Color.White;
                        quanNum.Minimum = new decimal(new int[] {
                        1,
                        0,
                        0,
                        0});
                        quanNum.Value = new decimal(new int[] {
                        1,
                        0,
                        0,
                        0});

                        quanNum.ValueChanged += (s, ev) => UpdatePrice();

                        orderPanel.Controls.Add(panel);
                        panel.Controls.Add(ordername);
                        panel.Controls.Add(priceLbl);
                        panel.Controls.Add(priceNum);
                        panel.Controls.Add(quanNum);
                        checkBoxPanelMap[checkbox] = panel;
                    }
                }
                else
                {
                    if (checkBoxPanelMap.ContainsKey(checkbox))
                    {
                        Guna.UI2.WinForms.Guna2Panel panel = checkBoxPanelMap[checkbox];
                        orderPanel.Controls.Remove(panel);
                        checkBoxPanelMap.Remove(checkbox);
                    }
                }
            }
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            double tax = 0.5;
            double taxTotal = 0.00;
            double svc = 0.00;
            double subtotal = 0.00;
            double total = 0.00;
            if (orderPanel.Controls["order"] != null)
            {
                svc = 5.00;
            } else
            {
                svc = 0.00;
            }
            foreach (Guna.UI2.WinForms.Guna2Panel panel in orderPanel.Controls)
            {
                string foodName = panel.Controls["orderName"].Text;
                Guna.UI2.WinForms.Guna2NumericUpDown quantitynum = (Guna.UI2.WinForms.Guna2NumericUpDown)panel.Controls["quanNum"];
                double priceFood;

                if (double.TryParse(panel.Controls["priceNum"].Text, out priceFood))
                {
                    subtotal += Convert.ToDouble(quantitynum.Value) * priceFood;

                }
            }
            taxTotal = ((subtotal + svc) * tax) / 100;
            total = subtotal + svc + taxTotal;

            svcLbl.Text = Convert.ToString(svc.ToString("0.00"));
            subTotalLbl.Text = Convert.ToString(subtotal.ToString("0.00"));
            taxLbl.Text = Convert.ToString(taxTotal.ToString("0.00"));
            totalPriceLbl.Text = Convert.ToString(total.ToString("0.00"));
        }
        
        private void cakesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                cakesBtn.ImageOffset = new System.Drawing.Point(0, -6);
                cakesBtn.TextOffset = new System.Drawing.Point(0, -6);
                drinksBtn.ImageOffset = new System.Drawing.Point(0, 0);
                drinksBtn.TextOffset = new System.Drawing.Point(0, 0);
                line.Location = new Point(436, 154);
                if (!isCakeAdded)
                {
                    contentPanel.Controls.Clear();
                    isCakeAdded = true;
                    MySqlConnection con = new MySqlConnection(connString);
                    con.Open();
                    string qry = "SELECT * FROM `cakes` WHERE stock != 0";
                    MySqlCommand cmd = new MySqlCommand(qry, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        makePanel(reader.GetString("cakeName"), reader.GetDouble("price"), cakesContent);
                    }
                    
                } else
                {
                    contentPanel.Controls.Clear();
                    foreach(Guna.UI2.WinForms.Guna2Panel panels in cakesContent)
                    {
                        contentPanel.Controls.Add(panels);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void drinksBtn_Click(object sender, EventArgs e)
        {
           
            drinksBtn.ImageOffset = new System.Drawing.Point(0, -6);
            drinksBtn.TextOffset = new System.Drawing.Point(0, -6);
            cakesBtn.ImageOffset = new System.Drawing.Point(0, 0);
            cakesBtn.TextOffset = new System.Drawing.Point(0, 0);
            line.Location = new Point(134, 154);
            contentPanel.Controls.Clear();
            
            foreach (Guna.UI2.WinForms.Guna2Panel panel in drinksContent)
            {
                
                contentPanel.Controls.Add(panel);
            }
            
        }


        private void receiptBtn_Click(object sender, EventArgs e)
        {

            int order_status = guna2ComboBox1.SelectedIndex + 1;
            ReceiptForm receiptForm = new ReceiptForm();

            if (orderPanel.Controls["order"] != null && !string.IsNullOrEmpty(name.Text) && (!string.IsNullOrEmpty(paymentTxt.Text)) && (Convert.ToDouble(paymentTxt.Text) > Convert.ToDouble(totalPriceLbl.Text)))
            {
                string[] dateS = Convert.ToString(receiptForm.date).Split(' ');
                string[] timeS = Convert.ToString(receiptForm.time).Split('.');
                RichTextBox rcp = receiptForm.prvReceipt;
                checkRcpNum();
                rcp.Clear();
                rcp.Text = "";
                receiptForm.receiptNum = receiptNumber;
                rcp.AppendText("------------------------------ Receipt #: " + Convert.ToString(receiptNumber) +"-------------------------------" + Environment.NewLine);
                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                rcp.AppendText("\t\t" + "JDK CAFE" + Environment.NewLine);
                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                List<Order> orders = new List<Order>();

                string insufficientStockMessage = ""; 

                foreach (Guna.UI2.WinForms.Guna2Panel panel in orderPanel.Controls)
                {
                    Control lbl = panel.Controls["orderName"];
                    Control quan = panel.Controls["quanNum"];
                    if (lbl is Label ordername)
                    {
                        if (quan is Guna.UI2.WinForms.Guna2NumericUpDown quanNum)
                        {

                            string itemName = ordername.Text;
                            int quantity = (int)quanNum.Value;
                            orders.Add(new Order(itemName, quantity));

                            bool isDrink = CheckIsDrink(itemName);

                            bool hasSufficientStock;
                            if (isDrink)
                            {
                                hasSufficientStock = CheckStockInDrinks(itemName, quantity);
                            }
                            else
                            {
                                hasSufficientStock = CheckStockInCakes(itemName, quantity);
                            }

                            if (!hasSufficientStock)
                            {
                                int remainingStock;
                                if (isDrink)
                                {
                                    remainingStock = GetRemainingStockInDrinks(itemName);
                                }
                                else
                                {
                                    remainingStock = GetRemainingStockInCakes(itemName);
                                }

                                insufficientStockMessage += $"{itemName}: Insufficient stock. Remaining stock: {remainingStock}\n";
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(insufficientStockMessage))
                {
                    MessageBox.Show(insufficientStockMessage, "Insufficient Stock");
                    return;
                }
                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                rcp.AppendText("Customer Name \t\t" + name.Text + Environment.NewLine);
                rcp.AppendText("Order Type \t\t" + ((order_status == 1) ? "DINE IN" : "TAKE OUT") + Environment.NewLine);

                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                rcp.AppendText("Service Charge \t\t" + svcLbl.Text + Environment.NewLine);
                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                rcp.AppendText("Tax \t\t\t\t" + taxLbl.Text + Environment.NewLine);
                rcp.AppendText("Sub Total \t\t\t" + subTotalLbl.Text + Environment.NewLine);
                rcp.AppendText("Total Cost \t\t\t" + totalPriceLbl.Text + Environment.NewLine);
                rcp.AppendText("Payment \t\t\t" + paymentTxt.Text + Environment.NewLine);
                rcp.AppendText("change \t\t\t \t" + changelbl.Text + Environment.NewLine);

                rcp.AppendText("---------------------------------------------------------------------------------------------" + Environment.NewLine);
                rcp.AppendText(Convert.ToString(dateS[0]) + Environment.NewLine);
                rcp.AppendText(Convert.ToString(timeS[0]) + Environment.NewLine);
                receiptForm.setTotalAmount(Convert.ToDouble(totalPriceLbl.Text));
                receiptForm.SetOrders(orders);
                receiptForm.ShowDialog();
            } else
            {
                MessageBox.Show("INPUT ERROR");
            }
        }
        private bool CheckIsDrink(string orderName)
        {

            string query = "SELECT COUNT(*) FROM drinks WHERE drinkName = @DrinkName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrinkName", orderName);

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        private bool CheckStockInDrinks(string drinkName, int quantity)
        {
            string querySelectStock = "SELECT stock FROM drinks WHERE drinkName = @DrinkName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();

                using (MySqlCommand selectCommand = new MySqlCommand(querySelectStock, connection))
                {
                    selectCommand.Parameters.AddWithValue("@DrinkName", drinkName);
                    int currentStock = Convert.ToInt32(selectCommand.ExecuteScalar());

                    if (currentStock >= quantity)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool CheckStockInCakes(string cakeName, int quantity)
        {
            string querySelectStock = "SELECT stock FROM cakes WHERE cakeName = @CakeName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();

                using (MySqlCommand selectCommand = new MySqlCommand(querySelectStock, connection))
                {
                    selectCommand.Parameters.AddWithValue("@CakeName", cakeName);
                    int currentStock = Convert.ToInt32(selectCommand.ExecuteScalar());

                    if (currentStock >= quantity)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private int GetRemainingStockInDrinks(string drinkName)
        {
            string query = "SELECT stock FROM drinks WHERE drinkName = @DrinkName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DrinkName", drinkName);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private int GetRemainingStockInCakes(string cakeName)
        {
            string query = "SELECT stock FROM cakes WHERE cakeName = @CakeName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CakeName", cakeName);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            orderPanel.Controls.Clear();
            changelbl.Text = "0.00";
            name.Text = "";
            paymentTxt.Text = "";
            foreach (Control cont in contentPanel.Controls)
            {
                if (cont is Guna.UI2.WinForms.Guna2Panel foodPanel)
                {
                    foreach (Control control in foodPanel.Controls)
                    {
                        if (control is Guna.UI2.WinForms.Guna2CustomCheckBox check)
                        {
                            check.Checked = false;
                        }
                    }
                }
            }
            foreach (Guna.UI2.WinForms.Guna2Panel panel in drinksContent)
            {
                foreach(Control control in panel.Controls)
                {
                    if (control is Guna.UI2.WinForms.Guna2CustomCheckBox checkbox1)
                    {
                        checkbox1.Checked = false;
                    }
                }
            }
            foreach (Guna.UI2.WinForms.Guna2Panel panel in cakesContent)
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is Guna.UI2.WinForms.Guna2CustomCheckBox checkbox2)
                    {
                        checkbox2.Checked = false;
                    }
                }
            }
            UpdatePrice();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            NewProduct newProdForm = new NewProduct();
            newProdForm.Show();
            this.Hide();
            
        }

        private void checkRcpNum()
        {
            MySqlConnection con = new MySqlConnection(connString);
            con.Open();
            string qry = "SELECT * FROM `receipt-records` WHERE receiptNumber = @rcpNum";
            MySqlCommand cmd = new MySqlCommand(qry, con);
            cmd.Parameters.AddWithValue("@rcpNum", receiptNumber);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                receiptNumber++;
                checkRcpNum();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            double payment = 0;
            double total_price = 0;

            if (!string.IsNullOrEmpty(paymentTxt.Text))
            {
                try
                {
                    payment = Convert.ToDouble(paymentTxt.Text);
                    total_price = Convert.ToDouble(totalPriceLbl.Text);
                    changelbl.Text = (payment - total_price).ToString();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid input. Please enter a valid number.");
                }
            }   
        }
    }
}
