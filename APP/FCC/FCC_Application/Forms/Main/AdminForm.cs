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
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace FCC_Application.Forms
{
    public partial class AdminForm : Form, IUserView
    {
        public AdminForm()
        {
            InitializeComponent();
            dataGridView1.EnableHeadersVisualStyles = false;
            button11_Click(Owner, EventArgs.Empty);
            label8.Visible = false;
            label9.Text = "Показать модель";
            panel3.Visible = false;
            panel4.Visible = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void ParentForm_Deactivate(object sender, EventArgs e)
        {
            OnDeactivated?.Invoke();
        }

        private void ParentForm_Activated(object sender, EventArgs e)
        {
            OnActivate?.Invoke();
        }

        public event Action<int, int> OnResizeEvent;
        public event Action<string, int> OnLoadedEvent;
        public event Action OnClosingEvent;
        public event Action OnActivate;
        public event Action OnDeactivated;
        public event Action<string> OnLoadObject;
        public event Action<int> OnSelectObject;
        public event Action HandInput;
        public event Action CaptureInput;
        public event Action<int> OnDeleteObject;
        public event Action<int> OnRelocateObject;

        public string table_str = "";
        public int count_columns = 0;
        
        private bool IsPreviewLoaded = false;
        public int count_preview_i = -1;
        

        public Form GetForm()
        {
            return this;
        }

        public IntPtr GetUnityParentHandle()
        {
            return unityParent.Handle;
        }

        public void ShowEmbeddingFalliedMessage(string fileName)
        {
            MessageBox.Show("Ошибка запуска подсистемы размещения и компоновки. По указанному пути отсутствуют файлы сцены:\n" + fileName);
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClosingEvent?.Invoke();
        }

        private void AdminForm_Activated(object sender, EventArgs e)
        {
            OnActivate?.Invoke();
        }

        private void AdminForm_Deactivate(object sender, EventArgs e)
        {
            OnDeactivated?.Invoke();
        }

        private void AdminForm_Resize(object sender, EventArgs e)
        {
            OnResizeEvent?.Invoke(unityParent.Width, unityParent.Height);
        }
        public void setColor_button() {
            button11.BackColor = Color.FromArgb(224, 224, 224);
            button1.BackColor = Color.FromArgb(224, 224, 224);
            button3.BackColor = Color.FromArgb(224, 224, 224);
            button5.BackColor = Color.FromArgb(224, 224, 224);
            button6.BackColor = Color.FromArgb(224, 224, 224);
            button4.BackColor = Color.FromArgb(224, 224, 224);
            button8.BackColor = Color.FromArgb(224, 224, 224);
            button9.BackColor = Color.FromArgb(224, 224, 224);
            button2.BackColor = Color.FromArgb(224, 224, 224);
            button12.BackColor = Color.FromArgb(224, 224, 224);
            button13.BackColor = Color.FromArgb(224, 224, 224);
            button14.BackColor = Color.FromArgb(224, 224, 224);
            button10.BackColor = Color.FromArgb(224, 224, 224);
        }

        public bool crate_db() {
            if (!File.Exists("data\\object_catalysts_tech.db3")) {
                MessageBox.Show("Cоздать базу надо сначала"); 
                return false;
            }
            if (!File.Exists("data\\users.db3")) {
                MessageBox.Show("Cоздать базу надо сначала");
                return false;
            }
            if (!File.Exists("data\\raw.db3")) {
                MessageBox.Show("Cоздать базу надо сначала"); //тех долг
                return false;
            }
            return true;
        }
        private void close_column_bd() {
            Column1.Visible = false;
            Column2.Visible = false;
            Column3.Visible = false;
            Column4.Visible = false;
            Column5.Visible = false;
            Column6.Visible = false;
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {
            try {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
            }
            catch {

            }
        }
  
        private void tableLayoutPanel2_Resize(object sender, EventArgs e) {
            panel1.Location = new Point(Width / 2, 0);
            panel1.Size = new Size(Width / 2, Height - 1);
            /*panel3.Location = new Point(unityParent.Location.X + Width / 2 + 1+ unityParent.Width*3/4+1, unityParent.Location.Y + 1);
            panel3.Size = new Size(unityParent.Width / 4, unityParent.Height / 14);*/
            panel4.Location = new Point(unityParent.Location.X + Width / 2 + 1, unityParent.Location.Y + 3);
            panel4.Size = new Size(unityParent.Width / 2, unityParent.Height / 14);
        }

        private void label7_Click_1(object sender, EventArgs e) {//открыть предпросмотр
            if (panel1.Visible) {
                panel1.Visible = false;
                OnClosingEvent?.Invoke();
            }
            else {
                panel1.Visible = true;
                panel1.BringToFront();
                panel1.Location = new Point(Width / 2, 0);
                panel1.Size = new Size(Width / 2, Height - 1);

                if (IsPreviewLoaded) {
                    panel4.Visible = true;
                    panel4.BringToFront();
                    panel4.Location = new Point(unityParent.Location.X + Width / 2 + 1, unityParent.Location.Y + 3);
                    panel4.Size = new Size(unityParent.Width / 2, unityParent.Height / 14);

                    /*panel3.Visible = true;
                    panel3.BringToFront();
                    panel3.Location = new Point(unityParent.Location.X + Width / 2 + 1 + unityParent.Width * 3 / 4+1, unityParent.Location.Y + 1);
                    panel3.Size = new Size(unityParent.Width / 4, unityParent.Height / 3);*/
                }
                if (!IsPreviewLoaded) {
                    OnLoadedEvent?.Invoke("FCC\\PreviewScene\\PreviewObjectScene.exe",8054);
                    IsPreviewLoaded = true;
                }
                
                
            }
            



            

            


            /*SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.path_model FROM STAMP";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            List<string> list = new List<string>();
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    list.Add(Table_Departure.Rows[i][0].ToString());
            }
            connection.Close();*/
            //лист с путями
        }

        private void label10_Click(object sender, EventArgs e) { //закрыть
            if (panel1.Visible) {
                panel1.Visible = false;
                panel4.Visible = false;
            }
            else {
                panel1.Visible = true;
                panel4.Visible = true;
            }
            panel1.BringToFront();
            panel1.Location = new Point(Width / 2, 0);
            panel1.Size = new Size(Width / 2, Height - 1);

            panel4.BringToFront();
            panel4.Location = new Point(unityParent.Location.X + Width / 2 + 1, unityParent.Location.Y + 3);
            panel4.Size = new Size(unityParent.Width / 2, unityParent.Height / 14);

            /*panel3.BringToFront();
            panel3.Location = new Point(unityParent.Location.X + Width / 2 + 1 + unityParent.Width * 3 / 4+1, unityParent.Location.Y + 1);
            panel3.Size = new Size(unityParent.Width / 4, unityParent.Height / 3);*/

            count_preview_i = 0;
            
        }

        /// <summary>
        /// Типы оборудования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e) {//типы оборудования
            setColor_button();
            button11.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button11.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "TYPE";
            count_columns = 2;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Тип";
            Column1.Visible = true;
            Column2.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM TYPE";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Марки оборудования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {  //марки
            setColor_button();
            button1.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = true;
            label4.Text = button1.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "STAMP";
            count_columns = 6;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Идентификатор типа";
            Column3.HeaderText = "Марка";
            Column4.HeaderText = "Файл 3D модели";
            Column5.HeaderText = "Файл текстуры";
            Column6.HeaderText = "Файл миниатюры";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;
            Column5.Visible = true;
            Column6.Visible = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM STAMP";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                sqlQuery = "SELECT TYPE.name FROM TYPE WHERE TYPE.id = " + dataGridView1[1, i].Value;
                adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                Table_Departure = new DataTable();
                adapter_Departure.Fill(Table_Departure);
                dataGridView1[1, i].Value = dataGridView1[1, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
            }
            connection.Close();

        }

        /// <summary>
        /// Параметры оборудования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e) {
            setColor_button();
            button3.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button3.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "PARAMETR_EQUIPMENT";
            count_columns = 3;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Название";
            Column3.HeaderText = "Размерность";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM PARAMETR_EQUIPMENT";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Числовые значения параметров оборудования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e) 
            {
            setColor_button();
            button5.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button5.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "NUM_PARAM_EQUIPMENT";
            count_columns = 4;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Идентификатор марки";
            Column3.HeaderText = "Идентификатор параметра";
            Column4.HeaderText = "Значение";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM NUM_PARAM_EQUIPMENT";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            try {
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT STAMP.name FROM STAMP WHERE STAMP.id = " + dataGridView1[1, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[1, i].Value = dataGridView1[1, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT PARAMETR_EQUIPMENT.name FROM PARAMETR_EQUIPMENT WHERE PARAMETR_EQUIPMENT.id = " + dataGridView1[2, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[2, i].Value = dataGridView1[2, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
            }
            catch {

            }
            connection.Close();
        }

        /// <summary>
        /// Сотрудники
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e) 
            {
            setColor_button();
            button6.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button6.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "USER";
            count_columns = 4;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Логин";
            Column3.HeaderText = "Пароль";
            Column4.HeaderText = "Роль";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\users.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM USER";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
            /*for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                dataGridView1[2, i].Value = "****";
            }*/
        }

        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)//добавить
          {
            if (table_str == "NUM_PARAM_EQUIPMENT") {
                Forms_admin.F_numbers form = new Forms_admin.F_numbers(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button5_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "NUM_PARAM_CATALYST") {
                Forms_admin.F_numbers_catalysts form = new Forms_admin.F_numbers_catalysts(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button9_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "NUMERICAL_CHARACTERISTIC_RAW") {
                Forms_admin.F_numbers_raw form = new Forms_admin.F_numbers_raw(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button13_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "TYPE") {
                Forms_admin.F_type form = new Forms_admin.F_type(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button11_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "CATALYST") {
                Forms_admin.F_catalyst form = new Forms_admin.F_catalyst(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button4_Click_1(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "STAMP") {
                Forms_admin.F_stamp form = new Forms_admin.F_stamp(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button1_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            
            if (table_str == "PARAMETR_EQUIPMENT") {
                Forms_admin.F_parametr form = new Forms_admin.F_parametr(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button3_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "RAW") {
                Forms_admin.F_raw form = new Forms_admin.F_raw(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button2_Click_1(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_CATALYST") {
                Forms_admin.F_parametr_catalysts form = new Forms_admin.F_parametr_catalysts(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button8_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_TECH_REG") {
                Forms_admin.F_parametr_tech form = new Forms_admin.F_parametr_tech(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button14_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_RAW") {
                Forms_admin.F_parametr_raw form = new Forms_admin.F_parametr_raw(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button12_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "TECH_REG") {
                Forms_admin.F_technological form = new Forms_admin.F_technological(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button10_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "USER") {
                Forms_admin.F_user form = new Forms_admin.F_user(true, this);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                    button6_Click(Owner, EventArgs.Empty);
                    return;
                }
            }





            
        }
        private string funk_del(string str) {
            string ret_str = "";
            for (int i = 0; i < str.Length; i++) {
                if (str[i] == ' ' && str[i+1] == '(') {
                    return ret_str;
                }
                ret_str += str[i];
            }
            return "";
        }

        /// <summary>
        /// Редактировать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) //редактировать
          { 
            List<string> list = new List<string>();
            try {
                for (int i = 0; i < count_columns; i++)
                    list.Add(dataGridView1[i, e.RowIndex].Value.ToString()); 
            }
            catch {
                return;
            }
            if (table_str == "STAMP") {
                list[1] = funk_del(list[1]);
                Forms_admin.F_stamp form = new Forms_admin.F_stamp(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button1_Click(Owner, EventArgs.Empty);
                    
                    return;
                }          
            }

            if (table_str == "TYPE") {
                Forms_admin.F_type form = new Forms_admin.F_type(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button11_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "CATALYST") {
                Forms_admin.F_catalyst form = new Forms_admin.F_catalyst(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button4_Click_1(Owner, EventArgs.Empty);
                    return;
                }
            }
            
            if (table_str == "NUM_PARAM_EQUIPMENT") {
                list[1] = funk_del(list[1]);
                list[2] = funk_del(list[2]);
                Forms_admin.F_numbers form = new Forms_admin.F_numbers(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button5_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "NUM_PARAM_CATALYST") {
                list[1] = funk_del(list[1]);
                list[2] = funk_del(list[2]);
                Forms_admin.F_numbers_catalysts form = new Forms_admin.F_numbers_catalysts(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button9_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            
            if (table_str == "PARAMETR_EQUIPMENT") {
                Forms_admin.F_parametr form = new Forms_admin.F_parametr(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button3_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_CATALYST") {
                Forms_admin.F_parametr_catalysts form = new Forms_admin.F_parametr_catalysts(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button8_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_TECH_REG") {
                Forms_admin.F_parametr_tech form = new Forms_admin.F_parametr_tech(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button14_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "USER") {
                Forms_admin.F_user form = new Forms_admin.F_user(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button6_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "RAW") {
                Forms_admin.F_raw form = new Forms_admin.F_raw(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button2_Click_1(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "PARAMETR_RAW") {
                Forms_admin.F_parametr_raw form = new Forms_admin.F_parametr_raw(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button12_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "TECH_REG") {
                list[1] = funk_del(list[1]);
                list[2] = funk_del(list[2]);
                list[3] = funk_del(list[3]);
                Forms_admin.F_technological form = new Forms_admin.F_technological(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button10_Click(Owner, EventArgs.Empty);
                    return;
                }
            }
            if (table_str == "NUMERICAL_CHARACTERISTIC_RAW") {
                list[1] = funk_del(list[1]);
                list[2] = funk_del(list[2]);
                Forms_admin.F_numbers_raw form = new Forms_admin.F_numbers_raw(false, this, list);
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    return;
                }
                if (DialogResult.OK == form.DialogResult || DialogResult.Abort == form.DialogResult) {
                    button13_Click(Owner, EventArgs.Empty);
                    return;
                }
            }

        }

        private string remove_brackets(string str) {
            string ret_str = "";
            for (int i = str.Length-2; i>=0; i--) {
                if (str[i] == '(') {
                    return ret_str;
                }
                ret_str = ret_str.Insert(0, str[i].ToString());
                
            }
            return "";
        }

        private void label9_Click(object sender, EventArgs e)//следующий //показать модель
        {
            label9.Text = "Следующий →";
            label8.Visible = true;

            panel4.Visible = true;
            panel4.BackColor = Color.FromArgb(239, 239, 239);
            panel4.BringToFront();
            panel4.Location = new Point(unityParent.Location.X + Width / 2+1, unityParent.Location.Y+3);
            panel4.Size = new Size(unityParent.Width / 2, unityParent.Height / 14);

            /*panel3.Visible = true;
            panel3.BackColor = Color.FromArgb(239, 239, 239);
            pictureBox1.BackColor = Color.FromArgb(239, 239, 239);
            panel3.BringToFront();
            panel3.Location = new Point(unityParent.Location.X + Width / 2 + 1 + unityParent.Width * 3 / 4+1, unityParent.Location.Y + 1);
            panel3.Size = new Size(unityParent.Width / 4, unityParent.Height / 3);*/


            if (label7.Visible == true) {
                if (IsPreviewLoaded) {
                    if (dataGridView1.Rows.Count > 0) {
                        count_preview_i++;
                        if (count_preview_i == dataGridView1.Rows.Count) {
                            count_preview_i = 0;
                        }
                        label17.Text = dataGridView1.Rows[count_preview_i].Cells[2].Value.ToString();
                        label16.Text = remove_brackets(dataGridView1.Rows[count_preview_i].Cells[1].Value.ToString()); 
                        string filename = dataGridView1.Rows[count_preview_i].Cells[3].Value.ToString();

                        /*if (dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString() != "") {
                            pictureBox1.Image = Image.FromFile("..\\..\\..\\..\\Pictures\\"+ dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString());
                        }*/
                        

                        

                        dataGridView1.Rows[count_preview_i].Selected = true;
                        if (filename != "") {
                            filename = filename.Substring(0, filename.IndexOf('.'));
                            OnLoadObject?.Invoke(filename);
                        }
                        else {
                            OnLoadObject?.Invoke("defolt");
                        }
                        
                    }
                }

            }

        }

        private void label8_Click(object sender, EventArgs e)//предыдущий
        {
            if (label7.Visible == true) {
                if (IsPreviewLoaded) {
                    if (dataGridView1.Rows.Count > 0) {
                        count_preview_i--;
                        if (count_preview_i == -1) {
                            count_preview_i = dataGridView1.Rows.Count-1;
                        }
                        dataGridView1.Rows[count_preview_i].Selected = true;
                        label17.Text = dataGridView1.Rows[count_preview_i].Cells[2].Value.ToString();
                        label16.Text = remove_brackets(dataGridView1.Rows[count_preview_i].Cells[1].Value.ToString());
                     /*   if (dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString() != "") {
                            pictureBox1.Image = Image.FromFile("..\\..\\..\\..\\Pictures\\" + dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString());
                        }*/
                        string filename = dataGridView1.Rows[count_preview_i].Cells[3].Value.ToString();
                        
                        if (filename != "") {
                            filename = filename.Substring(0, filename.IndexOf('.'));
                            OnLoadObject?.Invoke(filename);
                        }
                        else {
                            OnLoadObject?.Invoke("defolt");
                        }

                    }
                }

            }
            /*OnLoadObject?.Invoke("main_test_model");*/
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (label7.Visible == true) {
                if (IsPreviewLoaded) {
                    count_preview_i = dataGridView1.CurrentRow.Index;
                    label17.Text = dataGridView1.Rows[count_preview_i].Cells[2].Value.ToString();
                    label16.Text = remove_brackets(dataGridView1.Rows[count_preview_i].Cells[1].Value.ToString());
                    /*if (dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString() != "") {
                        pictureBox1.Image = Image.FromFile("..\\..\\..\\..\\Pictures\\" + dataGridView1.Rows[count_preview_i].Cells[5].Value.ToString());
                    }*/
                    string filename = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    if (filename != "") {
                        filename = filename.Substring(0, filename.IndexOf('.'));
                        OnLoadObject?.Invoke(filename);
                        
                    }
                    else {
                        OnLoadObject?.Invoke("defolt");
                    }
                }

            }
        }

        public void ParseMessage(string message)
        {
            throw new NotImplementedException();
        }
 
        /// <summary>
        /// Свойства катализаторов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e) {//свойства катализаторов
            setColor_button();
            button8.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button8.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "PARAMETR_CATALYST";
            count_columns = 3;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Название";
            Column3.HeaderText = "Размерность";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM PARAMETR_CATALYST";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Типы катализаторов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click_1(object sender, EventArgs e) {//типы катализаторов
            setColor_button();
            button4.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button4.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "CATALYST";
            count_columns = 2;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Тип";
            Column1.Visible = true;
            Column2.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM CATALYST";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Численные значения свойств катализаторов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e) {//численные значения свойств катализаторов
            setColor_button();
            button9.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button9.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "NUM_PARAM_CATALYST";
            count_columns = 4;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Идентификатор типа";
            Column3.HeaderText = "Идентификатор параметра";
            Column4.HeaderText = "Значение";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM NUM_PARAM_CATALYST";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);

                    
                }
            }
            try {
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT CATALYST.name FROM CATALYST WHERE CATALYST.id = " + dataGridView1[1, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[1, i].Value = dataGridView1[1, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT PARAMETR_CATALYST.name FROM PARAMETR_CATALYST WHERE PARAMETR_CATALYST.id = " + dataGridView1[2, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[2, i].Value = dataGridView1[2, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
            }
            catch {

            }
            connection.Close();
        }

        /// <summary>
        /// Типы сырья
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e) {//типы сырья
            setColor_button();
            button2.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button2.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "RAW";
            count_columns = 2;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Тип";
            Column1.Visible = true;
            Column2.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM RAW";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Численные значения свойств сырья
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e) {//численные значения свойств сырья
            setColor_button();
            button13.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button13.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "NUMERICAL_CHARACTERISTIC_RAW";
            count_columns = 4;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Идентификатор типа";
            Column3.HeaderText = "Идентификатор параметра";
            Column4.HeaderText = "Значение";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM NUMERICAL_CHARACTERISTIC_RAW";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            try {
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT RAW.name FROM RAW WHERE RAW.id = " + dataGridView1[1, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[1, i].Value = dataGridView1[1, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                    sqlQuery = "SELECT PARAMETR_RAW.name FROM PARAMETR_RAW WHERE PARAMETR_RAW.id = " + dataGridView1[2, i].Value;
                    adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                    Table_Departure = new DataTable();
                    adapter_Departure.Fill(Table_Departure);
                    dataGridView1[2, i].Value = dataGridView1[2, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                }
            }
            catch {

            }
            connection.Close();
        }

        /// <summary>
        /// Свойства сырья
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e) {//свойства сырья
            setColor_button();
            button12.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button12.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "PARAMETR_RAW";
            count_columns = 3;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Название";
            Column3.HeaderText = "Размерность";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;


            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM PARAMETR_RAW";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        /// <summary>
        /// Технологические регламенты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e) {//тех регламенты
            setColor_button();
            button10.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button10.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "TECH_REG";
            count_columns = 6;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Идентификатор марки";
            Column3.HeaderText = "Идентификатор катализатора";
            Column4.HeaderText = "Идентификатор параметра";
            Column5.HeaderText = "Минимальное значение";
            Column6.HeaderText = "Максимальное значение";
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
            Column4.Visible = true;
            Column5.Visible = true;
            Column6.Visible = true;

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM TECH_REG";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
                try {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                        sqlQuery = "SELECT STAMP.name FROM STAMP WHERE STAMP.id = " + dataGridView1[1, i].Value;
                        adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                        Table_Departure = new DataTable();
                        adapter_Departure.Fill(Table_Departure);
                        dataGridView1[1, i].Value = dataGridView1[1, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                        sqlQuery = "SELECT CATALYST.name FROM CATALYST WHERE CATALYST.id = " + dataGridView1[2, i].Value;
                        adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                        Table_Departure = new DataTable();
                        adapter_Departure.Fill(Table_Departure);
                        dataGridView1[2, i].Value = dataGridView1[2, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++) {
                        sqlQuery = "SELECT PARAMETR_TECH_REG.name FROM PARAMETR_TECH_REG WHERE PARAMETR_TECH_REG.id = " + dataGridView1[3, i].Value;
                        adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                        Table_Departure = new DataTable();
                        adapter_Departure.Fill(Table_Departure);
                        dataGridView1[3, i].Value = dataGridView1[3, i].Value + " (" + Table_Departure.Rows[0][0] + ")";
                    }
                }
                catch {

                }        
            connection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if(comboBox1.Text == "Типы оборудования") {
                button11_Click(Owner, EventArgs.Empty);
            }
            if (comboBox1.Text == "Марки и модели оборудования") {

            }
            if (comboBox1.Text == "Параметры оборудования оборудования") {

            }
            if (comboBox1.Text == "Численные характеристики параметров оборудования") {

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Параметры технологических регламентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e) {//параметры технологических регламентов
            setColor_button();
            button14.BackColor = Color.FromArgb(192, 192, 255);
            label7.Visible = false;
            label4.Text = button14.Text;
            if (!crate_db()) {//базы данных нет и ее не могли создать
                return;
            }
            table_str = "PARAMETR_TECH_REG";
            count_columns = 3;
            dataGridView1.Rows.Clear();
            close_column_bd();
            Column1.HeaderText = "Идентификатор";
            Column2.HeaderText = "Название";
            Column3.HeaderText = "Размерность";
       
            Column1.Visible = true;
            Column2.Visible = true;
            Column3.Visible = true;
      

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT * FROM PARAMETR_TECH_REG";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++)
                    dataGridView1.Rows.Add(Table_Departure.Rows[i].ItemArray);
            }
            connection.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void label14_Click(object sender, EventArgs e) {

        }

        private void panel4_Paint(object sender, PaintEventArgs e) {

        }
    }
}
