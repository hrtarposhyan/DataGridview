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

        private async void toolStripButton5_Click(object sender, EventArgs e) // Refresh
        {
            listView1.Items.Clear();
            await LoadStudentsAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) //INSERT
        {
            INSERT insert = new INSERT(SqlConnection);
            insert.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) // UPDATE
        {
            if (listView1.SelectedItems.Count > 0)
            {
                UPDATE update = new UPDATE(SqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));
                update.Show();
            }
            else
            {
                MessageBox.Show("Not a single line was selected!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private async void toolStripButton3_Click(object sender, EventArgs e)  // DELETE
        {
            if (listView1.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Do you really want to delete this line?", "Delete line", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:

                        SqlCommand deleteSqlCommand = new SqlCommand("DELETE FROM [Students] WHERE [Id]=@Id", SqlConnection);

                        deleteSqlCommand.Parameters.AddWithValue("Id", Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await deleteSqlCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        listView1.Clear();

                        await LoadStudentsAsync();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Not a single line was selected!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) // FILE EXIT
        {
            Application.Exit();
        }

        private void aboutTheProgramToolStripMenuItem_Click(object sender, EventArgs e)   //REFERENCE 
        {
            MessageBox.Show("WorkingWithRemoteDb\n C#, 2020", "About the program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
