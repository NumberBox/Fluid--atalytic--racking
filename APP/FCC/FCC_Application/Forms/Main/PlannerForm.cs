using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Models;
using FCC_Application.Views;
using FCC_Application.Forms.Additional;
using FCC_Application.Utills;
using System.Data.SQLite;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FCC_Application.Forms
{
    public partial class PlannerForm : Form, IUserView
    {
        public event Action<int, int> OnResizeEvent;
        public event Action<string,int> OnLoadedEvent;
        public event Action OnClosingEvent;
        public event Action OnActivate;
        public event Action OnDeactivated;
        public event Action<string> OnLoadObject;
        public event Action<int> OnSelectObject;
        public event Action HandInput;
        public event Action CaptureInput;
        public event Action<int> OnDeleteObject;
        public event Action<int> OnRelocateObject;

        SQLiteConnection connection;
        int stamp_id;
        int type_id;
        string sqlQuery;
        SQLiteDataAdapter adapter_Departure;
        DataTable table_Departure;
        public static string list_id_string = "";
        private Plant plant;

        private enum MainPlantComponents
        {
               REACTOR_DB_ID = 1,
               REGENERATOR_DB_ID = 2
        };
        
        //---------------------------------------------------------------------------------------------------------
        public void Select_Type_Equipment() {
            EquipCombobox.Items.Clear();
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = "SELECT TYPE.name FROM TYPE";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            if (table_Departure.Rows.Count > 0) {
                for (int i = 0; i < table_Departure.Rows.Count; i++)
                    EquipCombobox.Items.Add(table_Departure.Rows[i][0].ToString());
            }
            //EquipCombobox.Items.Add("Выберите тип оборудования");
            //EquipCombobox.Text = "Выберите тип оборудования";
            connection.Close();
        }
        //----------------------------------------------------------------------------------------------------------
        public PlannerForm()
        {
            InitializeComponent();
            EquipCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            StampCombobox.DropDownStyle = ComboBoxStyle.DropDownList; 
            Select_Type_Equipment();
            EquipCombobox.Text = EquipCombobox.Items[0].ToString();
            plant = new Plant();
           //------------------------------------------------------------------------------
        }

        private void PlannerForm_Deactivate(object sender, EventArgs e)
        {
            OnDeactivated?.Invoke();
            CaptureInput?.Invoke();
        }

        private void PlannerForm_Activated(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            OnActivate?.Invoke();
        }

        private void PlannerForm_Load(object sender, EventArgs e)
        {
            OnLoadedEvent?.Invoke("FCC\\Scene\\FCC_MainScene.exe", 8050);
        }

        private void PlannerForm_Resize(object sender, EventArgs e)
        {
            OnResizeEvent?.Invoke(unityParent.Width, unityParent.Height);
            //if (task != null)
            //{
            //    task = new Additional.Task();
            //    task.ShowDialog();
            //    task = null;
            //}
        }

        public IntPtr GetUnityParentHandle()
        {
            return unityParent.Handle;
        }

        private void PlaceObjectOnScene_Click(object sender, EventArgs e)
        {
            RestoreScene();
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = $"SELECT PATH_MODEL FROM STAMP WHERE ID = '{stamp_id}'";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            connection.Close();
            string filename = table_Departure.Rows[0].ItemArray[0].ToString();
            if (filename != "")
            {
                if (type_id == ((int)MainPlantComponents.REACTOR_DB_ID))
                {
                    if (ParamsDataGrid.Rows.Count < 4)
                    {
                        MessageBox.Show("Количество свойств в исходном наборе для данного реактора меньше минимального. Выберите другой реактор. ");
                        return;
                    }
                    double h_value = 0;
                    double diametr = 0;
                    double perfomance = 0;
                    double powerUsage = 0;
                    foreach (DataGridViewRow row in ParamsDataGrid.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals("Высота"))
                        {
                            h_value = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Диаметр"))
                        {
                            diametr = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Производительность"))
                        {
                            perfomance = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Энергопотребление"))
                        {
                            powerUsage = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        if (h_value != 0 && diametr != 0 && perfomance != 0 && powerUsage != 0)
                        {
                            break;
                        }
                    }
                    if (h_value == 0 || diametr == 0 || perfomance == 0 || powerUsage == 0)
                    {
                        MessageBox.Show("У данного реактора отствует или имеет некорректное значение одно(несколько) из основных свойств: высота, диаметр, производительность, энергопотребление. Выберите другой реактор. ");
                        return;
                    }
                }
                else if (type_id == ((int)MainPlantComponents.REGENERATOR_DB_ID))
                {
                    if (ParamsDataGrid.Rows.Count < 3)
                    {
                        MessageBox.Show("Количество свойств в исходном наборе для данного регенератора меньше минимального. Выберите другой регенератор. ");
                        return;
                    }
                    double h_value = 0;
                    double diametr = 0;
                    double powerUsage = 0;
                    foreach (DataGridViewRow row in ParamsDataGrid.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals("Высота"))
                        {
                            h_value = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Диаметр"))
                        {
                            diametr = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Энергопотребление"))
                        {
                            powerUsage = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        if (h_value != 0 && diametr != 0 && powerUsage != 0)
                        {
                            break;
                        }
                    }
                    if (h_value == 0 || diametr == 0 || powerUsage == 0)
                    {
                        MessageBox.Show("У данного регенератора отствует или имеет некорректное значение одно(несколько) из основных свойств: высота, диаметр, энергопотребление. Выберите другой регенератор. ");
                        return;
                    }
                }
                OnLoadObject?.Invoke(filename.Substring(0, filename.IndexOf('.')));
            }
            else {
                MessageBox.Show("Для данной единицы оборудования отсутствует виртуальная модель");
            }
        }

        private void RestoreScene() {
            this.ActiveControl = null;
            OnActivate?.Invoke();
        }

        public void ShowEmbeddingFalliedMessage(string fileName)
        {
            MessageBox.Show("Ошибка запуска подсистемы размещения и компоновки. По указанному пути отсутствуют файлы сцены:\n" + fileName);
        }

        private void PlannerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClosingEvent?.Invoke();
        }

        public Form GetForm()
        {
            return this;
        }
        
        private void EquipCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) {
                ShowEquipList.Enabled = false;
                StampCombobox.Items.Clear();
                connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                connection.Open();
                sqlQuery = $"SELECT ID FROM TYPE WHERE NAME = '{EquipCombobox.Text}'";
                adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                table_Departure = new DataTable();
                adapter_Departure.Fill(table_Departure);
                int type_id = Convert.ToInt32(table_Departure.Rows[0].ItemArray[0]);
                sqlQuery = $"SELECT Name FROM STAMP WHERE ID_TYPE = {type_id}";
                adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
                table_Departure = new DataTable();
                adapter_Departure.Fill(table_Departure);
                if (table_Departure.Rows.Count > 0) {
                    for (int i = 0; i < table_Departure.Rows.Count; i++)
                        StampCombobox.Items.Add(table_Departure.Rows[i][0].ToString());
                    StampCombobox.Text = table_Departure.Rows[0][0].ToString();
                }
                connection.Close();
            }
            else {
                ShowEquipList.Enabled = true;
                StampCombobox.Items.Clear();
                SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
                connection.Open();
                string sqlQuery = "SELECT STAMP.name FROM STAMP INNER JOIN TYPE on TYPE.id = STAMP.id_type" +
                       " WHERE TYPE.name = \"" + EquipCombobox.Text + "\" AND STAMP.id in (" + list_id_string + ")";
                SQLiteDataAdapter adapter_Departure1 = new SQLiteDataAdapter(sqlQuery, connection);
                DataTable Table_Departure1 = new DataTable();
                adapter_Departure1.Fill(Table_Departure1);
                if (Table_Departure1.Rows.Count > 0) {
                    for (int i = 0; i < Table_Departure1.Rows.Count; i++) {
                        StampCombobox.Items.Add(Table_Departure1.Rows[i][0].ToString());
                    }
                    StampCombobox.Text = Table_Departure1.Rows[0][0].ToString();
                }
                connection.Close();
            }
        }

        private void StampCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = $"SELECT ID FROM STAMP WHERE NAME = '{StampCombobox.Text}'";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            stamp_id = Convert.ToInt32(table_Departure.Rows[0].ItemArray[0]);
            type_id = GetTypeById(stamp_id);
            sqlQuery = $"SELECT Name as 'Параметр', NP.Number as 'Значение', Dimension as 'Единица измерения' FROM PARAMETR_EQUIPMENT AS P INNER JOIN NUM_PARAM_EQUIPMENT as NP On NP.Id_Parametr = P.Id WHERE NP.ID_STAMP = {stamp_id}";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            ParamsDataGrid.DataSource = table_Departure;
            connection.Close();
            
        }

        private int GetTypeById(int id) {
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = $"SELECT ID_TYPE FROM STAMP WHERE ID = {id}";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            connection.Close();
            return Convert.ToInt32(table_Departure.Rows[0].ItemArray[0]);
        }

        public void ParseMessage(string message)
        {
            if (message[0] == 'p')
            {
                this.ObjectsOnScene.BeginInvoke((MethodInvoker)(() => this.ObjectsOnScene.Rows.Add("Объект" + message.Substring(2), EquipCombobox.Text, StampCombobox.Text, message.Substring(2))));
                plant.equipment.Add(Convert.ToInt32(message.Substring(2)), stamp_id);
                if (type_id == ((int)MainPlantComponents.REACTOR_DB_ID))
                {
                    double h_value = 0;
                    double diametr = 0;
                    double perfomance = 0;
                    foreach (DataGridViewRow row in ParamsDataGrid.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals("Высота"))
                        {
                            h_value = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Диаметр"))
                        {
                            diametr = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Производительность"))
                        {
                            perfomance = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        if (h_value != 0 && diametr != 0 && perfomance != 0)
                        {
                            break;
                        }
                    }
                    plant.reactor = new Reactor(h_value, diametr, stamp_id, perfomance);
                    Perfomance.BeginInvoke((MethodInvoker)(() => Perfomance.Text = perfomance.ToString()));
                }
                else if (type_id == ((int)MainPlantComponents.REGENERATOR_DB_ID))
                {
                    double h_value = 0;
                    double diametr = 0;
                    foreach (DataGridViewRow row in ParamsDataGrid.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals("Высота"))
                        {
                            h_value = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        else if (row.Cells[0].Value.ToString().Equals("Диаметр"))
                        {
                            diametr = Convert.ToDouble(row.Cells[1].Value.ToString().Replace('.', ','));
                        }
                        if (h_value != 0 && diametr != 0)
                        {
                            break;
                        }
                    }
                    plant.regenerator = new Regenerator(h_value, diametr, stamp_id);

                }
                plant.IncreaseUsage(GetPowerUsageById(stamp_id));
                PowerUsage.BeginInvoke((MethodInvoker)(() => PowerUsage.Text = plant.powerUsage.ToString()));
            }
            else if (message[0] == 'a')
            {
                RestoreScene();
            }
            else if (message[0] == 's')
            {
                PlantArea.BeginInvoke((MethodInvoker)(() => PlantArea.Text = message.Substring(2)));
            }
            else if (message[0] == 'h')
            {
                PlantHeight.BeginInvoke((MethodInvoker)(() => PlantHeight.Text = message.Substring(2)));
            }
        }

        private double GetPowerUsageById(int id) {
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = $"SELECT NP.Number as 'Значение' FROM PARAMETR_EQUIPMENT AS P INNER JOIN NUM_PARAM_EQUIPMENT as NP On NP.Id_Parametr = P.Id WHERE NP.ID_STAMP = {id} AND P.NAME = 'Энергопотребление'";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            connection.Close();
            if (table_Departure.Rows.Count > 0)
            {
                return Convert.ToDouble(table_Departure.Rows[0].ItemArray[0].ToString().Replace('.', ','));
            }
            else
                return 0;
        }

        private void SelectObj_Click(object sender, EventArgs e)
        {
            RestoreScene();
            OnSelectObject?.Invoke(Convert.ToInt32(ObjectsOnScene.SelectedRows[0].Cells[3].Value));
        }

        private void DeleteObj_Click(object sender, EventArgs e) {
            RestoreScene();
            int del_id = Convert.ToInt32(ObjectsOnScene.SelectedRows[0].Cells[3].Value);
            OnDeleteObject?.Invoke(del_id);
            plant.DecreaseUsage(GetPowerUsageById(plant.ReturnDatabaseIdByMountIndex(del_id)));
            ObjectsOnScene.Rows.Remove(ObjectsOnScene.SelectedRows[0]);
            PowerUsage.Text = plant.powerUsage.ToString();
            if (GetTypeById(plant.ReturnDatabaseIdByMountIndex(del_id)) == ((int)MainPlantComponents.REACTOR_DB_ID)) {
                plant.reactor = null;
                Perfomance.Text = "0";
            }
            else if (GetTypeById(plant.ReturnDatabaseIdByMountIndex(del_id)) == ((int)MainPlantComponents.REGENERATOR_DB_ID))
            {
                plant.regenerator = null;
            }
        }


        private void ControlFocused(object sender, EventArgs e)
        {
            CaptureInput?.Invoke();
        }

        private void label9_Click(object sender, EventArgs e) {
            if (checkBox1.Checked) {
                checkBox1.Checked = false;                
            }
            else {             
                checkBox1.Checked = true;
            }     
        }
        private void set_paramer_equipment() {
            StampCombobox.Items.Clear();
            list_id_string = "";
            List<int> list_id = new List<int>();
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.id, TYPE.name, STAMP.name, PARAMETR_EQUIPMENT.name, NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                              " WHERE STAMP.id in ("+
                              " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                              " WHERE STAMP.id in ("+
                              " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" + //
                              " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +                            //
                              " WHERE STAMP.id in ("+                                                                                                                //
                              " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                              " WHERE STAMP.id NOT in ("+
                              " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                              " WHERE(PARAMETR_EQUIPMENT.name = \"Производительность\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) < " + Additional.Task.P + ")"+
                              " OR(PARAMETR_EQUIPMENT.name = \"Энергопотребление\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) > " + Additional.Task.E + ")))" +
                              " AND(TYPE.name = \"Реактор\" AND PARAMETR_EQUIPMENT.name = \"Концентрация получаемого бензина\" AND CAST(NUM_PARAM_EQUIPMENT.number as real) > " + Additional.Task.Stamp + "))" + //
                              " AND(TYPE.name = \"Реактор\" AND PARAMETR_EQUIPMENT.name = \"Тип применяемого сырья\" AND NUM_PARAM_EQUIPMENT.number = \"" + Additional.Task.Raw + "\"))" +
                              " AND(TYPE.name = \"Реактор\" AND PARAMETR_EQUIPMENT.name = \"Тип применяемого катализатора\" AND NUM_PARAM_EQUIPMENT.number = \"" + Additional.Task.Catalyst + "\")";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    list_id.Add(Convert.ToInt32(Table_Departure.Rows[i][0]));
                   // StampCombobox.Items.Add(Table_Departure.Rows[i][2].ToString());
                }
                //StampCombobox.Text = Table_Departure.Rows[0][2].ToString();
            }
            //connection.Open();
            sqlQuery = "SELECT STAMP.id, TYPE.name, STAMP.name, PARAMETR_EQUIPMENT.name, NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE STAMP.id in (" +
                       " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE STAMP.id NOT in (" +
                       " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE(PARAMETR_EQUIPMENT.name = \"Производительность\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) < " + Additional.Task.P + ")" +
                       " OR(PARAMETR_EQUIPMENT.name = \"Энергопотребление\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) > " + Additional.Task.E + ")))" +
                       " AND(TYPE.name = \"Регенератор\" AND PARAMETR_EQUIPMENT.name = \"Тип применяемого катализатора\" AND NUM_PARAM_EQUIPMENT.number = \"" + Additional.Task.Catalyst + "\")";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    list_id.Add(Convert.ToInt32(Table_Departure.Rows[i][0]));
                    //StampCombobox.Items.Add(Table_Departure.Rows[i][2].ToString());
                }
                //StampCombobox.Text = Table_Departure.Rows[0][2].ToString();
            }
            sqlQuery = "SELECT STAMP.id, TYPE.name, STAMP.name, PARAMETR_EQUIPMENT.name, NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE STAMP.id in (" +
                       " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE STAMP.id in (" +
                       " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE STAMP.id NOT in (" +
                       " SELECT STAMP.id FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                       " INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id INNER JOIN TYPE on STAMP.id_type = TYPE.id" +
                       " WHERE(PARAMETR_EQUIPMENT.name = \"Производительность\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) < " + Additional.Task.P + ")" +
                       " OR(PARAMETR_EQUIPMENT.name = \"Энергопотребление\" AND CAST(NUM_PARAM_EQUIPMENT.number as integer) > " + Additional.Task.E + ")))" +
                       " AND(TYPE.name != \"Регенератор\"))" +
                       " AND(TYPE.name != \"Реактор\")";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    list_id.Add(Convert.ToInt32(Table_Departure.Rows[i][0]));
                    //StampCombobox.Items.Add(Table_Departure.Rows[i][2].ToString());
                }
                //StampCombobox.Text = Table_Departure.Rows[0][2].ToString();
            }
            
            if (list_id.Count > 0) { 
                list_id_string = "";
                for (int i = 0; i < list_id.Count - 1; i++)
                {
                    list_id_string += list_id[i].ToString() + ", ";
                }
                list_id_string += list_id[list_id.Count - 1].ToString();

                EquipCombobox_SelectedIndexChanged(Owner, EventArgs.Empty);
            }
            else {
                ShowEquipList.Enabled = false;
            }
            

            connection.Close();
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox1.Checked) {
                Additional.Task form = new Additional.Task();
                form.Owner = this;
                form.ShowDialog();
                if (DialogResult.Cancel == form.DialogResult) {
                    checkBox1.Checked = false;
                    return;
                }
                if (DialogResult.OK == form.DialogResult) {
                   set_paramer_equipment();
                    EquipCombobox.Enabled = true;
                    StampCombobox.Enabled = true;
                    panel2.Enabled = true;
                    PlaceObjectOnScene.Enabled = true;
                    ParamsDataGrid.Enabled = true;
                    return;
                } 
            }
            else {
                ShowEquipList.Enabled = false;
                EquipCombobox_SelectedIndexChanged(Owner, EventArgs.Empty);
            }
        }

        private void RelocateObj_Click(object sender, EventArgs e)
        {
            RestoreScene();
            OnRelocateObject?.Invoke(Convert.ToInt32(ObjectsOnScene.SelectedRows[0].Cells[3].Value));
        }

        private void RunMathModule_Click(object sender, EventArgs e)
        {
            CaptureInput?.Invoke();
            MathModelCalculation form = new MathModelCalculation(plant);
            form.Owner = this;
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) {
            Additional.Found_Equipment form = new Additional.Found_Equipment();
            form.Owner = this;
            form.ShowDialog();
            if (DialogResult.Cancel == form.DialogResult) {               
                return;
            }
            if (DialogResult.OK == form.DialogResult) {
                return;
            }
        }

        private void ParamsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            
        }

        private void CreateReport_Click(object sender, EventArgs e)
        {
            CaptureInput?.Invoke();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.XLSX)|*.xlsx";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = "НовыйПаспортОбъекта";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                var wb = new XLWorkbook();
                
                //Adding a worksheet
                var ws = wb.Worksheets.Add(Name = "Структура установки");
                
                //Adding text
                //Title
                ws.Cell("B1").Value = "Список оборудования";
                ws.Cell("B2").Value = "Тип";
                ws.Cell("C2").Value = "Название";
                int iterator = 3;
                foreach (DataGridViewRow row in ObjectsOnScene.Rows) {
                    ws.Cell($"B{iterator}").Value = row.Cells[1].Value;
                    ws.Cell($"C{iterator}").Value = row.Cells[2].Value;
                    iterator += 1;
                }
                ws.Cell("E1").Value = "Характеристики установки";
                ws.Cell("E2").Value = "Общая площадь установки (м²):";
                ws.Cell("F2").Value = Convert.ToDouble(PlantArea.Text);
                ws.Cell("E3").Value = "Высота (м): ";
                ws.Cell("F3").Value = Convert.ToDouble(PlantHeight.Text);
                ws.Cell("E4").Value = "Энергопотребление (кВт·ч/т): ";
                ws.Cell("F4").Value = Convert.ToDouble(PowerUsage.Text);
                ws.Cell("E5").Value = "Производительность (т/год): ";
                ws.Cell("F5").Value = Convert.ToDouble(Perfomance.Text);

                if (CrackingProcess.workbook != null)
                {
                    IXLWorksheet we;
                    CrackingProcess.workbook.Worksheets.TryGetWorksheet("Результаты поверочного расчета", out we);
                    wb.AddWorksheet(we);
                }
                else {
                    var we = wb.Worksheets.Add(Name = "Результаты поверочного расчета");
                    we.Cell("B2").Value = "Поверочный расчет не производился";
                }





                wb.SaveAs(saveFileDialog.FileName);

            }


        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ObjectsOnScene_SelectionChanged(object sender, EventArgs e)
        {
            if (ObjectsOnScene.SelectedRows.Count > 0)
            {
                RelocateObj.Enabled = true;
                SelectObj.Enabled = true;
                DeleteObj.Enabled = true;
            }
            else {
                RelocateObj.Enabled = false;
                SelectObj.Enabled = false;
                DeleteObj.Enabled = false;
            }
        }
    }
}
