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
using System.IO;

namespace FCC_Application.Forms.Forms_admin {
    public partial class F_stamp : Form {
        private bool add_f;
        public static AdminForm form;
        List<string> list = new List<string>();
        public F_stamp(bool add, AdminForm form1) {
            InitializeComponent();
            setcombobox();
            add_f = add;
            form = form1;
            SQLiteConnection connection = new SQLiteConnection("Data Source=data//object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.id FROM STAMP";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            List<int> list = new List<int>();
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    list.Add(Convert.ToInt32(Table_Departure.Rows[i][0].ToString()));
            }
            connection.Close();
            if(list.Count!=0)
                textBox3.Text = Convert.ToString(list.Max() + 1);
            else
                textBox3.Text = Convert.ToString(1);
        }
        public F_stamp(bool add, AdminForm form1, List<string> list) {
            InitializeComponent();
            setcombobox();
            add_f = add;
            form = form1;
            this.list = list;
            textBox3.Text = list[0];
            comboBox1.Text = setcombobox2(Convert.ToInt32(list[1]));
            textBox2.Text = list[2];
            textBox1.Text = list[3];
            textBox4.Text = list[4];
            textBox5.Text = list[5];
            Text = "Редактирование";
            button2.Text = "Сохранить";
            button3.Visible = true;
        }
        private void setcombobox() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TYPE.name FROM TYPE";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {         
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    comboBox1.Items.Add(Table_Departure.Rows[i][0].ToString());
                comboBox1.Text = Table_Departure.Rows[0][0].ToString();
            }
            
            connection.Close();
        }
        private string setcombobox2(int id) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TYPE.name FROM TYPE WHERE [id] = " + id.ToString() + "";

            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                    return Table_Departure.Rows[0][0].ToString();
            }
            return "";
        }
        private string setcombobox3(string name) {

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TYPE.id FROM TYPE WHERE [name] = \"" + name.ToString() + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            connection.Close();
            if (Table_Departure.Rows.Count > 0) {
                    return Table_Departure.Rows[0][0].ToString();
            }
            return "";
        }


        private void button2_Click(object sender, EventArgs e) {//добавить редактировать
            try {
                if (add_f) {//добавить
                    string combobox_str = setcombobox3(comboBox1.Text);

                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [STAMP] ([id], [id_type], [name], [path_model], [path_texture], [path_pixture]) " +
                                         "VALUES(@id, @id_type, @name, @path_model, @path_texture, @path_pixture)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", textBox3.Text);
                    command.Parameters.AddWithValue("@id_type", combobox_str);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.Parameters.AddWithValue("@path_model", textBox1.Text);
                    command.Parameters.AddWithValue("@path_texture", textBox4.Text);
                    command.Parameters.AddWithValue("@path_pixture", textBox5.Text);
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                if (!add_f) {//сохранить
                    string combobox_str = setcombobox3(comboBox1.Text);
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "DELETE FROM [STAMP] WHERE [id] = @id";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.ExecuteNonQuery();
                    commandText = "INSERT INTO [STAMP] ([id], [id_type], [name], [path_model], [path_texture], [path_pixture]) " +
                                         "VALUES(@id, @id_type, @name, @path_model, @path_texture, @path_pixture)";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", textBox3.Text);
                    command.Parameters.AddWithValue("@id_type", combobox_str);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.Parameters.AddWithValue("@path_model", textBox1.Text);
                    command.Parameters.AddWithValue("@path_texture", textBox4.Text);
                    command.Parameters.AddWithValue("@path_pixture", textBox5.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch {
                if (!add_f) {
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [STAMP] ([id], [id_type], [name], [path_model], [path_texture], [path_pixture]) " +
                                             "VALUES(@id, @id_type, @name, @path_model, @path_texture, @path_pixture)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.Parameters.AddWithValue("@id_type", list[1]);
                    command.Parameters.AddWithValue("@name", list[2]);
                    command.Parameters.AddWithValue("@path_model", list[3]);
                    command.Parameters.AddWithValue("@path_texture", list[4]);
                    command.Parameters.AddWithValue("@path_pixture", list[5]);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            Close();
        }

        private void button3_Click(object sender, EventArgs e) {
            DialogResult result2 = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result2 == DialogResult.No) {
                return;
            }

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string commandText = "DELETE FROM [STAMP] WHERE [id] = @id";
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            commandText = "DELETE FROM [NUM_PARAM_EQUIPMENT] WHERE [id_stamp] = @id";
            command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            commandText = "DELETE FROM [TECH_REG] WHERE [id_stamp] = @id";
            command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            connection.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void button4_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = "obj files(*.obj)|*.obj";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            textBox1.Text = Path.GetFileName(openFileDialog1.FileName);
            if (!File.Exists("Objects\\" + Path.GetFileName(openFileDialog1.FileName))) {
                File.Copy(openFileDialog1.FileName, "Objects\\" + Path.GetFileName(openFileDialog1.FileName), true);
            }
            
        }

        private void button5_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = "mtl files(*.mtl)|*.mtl";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            textBox4.Text = Path.GetFileName(openFileDialog1.FileName);
            if (!File.Exists("Textures\\" + Path.GetFileName(openFileDialog1.FileName))) {
                File.Copy(openFileDialog1.FileName, "Textures\\" + Path.GetFileName(openFileDialog1.FileName), true);
            }
        }

        private void button6_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            textBox5.Text = Path.GetFileName(openFileDialog1.FileName);
            if (!File.Exists("Pictures\\" + Path.GetFileName(openFileDialog1.FileName))) {
                File.Copy(openFileDialog1.FileName, "Pictures\\" + Path.GetFileName(openFileDialog1.FileName), true);
            }
        }
    }
}
