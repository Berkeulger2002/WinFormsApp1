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
    public partial class Form4 : Form
    {
        public Form4()
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Close(); // Form2'yi kapatır
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TextBox'lardan veri alma
            string malzemeID = textBox1.Text.Trim();
            string malzemeAdi = textBox2.Text.Trim();
            string toplamMiktar = textBox3.Text.Trim();
            string malzemeBirim = textBox4.Text.Trim();
            string birimFiyat = textBox5.Text.Trim();

            // Boş alan kontrolü
            if (string.IsNullOrEmpty(malzemeID) || string.IsNullOrEmpty(malzemeAdi) ||
                string.IsNullOrEmpty(toplamMiktar) || string.IsNullOrEmpty(malzemeBirim) ||
                string.IsNullOrEmpty(birimFiyat))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!");
                return;
            }

            try
            {
                db_connection(); // Veritabanı bağlantısını aç

                // SQL sorgusu
                string query = "INSERT INTO malzemeler (MalzemeID, MalzemeAdi, ToplamMiktar, MalzemeBirim, BirimFiyat) " +
                               "VALUES (@MalzemeID, @MalzemeAdi, @ToplamMiktar, @MalzemeBirim, @BirimFiyat)";

                MySqlCommand cmd = new MySqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@MalzemeID", malzemeID);
                cmd.Parameters.AddWithValue("@MalzemeAdi", malzemeAdi);
                cmd.Parameters.AddWithValue("@ToplamMiktar", toplamMiktar);
                cmd.Parameters.AddWithValue("@MalzemeBirim", malzemeBirim);
                cmd.Parameters.AddWithValue("@BirimFiyat", birimFiyat);

                // Sorguyu çalıştır ve kullanıcıya bilgi ver
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Malzeme başarıyla kaydedildi!");
                }
                else
                {
                    MessageBox.Show("Kayıt ekleme başarısız oldu.");
                }

                connect.Close(); // Bağlantıyı kapat
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
