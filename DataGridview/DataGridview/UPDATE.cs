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

namespace DataGridview
{
    public partial class UPDATE : Form
    {
        private SqlConnection sqlConnection = null;
        private int id;
        public UPDATE(SqlConnection connection, int id)
        {
            InitializeComponent();

            sqlConnection = connection;

            this.id = id;
        }

        private async void UPDATE_Load(object sender, EventArgs e)
        {
            SqlCommand getStudentInfoCommand = new SqlCommand("Select [FirstName],[LastName],[Birthday] FROM[Students] WHERE [Id]=@Id",sqlConnection);
            getStudentInfoCommand.Parameters.AddWithValue("Id", id);
            SqlDataReader sqlReader = null;
            try
            {
                sqlReader = await getStudentInfoCommand.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    //textBox1.Text = sqlReader["FirstNmae"].ToString(); null reference exeption
                    textBox1.Text = Convert.ToString(sqlReader["FirstName"]);
                    textBox2.Text = Convert.ToString(sqlReader["LastName"]);
                    textBox3.Text = Convert.ToString(sqlReader["Birthday"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null && !sqlReader.IsClosed)
                    sqlReader.Close();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand updateStudentCommand = new SqlCommand("UPDATE [Students] SET [FirstName]=@FirstName,[LastName]=@LastName,[Birthday]=@Birthday WHERE [Id]=@Id",sqlConnection);
            updateStudentCommand.Parameters.AddWithValue("Id", id);
            updateStudentCommand.Parameters.AddWithValue("FirstName", textBox1.Text);
            updateStudentCommand.Parameters.AddWithValue("LastName", textBox2.Text);
            updateStudentCommand.Parameters.AddWithValue("Birthday", Convert.ToDateTime(textBox3.Text));

            try
            {
                await updateStudentCommand.ExecuteNonQueryAsync();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
