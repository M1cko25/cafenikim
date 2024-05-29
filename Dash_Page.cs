using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace JDK_BILLING_SYSTEM
{
    public partial class Dash_Page : Form
    {
        string connString = "Server=127.0.0.1;database=cafe-billing-system;user=root";

        public Dash_Page()
        {
            InitializeComponent();
            LoadPastWeekIncome();
            guna2DateTimePicker1.Value = DateTime.Now;


        }
        private void RevenueToday(DateTime selectedDate) {
            string sql = "SELECT COALESCE(SUM(total_amount), 0) AS total_amount " +
             "FROM `receipt-records` " +
             "WHERE DATE(date) = @selectedDate";
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@selectedDate", selectedDate);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    double totalAmount = result != null ? Convert.ToDouble(result) : 0;
                    today_income.Text = $"{totalAmount} Php";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadPastWeekIncome()
        {
            string connectionString = connString;

            string sql = @"SELECT 
                        COALESCE(SUM(rr.total_amount), 0) AS total_income, 
                        CONCAT(LEFT(MONTHNAME(all_dates.date), 1), ' ', DAY(all_dates.date)) AS receipt_date 
                    FROM 
                        (
                            SELECT CURDATE() - INTERVAL (a.a + (10 * b.a) + (100 * c.a)) DAY AS date
                            FROM (
                                SELECT 0 AS a UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                            ) AS a
                            CROSS JOIN (
                                SELECT 0 AS a UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                            ) AS b
                            CROSS JOIN (
                                SELECT 0 AS a UNION ALL SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9
                            ) AS c
                        ) AS all_dates
                    LEFT JOIN `receipt-records` AS rr ON DATE(all_dates.date) = DATE(rr.date)
                    WHERE 
                        all_dates.date BETWEEN DATE_SUB(CURDATE(), INTERVAL 6 DAY) AND CURDATE() 
                    GROUP BY 
                        all_dates.date";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Date = reader["receipt_date"].ToString();
                            double totalIncome = reader.GetDouble("total_income");
                            chart1.Series["Revenue"].Points.AddXY(Date, totalIncome);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Dash_Page_Load(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value.Date;

            int currentMonth = DateTime.Now.Month;
            string[] months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(currentMonth).ToArray();
            date_combo.Items.AddRange(months);

            int currentMonthIndex = Array.IndexOf(months, DateTime.Now.ToString("MMMM"));
            date_combo.SelectedIndex = currentMonthIndex;
            RevenueToday(selectedDate);
            LoadMonthlyIncome(DateTime.Now.Month);
        }

        private void date_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMonthIndex = date_combo.SelectedIndex + 1;
            LoadMonthlyIncome(selectedMonthIndex);
        }

        private void LoadMonthlyIncome(int month)
        {
            // SQL query to get the monthly income for the selected month
            string sql = "SELECT COALESCE(SUM(total_amount), 0) AS monthly_income " +
                         "FROM `receipt-records` " +
                         "WHERE MONTH(date) = @month AND YEAR(date) = YEAR(CURRENT_DATE)";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@month", month);

                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    double monthlyIncome = result != null ? Convert.ToDouble(result) : 0;
                    monthly.Text = $"{monthlyIncome} Php";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value.Date;
            RevenueToday(selectedDate);

        }
    }
}
