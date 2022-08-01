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


namespace FCC_Application.Forms.Forms_admin {
    public partial class F_technological : Form {

        private bool add_f;
        public static AdminForm form;
        List<string> list = new List<string>();
        public F_technological(bool add, AdminForm form1) {
            InitializeComponent();
            setcombobox();
            add_f = add;
            form = form1;
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TECH_REG.id FROM TECH_REG";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            List<int> list = new List<int>();
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    list.Add(Convert.ToInt32(Table_Departure.Rows[i][0].ToString()));
            }
            connection.Close();
            if (list.Count != 0)
                textBox1.Text = Convert.ToString(list.Max() + 1);
            else
                textBox1.Text = Convert.ToString(1);
        }
        public F_technological(bool add, AdminForm form1, List<string> list) {
            InitializeComponent();
            setcombobox();
            add_f = add;
            form = form1;
            this.list = list;
            textBox1.Text = list[0];
            comboBox1.Text = setcombobox3_stamp(Convert.ToInt32(list[1]));
            comboBox2.Text = setcombobox1_catalyst(Convert.ToInt32(list[2])); 
            comboBox3.Text = setcombobox2_parametr(Convert.ToInt32(list[3]));
            textBox3.Text = list[4];
            textBox5.Text = list[5];
            Text = "Редактирование";
            button2.Text = "Сохранить";
            button3.Visible = true;
        }

        private void setcombobox() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT CATALYST.name FROM CATALYST";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    comboBox2.Items.Add(Table_Departure.Rows[i][0].ToString());
                comboBox2.Text = Table_Departure.Rows[0][0].ToString();
            }
            sqlQuery = "SELECT PARAMETR_TECH_REG.name FROM PARAMETR_TECH_REG";
            SQLiteDataAdapter adapter_Departure1 = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure1 = new DataTable();
            adapter_Departure1.Fill(Table_Departure1);
            if (Table_Departure1.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure1.Rows.Count; i++)
                    comboBox3.Items.Add(Table_Departure1.Rows[i][0].ToString());
                comboBox3.Text = Table_Departure1.Rows[0][0].ToString();
            }
            sqlQuery = "SELECT STAMP.name FROM STAMP";
            SQLiteDataAdapter adapter_Departure2 = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure2 = new DataTable();
            adapter_Departure2.Fill(Table_Departure2);
            if (Table_Departure2.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure2.Rows.Count; i++)
                    comboBox1.Items.Add(Table_Departure2.Rows[i][0].ToString());
                comboBox1.Text = Table_Departure2.Rows[0][0].ToString();
            }
            connection.Close();
        }
        private string setcombobox1_catalyst(int id) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT CATALYST.name FROM CATALYST WHERE [id] = " + id.ToString() + "";

            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }
            return "";
        }
        private string setcombobox1_catalyst(string name) {

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT CATALYST.id FROM CATALYST WHERE [name] = \"" + name.ToString() + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }

            return "";
        }
        private string setcombobox2_parametr(int id) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT PARAMETR_TECH_REG.name FROM PARAMETR_TECH_REG WHERE [id] = " + id.ToString() + "";

            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }
            return "";
        }
        private string setcombobox2_parametr(string name) {

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT PARAMETR_TECH_REG.id FROM PARAMETR_TECH_REG WHERE [name] = \"" + name.ToString() + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }

            return "";
        }
        private string setcombobox3_stamp(int id) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.name FROM STAMP WHERE [id] = " + id.ToString() + "";

            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }
            return "";
        }
        private string setcombobox3_stamp(string name) {

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.id FROM STAMP WHERE [name] = \"" + name.ToString() + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                return Table_Departure.Rows[0][0].ToString();
            }

            return "";
        }



        private void button3_Click(object sender, EventArgs e) {
            DialogResult result2 = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result2 == DialogResult.No) {
                return;
            }

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string commandText = "DELETE FROM [TECH_REG] WHERE [id] = @id";
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                if (add_f) {//добавить
                    string combobox1_str = setcombobox3_stamp(comboBox1.Text);
                    string combobox2_str = setcombobox1_catalyst(comboBox2.Text);
                    string combobox3_str = setcombobox2_parametr(comboBox3.Text);
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [TECH_REG] ([id], [id_stamp], [id_catalyst], [id_parametr], [min_number], [max_number]) " +
                                         "VALUES(@id, @id_stamp, @id_catalyst, @id_parametr, @min_number, @max_number)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@id_stamp", combobox1_str);
                    command.Parameters.AddWithValue("@id_catalyst", combobox2_str);
                    command.Parameters.AddWithValue("@id_parametr", combobox3_str);
                    command.Parameters.AddWithValue("@min_number", textBox3.Text);
                    command.Parameters.AddWithValue("@max_number", textBox5.Text);
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                if (!add_f) {//сохранить
                    string combobox1_str = setcombobox3_stamp(comboBox1.Text);
                    string combobox2_str = setcombobox1_catalyst(comboBox2.Text);
                    string combobox3_str = setcombobox2_parametr(comboBox3.Text);
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "DELETE FROM [TECH_REG] WHERE [id] = @id";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.ExecuteNonQuery();
                    commandText = "INSERT INTO [TECH_REG] ([id], [id_stamp], [id_catalyst], [id_parametr], [min_number], [max_number]) " +
                                         "VALUES(@id, @id_stamp, @id_catalyst, @id_parametr, @min_number, @max_number)";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@id_stamp", combobox1_str);
                    command.Parameters.AddWithValue("@id_catalyst", combobox2_str);
                    command.Parameters.AddWithValue("@id_parametr", combobox3_str);
                    command.Parameters.AddWithValue("@min_number", textBox3.Text);
                    command.Parameters.AddWithValue("@max_number", textBox5.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch {
                if (!add_f) {
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [TECH_REG] ([id], [id_stamp], [id_catalyst], [id_parametr], [min_number], [max_number]) " +
                                         "VALUES(@id, @id_stamp, @id_catalyst, @id_parametr, @min_number, @max_number)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.Parameters.AddWithValue("@id_stamp", list[1]);
                    command.Parameters.AddWithValue("@id_catalyst", list[2]);
                    command.Parameters.AddWithValue("@id_parametr", list[3]);
                    command.Parameters.AddWithValue("@min_number", list[4]);
                    command.Parameters.AddWithValue("@max_number", list[5]);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            Close();
        }
    }
}
