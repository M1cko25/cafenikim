using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using Guna.UI2.WinForms;
using static JDK_BILLING_SYSTEM.LandingForm;

namespace JDK_BILLING_SYSTEM
{
    public partial class ReceiptForm : Form
    {
        string connString = "Server=127.0.0.1;database=cafe-billing-system;user=root;";
        public int receiptNum = 0;
        private double total_amount;
        private List<Order> orders;
        public ReceiptForm()
        {
            InitializeComponent();
            shadowForm.SetShadowForm(this);
            orders = new List<Order>();
        }
        public void setTotalAmount(double amount) {
            this.total_amount = amount;
        
        }
        public void SetOrders(List<Order> orders)
        {
            this.orders = orders;

        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            foreach (Order order in orders)
            {
                // Check if the order is for a drink
                bool isDrink = CheckIsDrink(order.OrderName);

                if (isDrink)
                {
                    UpdateStockInDrinks(order.OrderName, order.Quantity);
                }
                else
                {
                    UpdateStockInCakes(order.OrderName, order.Quantity);
                }
            }
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            //MessageBox.Show(total_amount.ToString());
            saveRecord();


        }

        private void UpdateStockInDrinks(string drinkName, int quantity)
        {

            string query = "UPDATE drinks SET stock = stock - @Quantity WHERE drinkName = @DrinkName";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@DrinkName", drinkName);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateStockInCakes(string cakeName, int quantity)
        {
            string query = "UPDATE cakes SET stock = stock - @Quantity WHERE cakeName = @CakeName";
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@CakeName", cakeName);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
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




        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public DateTime date = DateTime.Today;
        
        public TimeSpan time = DateTime.Now.TimeOfDay;
        
        private void saveBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            string[] dateS = Convert.ToString(date).Split(' ');
            string[] timeS = Convert.ToString(time).Split('.');
            saveFile.FileName = "Receipt " + dateS[0] + " " + Convert.ToString(timeS[0]);
            saveFile.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";


            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFile.FileName))
                    sw.WriteLine(prvReceipt.Text);
            }
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            prvReceipt.Copy();
            MessageBox.Show("Receipt Copied");
        }

     
        private void saveRecord()
        {
            LandingForm landing = new LandingForm();
                string[] dateS = Convert.ToString(date).Split(' ');
                string[] timeS = Convert.ToString(time).Split('.');
                try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                conn.Open();
                string query = "INSERT INTO `receipt-records`(`receiptNumber`, `time`,`total_amount`) VALUES (?, ?, ?)";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("param1", receiptNum);
                command.Parameters.AddWithValue("param2", Convert.ToString(dateS[0]));
                command.Parameters.AddWithValue("param3", total_amount);

                int recorded = command.ExecuteNonQuery();
                if (recorded > 0)
                {
                    MessageBox.Show("Receipt Recorded");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Receipt Not Recorded");
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Receipt Not Recorded, Error: " + ex.Message);
            }
            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(prvReceipt.Text, new Font("Arial", 14, FontStyle.Regular), Brushes.Black, 120, 120);
        }
    }
}
