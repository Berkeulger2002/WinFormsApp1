using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL Client kütüphanesi

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void db_connection()
        {
            try
            {
                string conn = "Server=localhost;Database=login;Uid=root;Password=;";
                connect = new MySqlConnection(conn);
                connect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private MySqlConnection connect;

        private bool validate_login(string user, string pass)
        {
            db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "SELECT * FROM user_account WHERE user_name = '" + textBox1.Text + "' AND password = '" + textBox2.Text + "'";
            cmd.Connection = connect;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                connect.Close();
                return true;
            }
            else
            {
                connect.Close();
                return false;
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string user = textBox1.Text;  // Kullanýcý adý alýnýr
                string pass = textBox2.Text;  // Þifre alýnýr
                if (user == "" || pass == "")
                {
                    MessageBox.Show("Please provide details.");
                    return;
                }
                bool r = validate_login(user, pass);
                if (r)
                {
                    Form2 mf = new Form2();
                    mf.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                // Hata yakalanýrsa mesaj gösterilir
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanýcýdan alýnan bilgiler
                string userr = textBox3.Text.Trim();
                string passs = textBox4.Text.Trim();

                // Boþ alan kontrolü
                if (string.IsNullOrEmpty(userr) || string.IsNullOrEmpty(passs))
                {
                    MessageBox.Show("Kullanýcý adý ve þifre boþ býrakýlamaz!");
                    return;
                }

                // Veritabaný baðlantýsýný baþlat
                db_connection();

                // SQL INSERT komutu
                string query = "INSERT INTO user_account (user_name, password) VALUES (@user, @pass)";
                MySqlCommand cmd = new MySqlCommand(query, connect);

                // Parametre ekleme
                cmd.Parameters.AddWithValue("@user", userr);
                cmd.Parameters.AddWithValue("@pass", passs);

                // Komut çalýþtýrma
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kayýt baþarýyla eklendi!");
                }
                else
                {
                    MessageBox.Show("Kayýt eklenemedi.");
                }

                // Baðlantýyý kapat
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
