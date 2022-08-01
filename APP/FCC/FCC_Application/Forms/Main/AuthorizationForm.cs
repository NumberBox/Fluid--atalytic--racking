using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Views;
using FCC_Application.Presenters;
using System.Data.SQLite;
using System.Security.Cryptography;


namespace FCC_Application.Forms
{
    public partial class AuthorizationForm : Form, IAuthorizationView
    {
        public AuthorizationForm()
        {
            InitializeComponent();
            CenterToParent();
        }

        public event Action GotAccess;
        public IUserView userForm;

        public Form GetForm() {
            return this;
        }

        public Form AccessIsAllowed(IAppMainWindowView appMainWindowView)
        {
            var embeddedPresenter = new EmbeddedPresenter(appMainWindowView, userForm, new UnityView());
            return userForm.GetForm();
        }

        private void SignInButton_Click(object sender, EventArgs e) //войти
        {
            MD5 md = MD5.Create();                                                                        //расскоментируй меня
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT USER.role, USER.login, USER.password FROM USER WHERE [login] = \"" + textBox1.Text + "\" AND [password] = \""+ Convert.ToBase64String(md.ComputeHash(Encoding.UTF8.GetBytes(textBox2.Text))) + "\"";
            SQLiteCommand command = new SQLiteCommand(sqlQuery, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            bool check_empty = false;

            while (reader.Read()) {
                if(reader[0].ToString()== "Администратор") {
                    userForm = new AdminForm();
                    GotAccess?.Invoke();
                    check_empty = true;
                }
                if (reader[0].ToString() == "Проектировщик") {
                    userForm = new PlannerForm();
                    GotAccess?.Invoke();
                    check_empty = true;
                }
            }
            connection.Close();

            if (!check_empty) {
                MessageBox.Show("Проверьте правильность введенных данных\n(пароль неверен или логин пользователя отсутствует в базе)", "Вход не выполнен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            /*if (textBox1.Text == "Администратор" || textBox1.Text == "1") {
                userForm = new AdminForm();
                GotAccess?.Invoke();
            }
            else if (textBox1.Text == "Проектировщик" || textBox1.Text == "2") {
                userForm = new PlannerForm();
                GotAccess?.Invoke();
            }*/
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {

        }

        private void label6_Click(object sender, EventArgs e) {
            Additional.MathModelCalculation form = new Additional.MathModelCalculation(null);
            form.Owner = this;
            form.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e) {

        }
    }
}
