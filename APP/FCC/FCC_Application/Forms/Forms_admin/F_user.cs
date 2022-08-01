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
using System.Security.Cryptography;

namespace FCC_Application.Forms.Forms_admin {
    public partial class F_user : Form {

        private bool add_f;
        public static AdminForm form;
        List<string> list = new List<string>();
        public F_user(bool add, AdminForm form1) {
            InitializeComponent();
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(label3, "Пароль будет сохранен в виде хеша");
            ToolTip1.SetToolTip(textBox2, "Пароль будет сохранен в виде хеша");
            comboBox1.Items.Add("Администратор");
            comboBox1.Items.Add("Проектировщик");
            comboBox1.Text = "Администратор";
            add_f = add;
            form = form1;
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT USER.id FROM USER";
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
        public F_user(bool add, AdminForm form1, List<string> list) {
            InitializeComponent();
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(label3, "Пароль будет сохранен в виде хеша");
            ToolTip1.SetToolTip(textBox2, "Пароль будет сохранен в виде хеша");
            comboBox1.Items.Add("Администратор");
            comboBox1.Items.Add("Проектировщик");
            add_f = add;
            form = form1;
            this.list = list;
            textBox1.Text = list[0];
            textBox3.Text = list[1];
            textBox2.Text = "";
            comboBox1.Text = list[3];
            Text = "Редактирование";
            button2.Text = "Сохранить";
            button3.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                if (add_f) {//добавить
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [USER] ([id], [login], [password], [role]) " +
                                         "VALUES(@id, @login, @password, @role)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@login", textBox3.Text);                   
                    MD5 md = MD5.Create();
                    command.Parameters.AddWithValue("@password", Convert.ToBase64String(md.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text))));
                    command.Parameters.AddWithValue("@role", comboBox1.Text);
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                if (!add_f) {//сохранить
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
                    connection.Open();
                    string commandText = "DELETE FROM [USER] WHERE [id] = @id";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.ExecuteNonQuery();
                    commandText = "INSERT INTO [USER] ([id], [login], [password], [role]) " +
                                          "VALUES(@id, @login, @password, @role)";
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", textBox1.Text);
                    command.Parameters.AddWithValue("@login", textBox3.Text);
                    MD5 md = MD5.Create();
                    command.Parameters.AddWithValue("@password", Convert.ToBase64String(md.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text))));
                    command.Parameters.AddWithValue("@role", comboBox1.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch {
                if (!add_f) {
                    SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
                    connection.Open();
                    string commandText = "INSERT INTO [USER] ([id], [login], [password], [role]) " +
                                          "VALUES(@id, @login, @password, @role)";
                    SQLiteCommand command = new SQLiteCommand(commandText, connection);
                    command.Parameters.AddWithValue("@id", list[0]);
                    command.Parameters.AddWithValue("@login", list[1]);
                    command.Parameters.AddWithValue("@password", list[2]);
                    command.Parameters.AddWithValue("@role", list[3]);
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

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
            connection.Open();
            string commandText = "DELETE FROM [USER] WHERE [id] = @id";
            SQLiteCommand command = new SQLiteCommand(commandText, connection);
            command.Parameters.AddWithValue("@id", list[0]);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
