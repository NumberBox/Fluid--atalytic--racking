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
    public partial class F_type : Form {

        private bool add_f;
        public static AdminForm form;
        List<string> list = new List<string>();
        public F_type(bool add, AdminForm form1) {
            InitializeComponent();
            add_f = add;
            form = form1;
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TYPE.id FROM TYPE";
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
                textBox1.Text = Convert.ToString(list.Max() + 1);
            else
                textBox1.Text = Convert.ToString(1);
        }
        public F_type(bool add, AdminForm form1, List<string> list) {
            InitializeComponent();
            add_f = add;
            form = form1;
            this.list = list;
            textBox1.Text = list[0];
            textBox2.Text = list[1];
            Text = "Редактирование";
            button2.Text = "Сохранить";
            button3.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e) {//добавить сохранить
            try {
                if (add_f) {//добавить
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [TYPE] ([id], [name]) " +
                                         "VALUES(@id, @name)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                    
                }
                if (!add_f) {//сохранить
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "DELETE FROM [TYPE] WHERE [id] = @id AND [name] = @name";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.Parameters.AddWithValue("@name", list[1]);
                    command.ExecuteNonQuery();
                    commandText = "INSERT INTO [TYPE] ([id], [name]) " +
                                         "VALUES(@id, @name)";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.ExecuteNonQuery();
                    connection.Close();                      
                }
            }
            catch {
                if (!add_f) {
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [TYPE] ([id], [name]) " +
                                         "VALUES(@id, @name)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.Parameters.AddWithValue("@name", list[1]);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }       
            Close();
        }

        private void button3_Click(object sender, EventArgs e) {//delete
            DialogResult result2 = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result2 == DialogResult.No) {
                return;
            }

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string commandText = "DELETE FROM [TYPE] WHERE [id] = @id AND [name] = @name";
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.Parameters.AddWithValue("@name", list[1]);
            command.ExecuteNonQuery();

            commandText = "DELETE FROM [NUM_PARAM_EQUIPMENT] WHERE [id_stamp] in (SELECT STAMP.id FROM STAMP WHERE id_type = @id)";
            command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            commandText = "DELETE FROM [TECH_REG] WHERE [id_stamp] in (SELECT STAMP.id FROM STAMP WHERE id_type = @id)";
            command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            commandText = "DELETE FROM [STAMP] WHERE [id_type] = @id";
            command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();

            connection.Close();
        }

        
    }
}
