using ilac_rehberim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ilac_rehberim.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        private readonly string _connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";

        public IActionResult Kayit()
        {
            return View();
        }
        public IActionResult Kayıt()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Kayit(string AdSoyad, string KullaniciAdi, string Telefon, string Email, string Sifre)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Kayit (NameSurname, UserName, Phone, mail, Sifre) VALUES (@AdSoyad, @KullaniciAdi, @Telefon, @Email, @Sifre)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdSoyad", AdSoyad);
                    command.Parameters.AddWithValue("@KullaniciAdi", KullaniciAdi);
                    command.Parameters.AddWithValue("@Telefon", Telefon);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Sifre", Sifre);

                    int affectedRows = command.ExecuteNonQuery();

                    // affectedRows değişkeni, sorgunun çalıştırılması sonucunda etkilenen satır sayısını içerir.
                    Console.WriteLine($"Etkilenen satır sayısı: {affectedRows}");
                }
            }

            // Başarılı bir şekilde kayıt yapıldığını belirten bir mesaj gösterilebilir veya başka bir işlem yapılabilir.
            return RedirectToAction("Index", "Home"); // Örneğin, kayıt yapıldıktan sonra ana sayfaya yönlendirilebilir.
        }



        [HttpPost]
        public IActionResult Giris(string kullaniciAdi, string sifre)
        {
            // Veritabanına bağlanma işlemi
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Kullanıcı adı ve şifreyi kontrol etme sorgusu
                string query = "SELECT COUNT(*) FROM Kayit WHERE UserName = @KullaniciAdi AND Sifre = @Sifre";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@Sifre", sifre);

                    // Kullanıcı adı ve şifre eşleşen kayıt sayısını alır
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        // Kullanıcı giriş yapabilir
                        // Kullanıcı adını oturumda sakla
                        HttpContext.Session.SetString("UserName", kullaniciAdi);

                        // Ana sayfaya yönlendir
                        return RedirectToAction("HomePage", "Home");
                    }
                    else
                    {
                        // Kullanıcı adı veya şifre hatalı
                        // Hata mesajını kullanıcıya gösterir
                        ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                        return View();
                    }
                }
            }
        }
        public IActionResult Settings()
        {
            // Kullanıcının bilgilerini veritabanından al
            string userName = HttpContext.Session.GetString("UserName");
            string query = "SELECT NameSurname, Phone, mail FROM Kayit WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        ViewBag.NameSurname = reader["NameSurname"].ToString();
                        ViewBag.Phone = reader["Phone"].ToString();
                        ViewBag.Email = reader["mail"].ToString();

                    }
                }
            }

            // Oturumdan kullanıcı adını al
            string uuserName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = uuserName;

            return View();
        }

        [HttpPost]
        public IActionResult UpdateSettings(string adSoyad, string telefon, string email, string password, string kullaniciAdi)
        {
            // Kullanıcının bilgilerini güncelleme işlemi...
            string userName = HttpContext.Session.GetString("UserName");
            string query = "UPDATE Kayit SET NameSurname = @AdSoyad, Phone = @Telefon, mail = @Email, UserName = @KullaniciAdi";
            // Şifre güncelleme sorgusu ekle
            if (!string.IsNullOrEmpty(password))
            {
                query += ", Sifre = @Password";
            }
            query += " WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdSoyad", adSoyad);
                    command.Parameters.AddWithValue("@Telefon", telefon);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi); // Yeni parametre ekleyin
                    command.Parameters.AddWithValue("@UserName", userName);
                    // Şifre parametresini ekle, eğer boş değilse
                    if (!string.IsNullOrEmpty(password))
                    {
                        command.Parameters.AddWithValue("@Password", password);
                    }

                    int affectedRows = command.ExecuteNonQuery();

                    // affectedRows değişkeni, sorgunun çalıştırılması sonucunda etkilenen satır sayısını içerir.
                    Console.WriteLine($"Etkilenen satır sayısı: {affectedRows}");
                }
            }

            // Başarılı bir şekilde güncellendiğini belirten bir mesaj gösterilebilir veya başka bir işlem yapılabilir.
            ViewBag.SuccessMessage = "Bilgileriniz başarıyla güncellendi.";

            // Settings sayfasına yönlendirilebilir.
            return RedirectToAction("Settings");
        }




    }
}
