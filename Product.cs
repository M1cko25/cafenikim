using MySql.Data.MySqlClient;
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
    public partial class Product : Form
    {

        public Product()
        {
            InitializeComponent();
        }

        public void FetchDrinks()
        {
            string connectionString = "Server=127.0.0.1;database=cafe-billing-system;user=root";

            string sql = "SELECT id, drinkName, stock, price FROM drinks";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    drink.DataSource = dataTable;

                    AdjustColumnWidths(drink);

                    drink.ColumnHeadersHeight = 40;
                    drink.Columns["id"].HeaderText = "ID";
                    drink.Columns["drinkName"].HeaderText = "Name";
                    drink.Columns["stock"].HeaderText = "Stock";
                    drink.Columns["price"].HeaderText = "Price";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching cakes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public void FetchCakes()
        {
            string connectionString = "Server=127.0.0.1;database=cafe-billing-system;user=root";

            string sql = "SELECT id, cakeName, stock, price FROM cakes";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    cake.DataSource = dataTable;

                    AdjustColumnWidths(cake);

                    cake.ColumnHeadersHeight = 40;
                    cake.Columns["id"].HeaderText = "ID";
                    cake.Columns["cakeName"].HeaderText = "Name";
                    cake.Columns["stock"].HeaderText = "Stock";
                    cake.Columns["price"].HeaderText = "Price";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching cakes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = guna2TextBox1.Text.ToLower();
            bool anyMatchFound = false;

            if (!string.IsNullOrEmpty(searchText))
            {
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[cake.DataSource];
                currencyManager.SuspendBinding();

                foreach (DataGridViewRow row in cake.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row entry

                    bool rowVisible = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            rowVisible = true;
                            anyMatchFound = true;
                            break;
                        }
                    }

                    row.Visible = rowVisible;
                }

                currencyManager.ResumeBinding();

                if (!anyMatchFound)
                {
                    cake.DataSource = new DataTable();
                }
            }
            else
            {
                FetchCakes();
            }

        }

        private void AdjustColumnWidths(DataGridView table)
        {
            // Calculate the width of the CakeName column
            int cakeNameWidth = table.Name == "cake" ? table.Columns["cakeName"].Width : table.Columns["drinkName"].Width ;
            foreach (DataGridViewColumn column in table.Columns)
            {
                // Calculate the maximum width of the content in the column
                int maxWidth = 0;

                foreach (DataGridViewRow row in table.Rows)
                {
                    if (row.Cells[column.Index].Value != null)
                    {
                        int cellWidth = TextRenderer.MeasureText(row.Cells[column.Index].Value.ToString(), table.Font).Width;
                        if (cellWidth > maxWidth)
                        {
                            maxWidth = cellWidth;
                        }
                    }
                }

                if (column.Name == "stock")
                {
                    column.ReadOnly = false;
                }
                else
                {
                    column.ReadOnly = true;
                }

                // Adjust the width of the Price column
                if (column.Name == "price")
                {
                    column.Width = Math.Max(cakeNameWidth / 4, maxWidth + 20);
                }
                else
                {
                    // Set the width of other columns based on the maximum width of their content
                    column.Width = maxWidth + 20;
                }
            }
        }

        private void Product_Load(object sender, EventArgs e)
        {
            FetchCakes();
            FetchDrinks();
        }



        private void UpdateStockInDatabase(int id, int newStockValue)
        {
            string connectionString = "Server=127.0.0.1;database=cafe-billing-system;user=root";
            string sql = "UPDATE cakes SET stock = @newStockValue WHERE id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@newStockValue", newStockValue);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
              
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Stock updated successfully in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FetchCakes();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update stock in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating stock in the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void UpdateStockInDatabaseDrink(int id, int newStockValue)
        {
            string connectionString = "Server=127.0.0.1;database=cafe-billing-system;user=root";
            string sql = "UPDATE drinks SET stock = @newStockValue WHERE id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@newStockValue", newStockValue);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Stock updated successfully in the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FetchDrinks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update stock in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating stock in the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void cell(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == cake.Columns["stock"].Index && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(cake.Rows[e.RowIndex].Cells["id"].Value);

                int newStockValue = Convert.ToInt32(cake.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                UpdateStockInDatabase(id, newStockValue);

                //MessageBox.Show($"ID: {id}\nNew Stock Value: {newStockValue}", "Stock Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void changes(object sender, EventArgs e)
        {

            string searchText = guna2TextBox3.Text.ToLower();
            bool anyMatchFound = false;

            if (!string.IsNullOrEmpty(searchText))
            {
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[drink.DataSource];
                currencyManager.SuspendBinding();

                foreach (DataGridViewRow row in drink.Rows)
                {
                    if (row.IsNewRow) continue; // Skip the new row entry

                    bool rowVisible = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            rowVisible = true;
                            anyMatchFound = true;
                            break;
                        }
                    }

                    row.Visible = rowVisible;
                }

                currencyManager.ResumeBinding();

                if (!anyMatchFound)
                {
                    drink.DataSource = new DataTable();
                }
            }
            else
            {
                FetchDrinks();
            }
        }

        private void valuchange(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == drink.Columns["stock"].Index && e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(drink.Rows[e.RowIndex].Cells["id"].Value);

                int newStockValue = Convert.ToInt32(drink.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                UpdateStockInDatabaseDrink(id, newStockValue);

                //MessageBox.Show($"ID: {id}\nNew Stock Value: {newStockValue}", "Stock Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            NewProduct newProduct = new NewProduct();
            newProduct.Show();


        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            NewProduct newProduct = new NewProduct();
            newProduct.Show();
        }
    }
}
