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
using Npgsql;

namespace v7
{
    public partial class Form1 : Form
    {
        //Database start conecctoin
        private string conString = "Host=localhost;Username=viusal;Password=12345;Database=aaa";
        private NpgsqlConnection con;
        private NpgsqlCommand cmd;
        //Database end conecctoin
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new NpgsqlConnection(conString);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        //Shaow button
        private void button5_Click(object sender, EventArgs e)
        {
            //Database start open
            con.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM ogrenci", con);

            // Execute the query and obtain a result set
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());

            dataGridView1.DataSource = dt;
            con.Close();
            //Database start close
        }

        //Insert button
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (ad.Text == "" || soyad.Text == "" || bolum.Text == "" || irik.Text == "")
            {
                MessageBox.Show("Verilerden birini girmeyi unuttun.");
                return;
            }

            if (bolum.Text != "bilgisayar")
            {
                MessageBox.Show(bolum.Text + ": Bu bölüm bulunmadı");
                return;
            }

            con.Open();
            string sql = "INSERT INTO ogrenci(ad, soyad, bolumno, irik) VALUES(@ad, @soyad, @bolumno, @irik)";

            //insert into ogrenci Start
            cmd = new NpgsqlCommand(sql, con);
            cmd.Parameters.AddWithValue("ad", ad.Text);
            cmd.Parameters.AddWithValue("soyad", soyad.Text);
            cmd.Parameters.AddWithValue("bolumno", 1);
            cmd.Parameters.AddWithValue("irik", irik.Text);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            //insert into ogrenci End
            MessageBox.Show("Bilgiler başarıyla eklendi");
            con.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                ad.Text = dataGridView1.Rows[e.RowIndex].Cells["ad"].Value.ToString();
                soyad.Text = dataGridView1.Rows[e.RowIndex].Cells["soyad"].Value.ToString();
                bolum.Text = "bilgisayar";
                id.Text = dataGridView1.Rows[e.RowIndex].Cells["ogrencino"].Value.ToString();
                irik.Text = dataGridView1.Rows[e.RowIndex].Cells["irik"].Value.ToString();
            }
        }

        //Delet button
        private void button1_Click(object sender, EventArgs e)
        {
                con.Open();
                string sql = "DELETE FROM ogrenci WHERE ogrencino=" + id.Text;
                cmd = new NpgsqlCommand(sql, con);
               
            if (cmd.ExecuteNonQuery().ToString() == "1")
                MessageBox.Show("öğrenciyi başarıyla silme");
            else
                MessageBox.Show("Bir hata oluşturdu");
                con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //Search button
        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();

            string sql = "SELECT * FROM ogrenci WHERE ogrencino=" + id.Text;
            cmd = new NpgsqlCommand(sql, con);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            dataGridView1.DataSource = dt;

            con.Close();
        }
        //Update button
        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "";

            con.Open();
            if (ad.Text != "")
            {
                sql = "UPDATE ogrenci SET ad = '" + ad.Text + "' WHERE ogrencino =" + id.Text;
                cmd = new NpgsqlCommand(sql, con);
            }
            if (soyad.Text != "")
            {
                sql = "UPDATE ogrenci set soyad = '" + soyad.Text + "' WHERE ogrencino= " + id.Text;
                cmd = new NpgsqlCommand(sql, con);
            }
            if (ad.Text == "" && soyad.Text == "")
                MessageBox.Show("Giriş bulunamadı");
            else
                MessageBox.Show("İşlem başarılı oldu.");
            cmd.ExecuteNonQuery();
            con.Close();
            
            
        }
    }
}
