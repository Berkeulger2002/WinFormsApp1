using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL Client kütüphanesi

namespace WinFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void db_connection()
        {
            try
            {
                string conn = "Server=localhost;Database=tarifler;Uid=root;Password=;";
                connect = new MySqlConnection(conn);
                connect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı Bağlantı Hatası: " + ex.Message);
            }
        }
        private MySqlConnection connect;

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // TextBox'lardan alınan veriler
                string tarifID = textBox1.Text.Trim(); // Primary Key
                string tarifAdi = textBox2.Text.Trim();
                string kategori = textBox3.Text.Trim();
                string hazirlamaSuresi = textBox4.Text.Trim();
                string talimatlar = textBox5.Text.Trim();

                // Boş alan kontrolü
                if (string.IsNullOrEmpty(tarifID) || string.IsNullOrEmpty(tarifAdi) || string.IsNullOrEmpty(kategori) || string.IsNullOrEmpty(hazirlamaSuresi) || string.IsNullOrEmpty(talimatlar))
                {
                    MessageBox.Show("Tüm alanları doldurmanız gerekmektedir!");
                    return;
                }

                // Veritabanı bağlantısını başlat
                db_connection();

                // SQL INSERT sorgusu
                string query = "INSERT INTO yemek (TarifID, TarifAdi, Kategori, HazirlamaSuresi, Talimatlar) " +
                               "VALUES (@tarifID, @tarifAdi, @kategori, @hazirlamaSuresi, @talimatlar)";

                MySqlCommand cmd = new MySqlCommand(query, connect);

                // Parametreler eklenir
                cmd.Parameters.AddWithValue("@tarifID", int.Parse(tarifID)); // int olduğundan dönüştürülür
                cmd.Parameters.AddWithValue("@tarifAdi", tarifAdi);
                cmd.Parameters.AddWithValue("@kategori", kategori);
                cmd.Parameters.AddWithValue("@hazirlamaSuresi", int.Parse(hazirlamaSuresi)); // int olduğundan dönüştürülür
                cmd.Parameters.AddWithValue("@talimatlar", talimatlar);

                // Sorgu çalıştırılır
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Tarif başarıyla eklendi!");
                }
                else
                {
                    MessageBox.Show("Tarif eklenemedi.");
                }

                // Bağlantı kapatılır
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close(); // Form2'yi kapatır
        }
    }
}
