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

namespace FCC_Application.Forms.Additional {
    public partial class Found_Equipment : Form {
        public Found_Equipment() {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile("Pictures\\photo_default.png");

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            datagrid.AllowUserToAddRows = false;
            dataGridView1.AllowUserToAddRows = false;

            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();

            string sqlQuery = $"SELECT staMP.id as 'Идентификатор в БД', Type.name as 'Тип оборудования', stamp.name as 'Марка' FROM STAMP INNER JOIN TYPE on TYPE.id = STAMP.id_type WHERE STAMP.id in (" + PlannerForm.list_id_string + ")";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            datagrid.DataSource = table_Departure;
            connection.Close();

             connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();

             sqlQuery = $"SELECT PARAMETR_EQUIPMENT.name as 'Параметр', NUM_PARAM_EQUIPMENT.number as 'Значение', PARAMETR_EQUIPMENT.dimension as 'Размерность' FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id INNER JOIN STAMP on STAMP.id = NUM_PARAM_EQUIPMENT.id_stamp WHERE STAMP.name = \"" + datagrid.Rows[0].Cells[2].Value + "\"";
             adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
             table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            dataGridView1.DataSource = table_Departure;
            connection.Close();




        }

        private void datagrid_CellClick(object sender, DataGridViewCellEventArgs e) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=data\\object_catalysts_tech.db3;Version=3;");
            connection.Open();
           

            string sqlQuery = $"SELECT PARAMETR_EQUIPMENT.name as 'Параметр', NUM_PARAM_EQUIPMENT.number as 'Значение', PARAMETR_EQUIPMENT.dimension as 'Размерность' FROM NUM_PARAM_EQUIPMENT INNER JOIN PARAMETR_EQUIPMENT on NUM_PARAM_EQUIPMENT.id_parametr = PARAMETR_EQUIPMENT.id INNER JOIN STAMP on STAMP.id = NUM_PARAM_EQUIPMENT.id_stamp WHERE STAMP.name = \""+ datagrid.SelectedRows[0].Cells[2].Value + "\"";
            SQLiteDataAdapter adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            DataTable table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            dataGridView1.DataSource = table_Departure;
            connection.Close();
            
            connection.Open();
            sqlQuery = $"SELECT path_pixture FROM STAMP WHERE name = \"" + datagrid.SelectedRows[0].Cells[2].Value + "\"";
            adapter_Departure = new SQLiteDataAdapter(sqlQuery, connection);
            table_Departure = new DataTable();
            adapter_Departure.Fill(table_Departure);
            connection.Close();
            try {
                pictureBox1.Image = Image.FromFile("Pictures\\"+table_Departure.Rows[0].ItemArray[0].ToString());
            }
            catch {
                pictureBox1.Image = Image.FromFile("Pictures\\photo_default.png");
            }
            

        }
    }
}
