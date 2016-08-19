using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EnglishWordLearning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Event();
            Bring();
            ShowFormDisable();
        }

        SqlConnection conn = new SqlConnection("Data Source=ENJOYKOVBOY;Initial Catalog=mydb;Integrated Security=True");
        SqlCommand comm = new SqlCommand();
        int a = 0;

        public void Event()
        {
            if(conn.State == ConnectionState.Open)
            {
                label1.ForeColor = Color.Green;
                label1.Text = "connection open";
            }

            else if(conn.State == ConnectionState.Closed)
            {
                label1.ForeColor = Color.Red;
                label1.Text = "connection close";
            }

            else if(conn.State == ConnectionState.Executing)
            {
                label1.ForeColor = Color.Blue;
                label1.Text = "processing on";
            }
        }

        public void Add(string eng, string tr)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                textBox1.Focus();
                if(textBox1.Text != "") textBox2.Focus();
            }
            else
            {
                conn.Open();
                Event();
                comm.Connection = conn;
                comm.CommandText = "insert into EnglishWordLearning values (@eng, @tur)";
                comm.Parameters.AddWithValue("@eng", eng);
                comm.Parameters.AddWithValue("@tur", tr);
                if(comm.ExecuteNonQuery() == 1)
                    MessageBox.Show("Your words on the server at now");

                comm.Parameters.Clear();
                conn.Close();
                Event();
            }
        }

        public void Bring()
        {
            conn.Open();
            Event();
            DataSet ds = new DataSet();
            SqlDataAdapter adaptr = new SqlDataAdapter("select * from EnglishWordLearning", conn);
            adaptr.Fill(ds, "EnglishWordLearning");
            dataGridView1.DataSource = ds.Tables["EnglishWordLearning"];
            comm.Parameters.Clear();
            conn.Close();
            Event();
        }

        public void Update(string eng, string tur, int Id)
        {
            conn.Open();
            Event();
            comm.Connection = conn;
            comm.CommandText = "update EnglishWordLearning set english = @Eng, Turkish = @Tur Where Id='" + Convert.ToInt32(numericUpDown1.Text) + "'";
            comm.Parameters.AddWithValue("@Eng", eng);
            comm.Parameters.AddWithValue("@Tur", tur);
            if(comm.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Updated was succesfully");
            }
            else
            {
                MessageBox.Show("another");
            }
            comm.Parameters.Clear();
            conn.Close();
            Event();
        }

        public void Remove()
        {
            conn.Open();
            Event();
            comm.Connection = conn;
            comm.CommandText = "delete from EnglishWordLearning where Id='" + numericUpDown1.Text + "'";
            comm.ExecuteNonQuery();
            comm.Parameters.Clear();
            conn.Close();
            Event();
        }

        public void ShowFormEnable()
        {
            groupBox2.Visible = true;
            this.Height = 320;
            this.Width = 580;
            this.Location = new Point(780, 70);
            button8.Text = "-->";
        }

        public void ShowFormDisable()
        {
            groupBox2.Visible = false;
            this.Height = 320;
            this.Width = 200;
            this.Location = new Point(1150, 70);
            button8.Text = "<--";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.TopMost == true) this.TopMost = false;
            else this.TopMost = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Add(textBox1.Text, textBox2.Text);
            Bring();
            Event();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Remove();
            Bring();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = "refresh";
            Bring();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Update(textBox1.Text, textBox2.Text, Convert.ToInt32(numericUpDown1.Text));
            Bring();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = a;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            conn.Open();
            comm.Connection = conn;
            comm.CommandText = "Delete From EnglishWordLearning";
            comm.ExecuteNonQuery();
            conn.Close();
            Bring();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(groupBox2.Visible == false)
            {
                ShowFormEnable();
            }
            else
            {
                ShowFormDisable();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled == false)
            {
                button9.Text = "Stop";
                button8.Enabled = false;
                button3.Enabled = false;
                timer1.Enabled = true;
                groupBox2.Enabled = false;
            }
            else
            {
                timer1.Enabled = false;
                button9.Text = "Start";
                button8.Enabled = true;
                button9.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a++;
            button9.Text = a.ToString();
            if(a == 10)
            {
                ShowFormEnable();
            }
            if(a == 13)
            {
                ShowFormDisable();
                a = 0;
            }
        }
    }
}
