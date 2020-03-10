using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace DataGridview
{
    public partial class Form1 : Form
    {
        private SqlConnection SqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StudentCS"].ConnectionString;
            SqlConnection = new SqlConnection(connectionString);

            await SqlConnection.OpenAsync();

            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;

            listView1.Columns.Add("Id");
            listView1.Columns.Add("FirstName");
            listView1.Columns.Add("LastName");
            listView1.Columns.Add("Birthday");

            await LoadStudentsAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SqlConnection != null && SqlConnection.State != ConnectionState.Closed)
                SqlConnection.Close();
        }

        private async Task LoadStudentsAsync() //SELECT
        {
            SqlDataReader sqlReader = null;
            SqlCommand getStudentsCommand = new SqlCommand("SELECT * FROM[Students]", SqlConnection);
            try
            {
                sqlReader = await getStudentsCommand.ExecuteReaderAsync();

                while(await sqlReader.ReadAsync())
                {
                    ListViewItem item = new ListViewItem(new string[]
                    {
                        Convert.ToString(sqlReader["Id"]),
                        Convert.ToString(sqlReader["FirstName"]),
                        Convert.ToString(sqlReader["LastName"]),
                        Convert.ToString(sqlReader["Birthday"])
                    });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Error!", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                if(sqlReader!=null && !sqlReader.IsClosed)
                {
                    sqlReader.Close();
                }
            }
        }
    }
}
