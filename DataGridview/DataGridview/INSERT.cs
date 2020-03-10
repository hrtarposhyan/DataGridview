﻿using System;
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
    public partial class INSERT : Form
    {
        private SqlConnection SqlConnection=null;
        public INSERT(SqlConnection connection)
        {
            InitializeComponent();
            SqlConnection = connection;
        }

        private void INSERT_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            SqlCommand insertStudentCommand = new SqlCommand("INSERT INTO[Students](FirstName,LastName,Birthday) VALUES(@FirstName,@LastName,@Birthday)",SqlConnection);
            insertStudentCommand.Parameters.AddWithValue("FirstName", textBox1.Text);
            insertStudentCommand.Parameters.AddWithValue("LastName", textBox2.Text);
            insertStudentCommand.Parameters.AddWithValue("Birthday", Convert.ToDateTime(textBox3.Text));
            try
            {
                await insertStudentCommand.ExecuteNonQueryAsync();
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
