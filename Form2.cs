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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form2_Load); // Load olayını bağlama

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
        private void LoadTarifler()
        {
            try
            {
                // Veritabanı bağlantısını aç
                db_connection();

                // SQL sorgusu
                string query = "SELECT * FROM yemek";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                MySqlDataReader reader = cmd.ExecuteReader();

                // Veritabanından gelen verileri listeye ekle
                while (reader.Read())
                {
                    listBox1.Items.Add($"ID: {reader["TarifID"]}");
                    listBox1.Items.Add($"Adı: {reader["TarifAdi"]}");
                    listBox1.Items.Add($"Kategori: {reader["Kategori"]}");
                    listBox1.Items.Add($"Süre: {reader["HazirlamaSuresi"]} dk");
                    listBox1.Items.Add($"Talimatlar: {reader["Talimatlar"]}");
                    listBox1.Items.Add(""); // Boşluk ekle
                }
                // Okuyucuyu ve bağlantıyı kapat
                reader.Close();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tarif yükleme hatası: " + ex.Message);
            }
        }

        // Form yüklendiğinde çağrılacak metot
        private void Form2_Load(object sender, EventArgs e)
        {
            LoadTarifler();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();

            // Form3'ü göster
            form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close(); // Form2'yi kapatır
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string searchKeyword = textBox1.Text.Trim(); // Kullanıcıdan alınan arama kelimesi

            if (string.IsNullOrEmpty(searchKeyword))
            {
                MessageBox.Show("Lütfen bir tarif adı girin!");
                return;
            }

            try
            {
                db_connection(); // Veritabanı bağlantısını aç

                // SQL sorgusu: Tarif adını arar
                string query = "SELECT * FROM yemek WHERE TarifAdi LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%"); // Arama parametresi

                MySqlDataReader reader = cmd.ExecuteReader();

                // Sonuçları liste veya bir kontrolde göster
                listBox2.Items.Clear(); // Önceki sonuçları temizle

                bool found = false;
                while (reader.Read())
                {
                    found = true;
                    string result = $"ID: {reader["TarifID"]}\n" +
                                    $"Adı: {reader["TarifAdi"]}\n" +
                                    $"Kategori: {reader["Kategori"]}\n" +
                                    $"Süre: {reader["HazirlamaSuresi"]} dk\n" +
                                    $"Talimatlar: {reader["Talimatlar"]}\n";
                    listBox2.Items.Add(result);
                }

                if (!found)
                {
                    listBox2.Items.Add("Aradığınız tarif bulunamadı.");
                }

                reader.Close();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string searchKeyword = textBox1.Text.Trim(); // Kullanıcıdan alınan arama kelimesi

            if (string.IsNullOrEmpty(searchKeyword))
            {
                MessageBox.Show("Lütfen bir tarif adı girin!");
                return;
            }

            try
            {
                db_connection(); // Veritabanı bağlantısını aç

                // SQL sorgusu: Tarif adını arar
                string query = "SELECT * FROM yemek WHERE TarifAdi LIKE @search";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                cmd.Parameters.AddWithValue("@search", "%" + searchKeyword + "%"); // Arama parametresi

                MySqlDataReader reader = cmd.ExecuteReader();

                // Sonuçları liste veya bir kontrolde göster
                listBox2.Items.Clear(); // Önceki sonuçları temizle

                bool found = false;
                while (reader.Read())
                {
                    found = true;

                    // ID
                    listBox2.Items.Add($"ID: {reader["TarifID"]}");

                    // Adı
                    listBox2.Items.Add($"Adı: {reader["TarifAdi"]}");

                    // Kategori
                    listBox2.Items.Add($"Kategori: {reader["Kategori"]}");

                    // Süre
                    listBox2.Items.Add($"Süre: {reader["HazirlamaSuresi"]} dk");

                    // Talimatlar
                    listBox2.Items.Add($"Talimatlar: {reader["Talimatlar"]}");

                    // Boşluk ekle (her tarifin arasına bir boşluk eklemek için)
                    listBox2.Items.Add("");
                }


                if (!found)
                {
                    listBox2.Items.Add("Aradığınız tarif bulunamadı.");
                }

                reader.Close();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close(); // Form2'yi kapatır
        }
    }
}
