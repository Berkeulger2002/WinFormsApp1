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
        private void LoadMalzemeler()
        {
            try
            {
                db_connection();

                string query = "SELECT * FROM malzemeler";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                MySqlDataReader reader = cmd.ExecuteReader();

                listBox3.Items.Clear();
                while (reader.Read())
                {
                    // Veriyi listBox3'e ekle
                    listBox3.Items.Add($"ID: {reader["MalzemeID"]}");
                    listBox3.Items.Add($"Adı: {reader["MalzemeAdi"]}");
                    listBox3.Items.Add($"Miktar: {reader["ToplamMiktar"]}");
                    listBox3.Items.Add($"Birim: {reader["MalzemeBirim"]}");
                    listBox3.Items.Add($"Fiyat: {reader["BirimFiyat"]}");
                    listBox3.Items.Add(""); // Boşluk ekle
                }

                reader.Close();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Malzemeleri yükleme hatası: " + ex.Message);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            LoadTarifler();
            LoadMalzemeler(); // Malzemeleri yükleme

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();

            // Form3'ü göster
            form3.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close(); // Form2'yi kapatır
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
                    // Tarif detaylarını listBox2'ye ekle
                    listBox2.Items.Add($"ID: {reader["TarifID"]}");
                    listBox2.Items.Add($"Adı: {reader["TarifAdi"]}");
                    listBox2.Items.Add($"Kategori: {reader["Kategori"]}");
                    listBox2.Items.Add($"Süre: {reader["HazirlamaSuresi"]} dk");
                    listBox2.Items.Add($"Talimatlar: {reader["Talimatlar"]}");
                    listBox2.Items.Add(""); // Boşluk ekle
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            // Form3'ü göster
            form4.Show();
            this.Close();
        }
        private void SearchRecipesByInstructions()
        {
            string searchKeyword = textBox2.Text.Trim(); // Kullanıcı girdisi

            if (string.IsNullOrEmpty(searchKeyword))
            {
                MessageBox.Show("Lütfen arama yapmak için bir kelime girin!");
                return;
            }

            try
            {
                db_connection(); // Veritabanı bağlantısını aç

                // TarifAdi ve TarifID'yi sorgulayan SQL
                string query = @"
            SELECT yemek.TarifID, yemek.TarifAdi
            FROM yemek
            WHERE yemek.Talimatlar LIKE @search";

                MySqlCommand command = new MySqlCommand(query, connect);
                command.Parameters.AddWithValue("@search", "%" + searchKeyword + "%");

                MySqlDataReader reader = command.ExecuteReader();

                listBox4.Items.Clear(); // Önceki sonuçları temizle

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Sadece TarifAdi ve TarifID'yi ekle
                        string result = $"Tarif Adı: {reader["TarifAdi"]}\n" +
                                        $"Tarif ID: {reader["TarifID"]}";

                        listBox4.Items.Add(result);
                        listBox4.Items.Add(""); // Boşluk ekle
                    }
                }
                else
                {
                    listBox4.Items.Add("Eşleşen tarif bulunamadı.");
                }

                reader.Close();
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SearchRecipesByInstructions();

        }
    }
}
