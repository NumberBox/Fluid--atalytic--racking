using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using FCC_Application.Models;

namespace FCC_Application.Forms.Additional
{
    public partial class Task : Form
    {
        public static string P;
        public static string E;
        public static string Catalyst;
        public static string Stamp;
        public static string Raw;
        public Task()
        {
            InitializeComponent();
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT name FROM CATALYST";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    comboBox2.Items.Add(Table_Departure.Rows[i][0].ToString());
                comboBox2.Text = Table_Departure.Rows[0][0].ToString();
            }
            connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            sqlQuery = "SELECT name FROM RAW";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    comboBox3.Items.Add(Table_Departure.Rows[i][0].ToString());
                comboBox3.Text = Table_Departure.Rows[0][0].ToString();
            }
            connection.Close();
            //comboBox1.Text = comboBox1.Items[2].ToString();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            
            try {
                P = Convert.ToDouble(textBox1.Text).ToString().Replace(',','.');
                E = Convert.ToDouble(textBox2.Text).ToString().Replace(',', '.');
                Stamp = Convert.ToDouble(textBox4.Text).ToString().Replace(',', '.');

            }
            catch {
                MessageBox.Show("Проверьте правильность запоняемых данных", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }
            PlannerTask.concentration_limit = Convert.ToDouble(textBox4.Text);
            Catalyst = comboBox2.Text;
            Raw = comboBox3.Text;
            PlannerTask.catalyst_name = Catalyst;
            PlannerTask.raw_name = Raw;
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) {

        }
    }
}
