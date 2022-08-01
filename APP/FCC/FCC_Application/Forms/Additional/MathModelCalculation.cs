using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using FCC_Application.Models;
using System.Windows.Forms;
using System.Data.SQLite;
using FCC_Application.Utills;
using OxyPlot;
using OxyPlot.Series;
using System.Diagnostics;
using ClosedXML.Excel;

namespace FCC_Application.Forms.Additional
{
    public partial class MathModelCalculation : Form
    {
        PlotModel gasModel = new PlotModel { Title = "Концентрация бензина" };
        PlotModel gasoilModel = new PlotModel { Title = "Концентрация вакуумного газойля" };
        PlotModel tempModel = new PlotModel { Title = "Температура смеси" };
        LineSeries gasSeries = new LineSeries();
        LineSeries gasoilSeries = new LineSeries();
        LineSeries tempSeries = new LineSeries();
        int n = 1;
        private string reactor_name_by_struct = "";
        private string regenerator_name_by_struct = "";

        public MathModelCalculation(Plant plant)
        {
            InitializeComponent();
            tabPage2.Enabled = false;
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.name, STAMP.Id FROM STAMP INNER JOIN TYPE on STAMP.id_type = TYPE.id WHERE TYPE.name = \"Регенератор\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            string defaultSelected = "";
            if (Table_Departure.Rows.Count > 0)
            {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    comboBox1.Items.Add(Table_Departure.Rows[i][0].ToString());
                    if (Convert.ToInt32(Table_Departure.Rows[i][1]) == plant?.regenerator?.id) {
                        defaultSelected = Table_Departure.Rows[i][0].ToString();
                    }
                }
                comboBox1.Text = Table_Departure.Rows[0][0].ToString();
            }
            if (plant?.regenerator != null) {
                comboBox1.SelectedIndex = comboBox1.FindStringExact(defaultSelected);
                regenerator_name_by_struct = comboBox1.Text;
                comboBox1.BackColor = Color.White;
            }
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = "SELECT CATALYST.name FROM CATALYST";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure1 = new DataTable();
            adapter_Departure.Fill(Table_Departure1);
            if (Table_Departure1.Rows.Count > 0)
            {
                for (int i = 0; i < Table_Departure1.Rows.Count; i++)
                    comboBox2.Items.Add(Table_Departure1.Rows[i][0].ToString());
                comboBox2.Text = Table_Departure1.Rows[0][0].ToString();
            }
            connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            sqlQuery = "SELECT RAW.name FROM RAW";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure2 = new DataTable();
            adapter_Departure.Fill(Table_Departure2);
            if (Table_Departure2.Rows.Count > 0)
            {
                for (int i = 0; i < Table_Departure2.Rows.Count; i++)
                    comboBox3.Items.Add(Table_Departure2.Rows[i][0].ToString());
                comboBox3.Text = Table_Departure2.Rows[0][0].ToString();
            }
            connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            sqlQuery = "SELECT STAMP.name, STAMP.id FROM STAMP INNER JOIN TYPE on STAMP.id_type = TYPE.id WHERE TYPE.name = \"Реактор\"";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure3 = new DataTable();
            adapter_Departure.Fill(Table_Departure3);
            if (Table_Departure3.Rows.Count > 0)
            {
                for (int i = 0; i < Table_Departure3.Rows.Count; i++)
                {
                    comboBox4.Items.Add(Table_Departure3.Rows[i][0].ToString());
                    if (Convert.ToInt32(Table_Departure3.Rows[i][1]) == plant?.reactor?.id)
                    {
                        defaultSelected = Table_Departure3.Rows[i][0].ToString();
                    }
                }
                comboBox4.Text = Table_Departure3.Rows[0][0].ToString();
            }
            if (plant?.reactor != null) {
                comboBox4.SelectedIndex = comboBox4.FindStringExact(defaultSelected);
                reactor_name_by_struct = comboBox4.Text;
                comboBox4.BackColor = Color.White;
            }
            else {
                comboBox4.SelectedIndex = 1;
            }
            connection.Close();
            ChangeStatus();
            textBox38.Text = Math.Round(set_W_regenerator(Convert.ToDouble(textBox24.Text), Convert.ToDouble(textBox20.Text), Convert.ToDouble(textBox2.Text)), 1).ToString();
            //textBox8.Text = number_tech_parametr("Концентрация кокса в регенерируемом катализаторе");
            //textBox10.Text = number_tech_parametr("Массовый расход катализатора");
            //textBox12.Text = number_tech_parametr("Температура сырья на входе в реактор");
            //textBox9.Text = number_tech_parametr("Концентрация пара в сырье");
            //textBox11.Text = number_tech_parametr("Массовый расход сырья");

            //textBox39.Text = number_tech_parametr("Концентрация кокса на выходе из реактора");
            //textBox41.Text = number_tech_parametr("Вес катализатора в сепараторе");
            //textBox40.Text = number_tech_parametr("Расход регенерируемого катализатора");

            //textBox22.Text = number_tech_parametr("Концентрация кокса на входе в регенератор");
            //textBox28.Text = number_tech_parametr("Концентрация кислорода на входе в регенератор");
            //textBox29.Text = number_tech_parametr("Входная температура катализатора в регенераторе");
            //textBox37.Text = number_tech_parametr("Температура воздуха в регенераторе");
            //textBox23.Text = number_tech_parametr("Расход катализатора в регенераторе");
            //textBox27.Text = number_tech_parametr("Расход воздуха в регенераторе");
            //textBox30.Text = number_tech_parametr("Масса кислорода в регенераторе");
            //textBox38.Text = number_tech_parametr("Масса катализатора в регенераторе");
        }
        /*private string number_tech_parametr(string parametr)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT TECH_PARAM.number FROM TECH_PARAMETR" +
                              " WHERE TECH_PARAMETR.name = \"" + parametr + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);

            connection.Close();
            if (Table_Departure.Rows.Count > 0)
                return Table_Departure.Rows[0][0].ToString();
            else
                return "";
        }*/

        /// <summary>
        /// Структура регламента
        /// </summary>
        struct Reglament {
            public string stamp;
            public string catalyst;
            public string parametr;
            public double min;
            public double max;
        }

        /// <summary>
        /// Структура выходных значений регламента для MessageBox
        /// </summary>
        public struct Return_reglament {
            public double min;
            public double max;
            public int error;
        }
        
        /// <summary>
        /// Проверка регламентов
        /// </summary>
        /// <returns></returns>
        public Return_reglament check_reglament() {
            List<Reglament> list_id = new List<Reglament>();
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT STAMP.name, CATALYST.name, PARAMETR_TECH_REG.name, TECH_REG.min_number, TECH_REG.max_number FROM TECH_REG INNER JOIN STAMP on STAMP.id = TECH_REG.id_stamp"+
                              " INNER JOIN CATALYST on CATALYST.id = TECH_REG.id_catalyst INNER JOIN PARAMETR_TECH_REG"+
                              " on PARAMETR_TECH_REG.id = TECH_REG.id_parametr";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);
            if (Table_Departure.Rows.Count > 0) {
                for (int i = 0; i < Table_Departure.Rows.Count; i++) {
                    Reglament reg;
                    reg.stamp = Table_Departure.Rows[i][0].ToString();
                    reg.catalyst = Table_Departure.Rows[i][1].ToString();
                    reg.parametr = Table_Departure.Rows[i][2].ToString();
                    reg.min = Convert.ToDouble(Table_Departure.Rows[i][3].ToString().Replace('.', ','));
                    reg.max = Convert.ToDouble(Table_Departure.Rows[i][4].ToString().Replace('.', ','));
                    list_id.Add(reg);            
                }         
            }
            connection.Close();

            Return_reglament return_reg;
            double reg_num = Convert.ToDouble(textBox11.Text.Replace('.', ','));
            for(int i = 0; i < list_id.Count; i++) {
                if(list_id[i].stamp == comboBox4.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Массовый расход сырья") {
                    if(reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox11.BackColor = Color.FromArgb(255,192,192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count-1) {
                    textBox11.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox10.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox4.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Массовый расход катализатора") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox10.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox10.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox9.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox4.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Концентрация греющего пара в сырье") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox9.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox9.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox12.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox4.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Температура сырья на входе в реактор") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox12.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox12.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox8.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox4.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Концентрация кокса в свежем катализаторе") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox8.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox8.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox27.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox1.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Массовый расход воздуха в регенераторе") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox27.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox27.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox37.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox1.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Температура воздуха в регенераторе") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox37.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox37.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox28.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox1.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Концентрация кислорода на входе в регенератор") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox28.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox28.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }
            reg_num = Convert.ToDouble(textBox30.Text.Replace('.', ','));
            for (int i = 0; i < list_id.Count; i++) {
                if (list_id[i].stamp == comboBox1.Text && list_id[i].catalyst == comboBox2.Text && list_id[i].parametr == "Количество воздуха в регенераторе") {
                    if (reg_num >= list_id[i].min && reg_num <= list_id[i].max) {
                        break;
                    }
                    else {
                        textBox30.BackColor = Color.FromArgb(255, 192, 192);
                        return_reg.min = list_id[i].min;
                        return_reg.max = list_id[i].max;
                        return_reg.error = -1;
                        return return_reg;
                    }
                }
                if (i == list_id.Count - 1) {
                    textBox30.BackColor = Color.FromArgb(255, 192, 192);
                    return_reg.min = list_id[i].min;
                    return_reg.max = list_id[i].max;
                    return_reg.error = 0;
                    return return_reg;
                }
            }



            return_reg.min = 0;
            return_reg.max = 0;
            return_reg.error = 1;
            return return_reg;
        }

        /// <summary>
        /// Проверка значений на недопустимые символы
        /// </summary>
        /// <returns></returns>
        public bool check_number() {
            try {
                Convert.ToDouble(Eps.Text);
            }
            catch {
                Eps.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(MaxIterCount.Text);
            }
            catch {
                MaxIterCount.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox1.Text);
            }
            catch {
                textBox1.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox3.Text);
            }
            catch {
                textBox3.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox16.Text);
            }
            catch {
                textBox16.BackColor = Color.FromArgb(255, 192, 192);
                return false; 
            }
            try {
                Convert.ToDouble(textBox4.Text);
            }
            catch {
                textBox4.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox5.Text);
            }
            catch {
                textBox5.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox6.Text);
            }
            catch {
                textBox6.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox8.Text);
            }
            catch {
                textBox8.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(StepValue.Text);
            }
            catch {
                StepValue.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox14.Text);
            }
            catch {
                textBox14.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox13.Text);
            }
            catch {
                textBox13.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox11.Text);
            }
            catch {
                textBox11.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox10.Text);
            }
            catch {
                textBox10.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox15.Text);
            }
            catch {
                textBox15.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox17.Text);
            }
            catch {
                textBox17.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox18.Text);
            }
            catch {
                textBox18.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox9.Text);
            }
            catch {
                textBox9.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox19.Text);
            }
            catch {
                textBox19.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox7.Text);
            }
            catch {
                textBox7.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox12.Text);
            }
            catch {
                textBox12.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }

            try {
                Convert.ToDouble(textBox27.Text);
            }
            catch {
                textBox27.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox10.Text);
            }
            catch {
                textBox10.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox42.Text);
            }
            catch {
                textBox42.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox33.Text);
            }
            catch {
                textBox33.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox34.Text);
            }
            catch {
                textBox34.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox36.Text);
            }
            catch {
                textBox36.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox35.Text);
            }
            catch {
                textBox35.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox38.Text);
            }
            catch {
                textBox38.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox30.Text);
            }
            catch {
                textBox30.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox28.Text);
            }
            catch {
                textBox28.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox31.Text);
            }
            catch {
                textBox31.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox6.Text);
            }
            catch {
                textBox6.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox32.Text);
            }
            catch {
                textBox32.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox37.Text);
            }
            catch {
                textBox37.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox25.Text);
            }
            catch {
                textBox25.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox39.Text);
            }
            catch {
                textBox39.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox26.Text);
            }
            catch {
                textBox26.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox2.Text);
            }
            catch {
                textBox2.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox21.Text);
            }
            catch {
                textBox21.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            try {
                Convert.ToDouble(textBox20.Text);
            }
            catch {
                textBox20.BackColor = Color.FromArgb(255, 192, 192);
                return false;
            }
            return true;

        }


        private void CalculateFirstCycle_Click(object sender, EventArgs e)
        {
            if (!check_number()) {
                MessageBox.Show("Проверьте правильность заполнения данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Return_reglament return_reg = check_reglament();
            if (return_reg.error==-1) {
                MessageBox.Show("Значение не соответствуют диапазону регламентов проведения процесса " + "[" + return_reg.min +"; "+ return_reg.max+"]", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (return_reg.error == 0) {
                MessageBox.Show("Значение регламента для данного оборудования и катализатора отсутствует в базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            CycleResults.Rows.Clear();
            double eps = Convert.ToDouble(Eps.Text);
            int q = Convert.ToInt32(MaxIterCount.Text);
            Reactor reactor = new Reactor(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox3.Text),0,0);
            CrackingProcess.reactor = reactor;
            CrackingProcess.alpha = Convert.ToDouble(textBox16.Text);
            CrackingProcess.cf = Convert.ToDouble(textBox4.Text);
            CrackingProcess.cp = Convert.ToDouble(textBox5.Text);
            CrackingProcess.Cps = Convert.ToDouble(textBox6.Text);
            CrackingProcess.Crc = Convert.ToDouble(textBox8.Text);
            CrackingProcess.delta = Convert.ToDouble(StepValue.Text);
            CrackingProcess.Ef = Convert.ToDouble(textBox14.Text);
            CrackingProcess.Eg = Convert.ToDouble(textBox13.Text);
            CrackingProcess.Ff = Convert.ToDouble(textBox11.Text);
            CrackingProcess.Fs = Convert.ToDouble(textBox10.Text);
            CrackingProcess.Hf = Convert.ToDouble(textBox15.Text);
            CrackingProcess.k0f = Convert.ToDouble(textBox17.Text);
            CrackingProcess.k0g = Convert.ToDouble(textBox18.Text);
            CrackingProcess.lambda = Convert.ToDouble(textBox9.Text);
            CrackingProcess.m_factor = Convert.ToDouble(textBox19.Text);
            CrackingProcess.ro = Convert.ToDouble(textBox7.Text);
            CrackingProcess.T0 = Convert.ToDouble(textBox12.Text);
            CrackingProcess.k03 = Convert.ToDouble(textBox21.Text);

            Regenerator regenerator = new Regenerator(Convert.ToDouble(textBox2.Text), Convert.ToDouble(textBox20.Text), 0);
            RegenerationProcess.regenerator = regenerator;
            RegenerationProcess.Fa = Convert.ToDouble(textBox27.Text);
            RegenerationProcess.Fs = Convert.ToDouble(textBox10.Text);
            RegenerationProcess.k = Convert.ToDouble(textBox42.Text);
            RegenerationProcess.Ma = Convert.ToDouble(textBox33.Text);
            RegenerationProcess.Mc = Convert.ToDouble(textBox34.Text);
            RegenerationProcess.n = Convert.ToDouble(textBox36.Text);
            RegenerationProcess.sigma = Convert.ToDouble(textBox35.Text);
            RegenerationProcess.W = Convert.ToDouble(textBox38.Text);
            RegenerationProcess.Wa = Convert.ToDouble(textBox30.Text);
            RegenerationProcess.yin = Convert.ToDouble(textBox28.Text);
            RegenerationProcess.Cpa = Convert.ToDouble(textBox31.Text);
            RegenerationProcess.Cps = Convert.ToDouble(textBox6.Text);
            RegenerationProcess.dH = Convert.ToDouble(textBox32.Text);
            RegenerationProcess.ta = Convert.ToDouble(textBox37.Text);

            CrackingProcess.kc = Convert.ToDouble(textBox25.Text);
            CrackingProcess.Ecf = Convert.ToDouble(textBox39.Text);
            CrackingProcess.N = Convert.ToDouble(textBox26.Text);

            

            bool IsSolve = false;
            double sepValue = 0;
            double eps_cur = 0;
            int i;
            for (i = 1; i < q-1; i++)
            {
                double curStep = CrackingProcess.delta;
                List<double> values = new List<double>();
                List<double> values_half = new List<double>();
                CrackingProcess.Calculate(curStep);
                values = CrackingProcess.yg;
                sepValue = CrackingProcess.Calculate(curStep / 2);
                values_half = CrackingProcess.yg;
                double sum = 0;
                for (int j = 1; j < values.Count; j++)
                {
                    sum = sum + Math.Pow(((values_half[2 * j] - values[j]))/values_half[2*j], 2);
                }
                sum = Math.Sqrt(sum / (values.Count-1)) * 100;
                eps_cur = (sum / 15);
                if (eps_cur < eps)
                {
                    IsSolve = true;
                    break;
                }
                else
                {
                    CrackingProcess.delta = curStep / 2;
                }
            }
            if (!IsSolve)
            {
                MessageBox.Show("Решение с погрешностью, не превосходящей заданную, не найдено.", "Не удалось найти решение!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            


            n = 1;
            RegenerationProcess.tin = CrackingProcess.temperature[CrackingProcess.temperature.Count - 1].Y;
            TrueStep.Text = CrackingProcess.delta.ToString();
            eps_value.Text = Math.Round(eps_cur,GetDecimalDigitsCount(eps)+1).ToString();
            q_count.Text = i.ToString();
            CrackingProcess.Crc = RegenerationProcess.Calculate(sepValue);
            GasOutReactorValue.Text = Math.Round(CrackingProcess.concentration[CrackingProcess.concentration.Count - 1].Y,3).ToString();
            GasoilOutReactorValue.Text = Math.Round(CrackingProcess.g_concentration[CrackingProcess.g_concentration.Count - 1].Y,3).ToString();
            TempOutReactorValue.Text = Math.Round(CrackingProcess.temperature[CrackingProcess.temperature.Count - 1].Y,2).ToString();
            CokeOutSepValue.Text = Math.Round(sepValue, 4).ToString();
            CokeOutRegValue.Text = Math.Round(CrackingProcess.Crc, 4).ToString();
            TempOutRegValue.Text = Math.Round(RegenerationProcess.GetOutTemperature(CrackingProcess.Crc), 2).ToString();
            stopwatch.Stop();
            label35_time_is_out.Text = stopwatch.ElapsedMilliseconds.ToString();
            gasModel = new PlotModel { Title = "Изменение концентрации бензина по высоте реактора" };
            gasModel.PlotType = PlotType.XY;
            OxyPlot.Axes.LinearAxis GasAxisY = new OxyPlot.Axes.LinearAxis();
            GasAxisY.Title = "Концентрация бензина, масс. доли";
            OxyPlot.Axes.LinearAxis GasAxisX = new OxyPlot.Axes.LinearAxis();
            GasAxisX.Position = OxyPlot.Axes.AxisPosition.Bottom;
            GasAxisX.Title = "Высота реактора, м";
            gasSeries = new LineSeries();
            gasSeries.ItemsSource = CrackingProcess.concentration;
            gasSeries.Color = OxyColor.FromRgb(51, 51, 255);
            gasSeries.YAxisKey = GasAxisY.Key;
            gasSeries.XAxisKey = GasAxisX.Key;
            gasModel.Series.Add(gasSeries);
            gasModel.Axes.Add(GasAxisY);
            gasModel.Axes.Add(GasAxisX);
            this.GasPlot.Model = gasModel;
            gasoilModel = new PlotModel { Title = "Изменение концентрации сырья по высоте реактора" };
            gasoilModel.PlotType = PlotType.XY;
            OxyPlot.Axes.LinearAxis GasoilAxisY = new OxyPlot.Axes.LinearAxis();
            GasoilAxisY.Title = "Концентрация сырья, масс. доли";
            OxyPlot.Axes.LinearAxis GasoilAxisX = new OxyPlot.Axes.LinearAxis();
            GasoilAxisX.Position = OxyPlot.Axes.AxisPosition.Bottom;
            GasoilAxisX.Title = "Высота реактора, м";
            gasoilModel.TitleFontSize = 16;
            gasoilModel.TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinView;
            gasoilSeries = new LineSeries();
            gasoilSeries.ItemsSource = CrackingProcess.g_concentration;
            gasoilSeries.Color = OxyColor.FromRgb(0, 153, 51);
            gasoilSeries.YAxisKey = GasoilAxisY.Key;
            gasoilSeries.XAxisKey = GasoilAxisX.Key;
            gasoilModel.Series.Add(gasoilSeries);
            gasoilModel.Axes.Add(GasoilAxisY);
            gasoilModel.Axes.Add(GasoilAxisX);
            this.GasoilPlot.Model = gasoilModel;
            tempModel = new PlotModel { Title = "Изменение температуры смеси по высоте реактора" };
            tempModel.PlotType = PlotType.XY;
            OxyPlot.Axes.LinearAxis TempAxisY = new OxyPlot.Axes.LinearAxis();
            TempAxisY.Title = "Температура смеси, К";
            OxyPlot.Axes.LinearAxis TempAxisX = new OxyPlot.Axes.LinearAxis();
            TempAxisX.Position = OxyPlot.Axes.AxisPosition.Bottom;
            TempAxisX.Title = "Высота реактора, м";
            tempSeries = new LineSeries();
            tempSeries.ItemsSource = CrackingProcess.temperature;
            tempSeries.YAxisKey = TempAxisY.Key;
            tempSeries.XAxisKey = TempAxisX.Key;
            tempSeries.Color = OxyColor.FromRgb(255, 51, 0);
            tempModel.Series.Add(tempSeries);
            tempModel.Axes.Add(TempAxisY);
            tempModel.Axes.Add(TempAxisX);
            this.TempPlot.Model = tempModel;
            CycleResults.Rows.Add();
            CycleResults.Rows[n - 1].Cells[0].Value = n;
            CycleResults.Rows[n - 1].Cells[1].Value = GasOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[2].Value = GasoilOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[3].Value = TempOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[4].Value = CokeOutSepValue.Text;
            CycleResults.Rows[n - 1].Cells[5].Value = CokeOutRegValue.Text;
            CycleResults.Rows[n - 1].Cells[6].Value = TempOutRegValue.Text;
            tabPage2.Enabled = true;
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            FillExcelSheetMainInfo();
        }

        private int GetDecimalDigitsCount(double value)
        {
            string[] str = value.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." }).Split('.');
            return str.Length == 2 ? str[1].Length : 0;
        }
        private void NewCycle_Click(object sender, EventArgs e)
        {
            if (PlannerTask.concentration_limit > CrackingProcess.yg[CrackingProcess.yg.Count - 1]) {
                MessageBox.Show("Значение концентрации бензина на выходе меньше задания.", "Предельное значение концентрации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((1 - CrackingProcess.m_factor * CrackingProcess.Crc) < 0)
            {
                MessageBox.Show("Активность катализатора слишком низкая. Реакция не произойдет.", "Критическая активность катализатора", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            n += 1;
            double sepValue = CrackingProcess.Calculate(CrackingProcess.delta);
            RegenerationProcess.tin = CrackingProcess.temperature[CrackingProcess.temperature.Count - 1].Y;
            CrackingProcess.Crc = RegenerationProcess.Calculate(sepValue);
            GasOutReactorValue.Text = Math.Round(CrackingProcess.concentration[CrackingProcess.concentration.Count - 1].Y, 3).ToString();
            GasoilOutReactorValue.Text = Math.Round(CrackingProcess.g_concentration[CrackingProcess.g_concentration.Count - 1].Y, 3).ToString();
            TempOutReactorValue.Text = Math.Round(CrackingProcess.temperature[CrackingProcess.temperature.Count - 1].Y, 2).ToString();
            CokeOutSepValue.Text = Math.Round(sepValue, 4).ToString();
            CokeOutRegValue.Text = Math.Round(CrackingProcess.Crc, 4).ToString();
            TempOutRegValue.Text = Math.Round(RegenerationProcess.GetOutTemperature(CrackingProcess.Crc), 2).ToString();
            gasSeries.ItemsSource = CrackingProcess.concentration;
            gasModel.Series.Clear();
            gasModel.Series.Add(gasSeries);
            GasPlot.InvalidatePlot(true);
            CycleResults.Rows.Add();
            CycleResults.Rows[n - 1].Cells[0].Value = n;
            CycleResults.Rows[n - 1].Cells[1].Value = GasOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[2].Value = GasoilOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[3].Value = TempOutReactorValue.Text;
            CycleResults.Rows[n - 1].Cells[4].Value = CokeOutSepValue.Text;
            CycleResults.Rows[n - 1].Cells[5].Value = CokeOutRegValue.Text;
            CycleResults.Rows[n - 1].Cells[6].Value = TempOutRegValue.Text;
            gasoilSeries.ItemsSource = CrackingProcess.g_concentration;
            gasoilModel.Series.Clear();
            gasoilModel.Series.Add(gasoilSeries);
            GasoilPlot.InvalidatePlot(true);
            tempSeries.ItemsSource = CrackingProcess.temperature;
            tempModel.Series.Clear();
            tempModel.Series.Add(tempSeries);
            TempPlot.InvalidatePlot(true);
            var ws = CrackingProcess.workbook.Worksheets.First();
            ws.Cell($"L{n+2}").Value = n;
            ws.Cell($"M{n + 2}").Value = Convert.ToDouble(GasOutReactorValue.Text);
            ws.Cell($"N{n + 2}").Value = Convert.ToDouble(GasoilOutReactorValue.Text);
            ws.Cell($"O{n + 2}").Value = Convert.ToDouble(TempOutReactorValue.Text);
            ws.Cell($"P{n + 2}").Value = Convert.ToDouble(CokeOutSepValue.Text);
            ws.Cell($"Q{n + 2}").Value = Convert.ToDouble(CokeOutRegValue.Text);
            ws.Cell($"R{n + 2}").Value = Convert.ToDouble(TempOutRegValue.Text);
        }

        private void FillExcelSheetMainInfo() {
            CrackingProcess.workbook = new XLWorkbook();
            var ws = CrackingProcess.workbook.Worksheets.Add("Результаты поверочного расчета");
            ws.Cell("H1").Value = "Исходные данные поверочного расчета";
            ws.Cell("H2").Value = "Стадия получения бензина";
            ws.Cell("H3").Value = "Геометрические параметры оборудования";
            ws.Cell("H4").Value = "Название";
            ws.Cell("I4").Value = "Единица измерения";
            ws.Cell("J4").Value = "Значение";
            ws.Cell("H5").Value = "Высота реактора";
            ws.Cell("I5").Value = "м";
            ws.Cell("J5").Value = CrackingProcess.reactor.h;
            ws.Cell("H6").Value = "Диаметр реактора";
            ws.Cell("I6").Value = "м";
            ws.Cell("J6").Value = CrackingProcess.reactor.D;

            ws.Cell("H7").Value = "Параметры свойств веществ";
            ws.Cell("H8").Value = "Название";
            ws.Cell("I8").Value = "Единица измерения";
            ws.Cell("J8").Value = "Значение";
            ws.Cell("H9").Value = "Удельная теплоемкость сырья";
            ws.Cell("I9").Value = "Дж/(кг•К)";
            ws.Cell("J9").Value = CrackingProcess.cp;
            ws.Cell("H10").Value = "Удельная теплоемкость катализатора";
            ws.Cell("I10").Value = "Дж/(кг•К)";
            ws.Cell("J10").Value = CrackingProcess.Cps;
            ws.Cell("H11").Value = "Удельная теплоемкость греющего пара";
            ws.Cell("I11").Value = "Дж/(кг•К)";
            ws.Cell("J11").Value = CrackingProcess.cf;
            ws.Cell("H12").Value = "Плотность потока сырья с катализатором";
            ws.Cell("I12").Value = "кг/м³";
            ws.Cell("J12").Value = CrackingProcess.ro;
            
            ws.Cell("H13").Value = "Эмпирические коэффициенты модели";
            ws.Cell("H14").Value = "Название";
            ws.Cell("I14").Value = "Единица измерения";
            ws.Cell("J14").Value = "Значение";
            ws.Cell("H15").Value = "Энергия активации крекинга бензина";
            ws.Cell("I15").Value = "Дж/моль";
            ws.Cell("J15").Value = CrackingProcess.Eg;
            ws.Cell("H16").Value = "Энергия активации крекинга сырья";
            ws.Cell("I16").Value = "Дж/моль";
            ws.Cell("J16").Value = CrackingProcess.Ef;
            ws.Cell("H17").Value = "Тепловой эффект крекинга сырья";
            ws.Cell("I17").Value = "Дж/кг";
            ws.Cell("J17").Value = CrackingProcess.Hf;
            ws.Cell("H18").Value = "Коэффициент старения катализатора";
            ws.Cell("I18").Value = "с⁻¹";
            ws.Cell("J18").Value = CrackingProcess.alpha;
            ws.Cell("H19").Value = "Предэкспоненциальный множитель константы скорости реакции крекинга бензина ";
            ws.Cell("I19").Value = "с⁻¹";
            ws.Cell("J19").Value = CrackingProcess.k0g;
            ws.Cell("H20").Value = "Предэкспоненциальный множитель константы скорости реакции крекинга сырья ";
            ws.Cell("I20").Value = "с⁻¹";
            ws.Cell("J20").Value = CrackingProcess.k0f;
            ws.Cell("H21").Value = "Предэкспоненциальный множитель константы скорости реакции коксообразования ";
            ws.Cell("I21").Value = "с⁻¹";
            ws.Cell("J21").Value = CrackingProcess.kc;
            ws.Cell("H22").Value = "Энергия активации реакции коксообразования ";
            ws.Cell("I22").Value = "Дж/моль";
            ws.Cell("J22").Value = CrackingProcess.Ecf;
            ws.Cell("H23").Value = "Коэффициент зависимости активности катализатора от входной концентрации кокса";
            ws.Cell("I23").Value = "-";
            ws.Cell("J23").Value = CrackingProcess.m_factor;
            ws.Cell("H24").Value = "Коэффициент зависимости выходной концентрации кокса от входной концентрации кокса в катализаторе";
            ws.Cell("I24").Value = "-";
            ws.Cell("J24").Value = CrackingProcess.N;

            ws.Cell("H25").Value = "Технологические параметры процесса";
            ws.Cell("H26").Value = "Название";
            ws.Cell("I26").Value = "Единица измерения";
            ws.Cell("J26").Value = "Значение";
            ws.Cell("H27").Value = "Массовый расход сырья";
            ws.Cell("I27").Value = "кг/с";
            ws.Cell("J27").Value = CrackingProcess.Ff;
            ws.Cell("H28").Value = "Массовый расход катализатора";
            ws.Cell("I28").Value = "кг/с";
            ws.Cell("J28").Value = CrackingProcess.Fs;
            ws.Cell("H29").Value = "Температура сырья на входе в реактор";
            ws.Cell("I29").Value = "К";
            ws.Cell("J29").Value = CrackingProcess.T0;
            ws.Cell("H30").Value = "Концентрация греющего пара в сырье";
            ws.Cell("I30").Value = "масс. доли";
            ws.Cell("J30").Value = CrackingProcess.lambda;
            ws.Cell("H31").Value = "Концентрация кокса в свежем катализаторе";
            ws.Cell("I31").Value = "масс. доли";
            ws.Cell("J31").Value = CrackingProcess.Crc;

            ws.Cell("H32").Value = "Стадия регенерации катализатора";
            ws.Cell("H33").Value = "Геометрические параметры оборудования";
            ws.Cell("H34").Value = "Название";
            ws.Cell("I34").Value = "Единица измерения";
            ws.Cell("J34").Value = "Значение";
            ws.Cell("H35").Value = "Высота регенератора";
            ws.Cell("I35").Value = "м";
            ws.Cell("J35").Value = RegenerationProcess.regenerator.h;
            ws.Cell("H36").Value = "Диаметр регенератора";
            ws.Cell("I36").Value = "м";
            ws.Cell("J36").Value = RegenerationProcess.regenerator.D;

            ws.Cell("H37").Value = "Параметры свойств веществ";
            ws.Cell("H38").Value = "Название";
            ws.Cell("I38").Value = "Единица измерения";
            ws.Cell("J38").Value = "Значение";
            ws.Cell("H39").Value = "Удельная теплоемкость воздуха";
            ws.Cell("I39").Value = "Дж/(кг•К)";
            ws.Cell("J39").Value = RegenerationProcess.Cpa;
            ws.Cell("H40").Value = "Молярная масса воздуха";
            ws.Cell("I40").Value = "г/моль";
            ws.Cell("J40").Value = RegenerationProcess.Ma;
            ws.Cell("H41").Value = "Молярная масса кокса";
            ws.Cell("I41").Value = "г/моль";
            ws.Cell("J41").Value = RegenerationProcess.Mc;
            ws.Cell("H42").Value = "Плотность катализатора";
            ws.Cell("I42").Value = "кг/м³";
            ws.Cell("J42").Value = textBox24.Text;
            ws.Cell("H43").Value = "Теплота сгорания кокса";
            ws.Cell("I43").Value = "Дж/моль";
            ws.Cell("J43").Value = RegenerationProcess.dH;

            ws.Cell("H44").Value = "Технологические параметры процесса";
            ws.Cell("H45").Value = "Название";
            ws.Cell("I45").Value = "Единица измерения";
            ws.Cell("J45").Value = "Значение";
            ws.Cell("H46").Value = "Массовый расход воздуха";
            ws.Cell("I46").Value = "кг/с";
            ws.Cell("J46").Value = RegenerationProcess.Fa;
            ws.Cell("H47").Value = "Температура воздуза";
            ws.Cell("I47").Value = "К";
            ws.Cell("J47").Value = RegenerationProcess.ta;
            ws.Cell("H48").Value = "Масса катализатора";
            ws.Cell("I48").Value = "кг";
            ws.Cell("J48").Value = RegenerationProcess.W;
            ws.Cell("H49").Value = "Масса воздуха";
            ws.Cell("I49").Value = "кг";
            ws.Cell("J49").Value = RegenerationProcess.Wa;
            ws.Cell("H50").Value = "Концентрация кислорода на входе ";
            ws.Cell("I50").Value = "мол. доли";
            ws.Cell("J50").Value = RegenerationProcess.yin;

            ws.Cell("H51").Value = "Эмпирические коэффициенты модели";
            ws.Cell("H52").Value = "Название";
            ws.Cell("I52").Value = "Единица измерения";
            ws.Cell("J52").Value = "Значение";
            ws.Cell("H53").Value = "Количество молей водорода на моли углерода в коксе";
            ws.Cell("I53").Value = "-";
            ws.Cell("J53").Value = RegenerationProcess.n;
            ws.Cell("H54").Value = "Соотношение CO₂/CO в дымовых газах";
            ws.Cell("I54").Value = "-";
            ws.Cell("J54").Value = RegenerationProcess.sigma;
            ws.Cell("H55").Value = "Коэффициент выжига кокса";
            ws.Cell("I55").Value = "с⁻¹";
            ws.Cell("J55").Value = RegenerationProcess.k;

            ws.Cell("A1").Value = "Результаты поверочного расчета первого производственного цикла";
            ws.Cell("A2").Value = "Координата по высоте реактора (м)";
            ws.Cell("B2").Value = "Концентрация бензина (масс. доли)";
            ws.Cell("C2").Value = "Концентрация сырья (масс. доли)";
            ws.Cell("D2").Value = "Температура смеси (К)";
            int iterator = 0;
            foreach (DataPoint point in CrackingProcess.concentration) {
                ws.Cell($"A{3 + iterator}").Value = Math.Round(point.X,3);
                ws.Cell($"B{3 + iterator}").Value = Math.Round(point.Y,3);
                ws.Cell($"C{ 3 + iterator}").Value = Math.Round(CrackingProcess.gasoile_c[iterator],3);
                ws.Cell($"D{3 + iterator}").Value = Math.Round(CrackingProcess.t[iterator],3);
                iterator += 1;
            }
            ws.Cell("L1").Value = "Результаты всех просчитанных циклов";
            ws.Cell("L2").Value = "№ цикла";
            ws.Cell("M2").Value = "Концентрация бензина, масс. доли";
            ws.Cell("N2").Value = "Концентрация сырья, масс. доли";
            ws.Cell("O2").Value = "Температура смеси, К";
            ws.Cell("P2").Value = "Концентрация кокса на выходе из реактора, масс. доли";
            ws.Cell("Q2").Value = "Концентрация кокса на выходе из регенератора, масс. доли";
            ws.Cell("R2").Value = "Температура катализатора на выходе из регенератора, К";
            ws.Cell("L3").Value = n;
            ws.Cell("M3").Value = Convert.ToDouble(GasOutReactorValue.Text);
            ws.Cell("N3").Value = Convert.ToDouble(GasoilOutReactorValue.Text);
            ws.Cell("O3").Value = Convert.ToDouble(TempOutReactorValue.Text);
            ws.Cell("P3").Value = Convert.ToDouble(CokeOutSepValue.Text);
            ws.Cell("Q3").Value = Convert.ToDouble(CokeOutRegValue.Text);
            ws.Cell("R3").Value = Convert.ToDouble(TempOutRegValue.Text);
        }















        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            set_color_texbox();
            try
            {
                textBox38.Text = Math.Round(set_W_regenerator(Convert.ToDouble(textBox24.Text), Convert.ToDouble(textBox20.Text), Convert.ToDouble(textBox2.Text)), 1).ToString();
                textBox38.BackColor = Color.Beige;
            }
            catch
            {
                textBox38.Text = "";
            }
        }



        private string number_raw_parametr(string parametr)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\raw.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT NUMERICAL_CHARACTERISTIC_RAW.number FROM NUMERICAL_CHARACTERISTIC_RAW INNER JOIN RAW on NUMERICAL_CHARACTERISTIC_RAW.id_raw = RAW.id" +
                             " INNER JOIN PARAMETR_RAW on NUMERICAL_CHARACTERISTIC_RAW.id_parametr = PARAMETR_RAW.id" +
                             " WHERE RAW.name = \"" + comboBox3.Text + "\" AND PARAMETR_RAW.name = \"" + parametr + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);

            connection.Close();
            if (Table_Departure.Rows.Count > 0)
                return Table_Departure.Rows[0][0].ToString().Replace('.', ',');
            else
                return "";
        }
        private string number_catalyst_parametr(string parametr)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT NUM_PARAM_CATALYST.number FROM NUM_PARAM_CATALYST INNER JOIN CATALYST on NUM_PARAM_CATALYST.id_catalyst = CATALYST.id" +
                              " INNER JOIN PARAMETR_CATALYST on NUM_PARAM_CATALYST.id_parametr = PARAMETR_CATALYST.id" +
                              " WHERE CATALYST.name = \"" + comboBox2.Text + "\" AND PARAMETR_CATALYST.name = \"" + parametr + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);

            connection.Close();
            if (Table_Departure.Rows.Count > 0)
                return Table_Departure.Rows[0][0].ToString().Replace('.', ',');
            else
                return "";
        }

        private void set_color_texbox()
        {
            textBox1.BackColor = SystemColors.Control;
            textBox3.BackColor = SystemColors.Control;
            textBox4.BackColor = SystemColors.Control;
            textBox5.BackColor = SystemColors.Control;
            textBox6.BackColor = SystemColors.Control;
            textBox7.BackColor = SystemColors.Control;
            textBox8.BackColor = Color.White;
            textBox9.BackColor = Color.White;
            textBox10.BackColor = Color.White;
            textBox11.BackColor = Color.White;
            textBox12.BackColor = Color.White;
            textBox13.BackColor = SystemColors.Control;
            textBox21.BackColor = SystemColors.Control;
            textBox14.BackColor = SystemColors.Control;
            textBox15.BackColor = SystemColors.Control;
            textBox16.BackColor = SystemColors.Control;
            textBox17.BackColor = SystemColors.Control;
            textBox18.BackColor = SystemColors.Control;
            textBox19.BackColor = SystemColors.Control;
            textBox2.BackColor = SystemColors.Control;
            textBox20.BackColor = SystemColors.Control;
            textBox24.BackColor = SystemColors.Control;
            textBox33.BackColor = SystemColors.Control;
            textBox34.BackColor = SystemColors.Control;
            textBox31.BackColor = SystemColors.Control;
            textBox27.BackColor = Color.White;
            textBox25.BackColor = SystemColors.Control;
            textBox26.BackColor = SystemColors.Control;
            textBox39.BackColor = SystemColors.Control;
            textBox28.BackColor = Color.White;
            textBox30.BackColor = Color.White;
            textBox37.BackColor = Color.White;
            textBox38.BackColor = SystemColors.Control;
            textBox32.BackColor = SystemColors.Control;
            textBox35.BackColor = SystemColors.Control;
            textBox36.BackColor = SystemColors.Control;
            textBox42.BackColor = SystemColors.Control;
            Eps.BackColor = Color.White;
            StepValue.BackColor = Color.White;
            MaxIterCount.BackColor = Color.White;
        }



        private void TextChanged_textbox(object sender, EventArgs e)
        {
            set_color_texbox();
            //TextBox textBox = sender as TextBox;
            //textBox.BackColor = Color.White;
        }


        private double set_W_regenerator(double p, double d, double h)
        {
            return (p * Math.PI * d * d / 4) * h;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage2 && tabPage2.Enabled == false)
                e.Cancel = true;
        }

        private void tableLayoutPanel23_Paint(object sender, PaintEventArgs e) {

        }

        private void label52_Click(object sender, EventArgs e) {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e) {

        }

        private void label23_Click(object sender, EventArgs e) {

        }

        private void tableLayoutPanel43_Paint(object sender, PaintEventArgs e) {

        }

        private void textBox21_TextChanged(object sender, EventArgs e) {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id" +
                              " INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " WHERE STAMP.name = \"" + comboBox4.Text + "\" AND PARAMETR_EQUIPMENT.name = \"Высота\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);

            if (Table_Departure.Rows.Count > 0)
            {
                textBox1.Text = Table_Departure.Rows[0][0].ToString().Replace('.', ',');
            }
            else
            {
                textBox1.Text = "";
            }

            sqlQuery = "SELECT NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id" +
                              " INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " WHERE STAMP.name = \"" + comboBox4.Text + "\" AND PARAMETR_EQUIPMENT.name = \"Диаметр\"";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure1 = new DataTable();
            adapter_Departure.Fill(Table_Departure1);

            if (Table_Departure1.Rows.Count > 0)
            {
                textBox3.Text = Table_Departure1.Rows[0][0].ToString().Replace('.', ',');
            }
            else
                textBox3.Text = "";
            textBox3.BackColor = Color.Beige;
            textBox1.BackColor = Color.Beige;
            connection.Close();
            ChangeStatus();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
            string sqlQuery = "SELECT NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id" +
                              " INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " WHERE STAMP.name = \"" + comboBox1.Text + "\" AND PARAMETR_EQUIPMENT.name = \"Высота\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure = new DataTable();
            adapter_Departure.Fill(Table_Departure);

            if (Table_Departure.Rows.Count > 0)
            {
                textBox2.Text = Table_Departure.Rows[0][0].ToString().Replace('.', ',');
            }
            else
            {
                textBox2.Text = "";
            }

            sqlQuery = "SELECT NUM_PARAM_EQUIPMENT.number FROM NUM_PARAM_EQUIPMENT INNER JOIN STAMP on NUM_PARAM_EQUIPMENT.id_stamp = STAMP.id" +
                              " INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id" +
                              " WHERE STAMP.name = \"" + comboBox1.Text + "\" AND PARAMETR_EQUIPMENT.name = \"Диаметр\"";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable Table_Departure1 = new DataTable();
            adapter_Departure.Fill(Table_Departure1);

            if (Table_Departure1.Rows.Count > 0)
            {
                textBox20.Text = Table_Departure1.Rows[0][0].ToString().Replace('.', ',');
            }
            else
                textBox20.Text = "";
            set_color_texbox();
            textBox2.BackColor = Color.Beige;
            textBox20.BackColor = Color.Beige;
            connection.Close();
            ChangeStatus();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_color_texbox();
            textBox6.Text = number_catalyst_parametr("Теплоемкость катализатора");
            textBox16.Text = number_catalyst_parametr("Коэффициент старения катализатора");
            textBox24.Text = number_catalyst_parametr("Плотность катализатора");
            textBox42.Text = number_catalyst_parametr("Коэффициент выжига кокса");

            textBox13.Text = number_catalyst_parametr("Энергия активакции реакции получения бензина");
            textBox17.Text = number_catalyst_parametr("Предэкспоненциальный множитель константы скорости реакции получения бензина");
            textBox25.Text = number_catalyst_parametr("Предэкспоненциальный множитель константы скорости реакции коксообразования");
            textBox19.Text = number_catalyst_parametr("Коэффициент зависимости активности катализатора от входной концентрации кокса");
            textBox39.Text = number_catalyst_parametr("Энергия активации реакции коксообразования");
            textBox26.Text = number_catalyst_parametr("Коэффициент зависимости выходной концентрации кокса от входной концентрации кокса в катализаторе");
            textBox21.Text = number_catalyst_parametr("Предэкспоненциальный множитель константы скорости крекинга бензина");

            textBox14.Text = number_catalyst_parametr("Энергия активации крекинга вакуммного газойля");
            textBox18.Text = number_catalyst_parametr("Предэкспоненциальный множитель для константы скорости реакции разложения вакумного газойля");
            textBox15.Text = number_catalyst_parametr("Тепловой эффект крекинга вакумного газойля");


            textBox6.BackColor = Color.Beige;
            textBox16.BackColor = Color.Beige;
            textBox24.BackColor = Color.Beige;
            textBox42.BackColor = Color.Beige;

            textBox13.BackColor = Color.Beige;
            textBox17.BackColor = Color.Beige;
            textBox25.BackColor = Color.Beige;
            textBox19.BackColor = Color.Beige;
            textBox39.BackColor = Color.Beige;
            textBox26.BackColor = Color.Beige;
            textBox21.BackColor = Color.Beige;

            textBox15.BackColor = Color.Beige;
            textBox14.BackColor = Color.Beige;
            textBox18.BackColor = Color.Beige;
            ChangeStatus();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_color_texbox();
            // textBox14.Text = number_raw_parametr("Энергия активации крекинга вакуммного газойля");
            textBox5.Text = number_raw_parametr("Теплоемкость газойля");
            //textBox18.Text = number_raw_parametr("Предэкспоненциальный множитель для константы скорости реакции разложения вакумного газойля");
            //textBox15.Text = number_raw_parametr("Тепловой эффект крекинга вакумного газойля");
            //textBox15.BackColor = Color.Beige;
            //textBox14.BackColor = Color.Beige;
            textBox5.BackColor = Color.Beige;
            //textBox18.BackColor = Color.Beige;
            ChangeStatus();
        }

        private void ChangeStatus()
        {
            
            if (regenerator_name_by_struct == "" || reactor_name_by_struct == "")
            {
                TaskStatus.Text = "Структура установки не задана";
                TaskStatus.ForeColor = Color.Gray;
            }
            else if (comboBox2.Text != PlannerTask.catalyst_name || comboBox3.Text != PlannerTask.raw_name)
            {
                TaskStatus.Text = "Не соответствует заданию на проектирование";
                TaskStatus.ForeColor = Color.Red;
            }
            else if (comboBox1.Text != regenerator_name_by_struct || comboBox4.Text != reactor_name_by_struct)
            {
                TaskStatus.Text = "Не соответствует структуре установки";
                TaskStatus.ForeColor = Color.Orange;
            }
            else {
                TaskStatus.Text = "В соответствии с заданием на проектирование";
                TaskStatus.ForeColor = Color.Green;
            }
        }
    }
}