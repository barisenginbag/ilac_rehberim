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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string _connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult grafik()
        {
            // Oturumdan kullanıcı adını al
            string userName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = userName;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Not()
        {
            List<DoktorNotu> notlar = new List<DoktorNotu>();

            try
            {
                string connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";
                string query = "SELECT Id, NotContent FROM DoktorNotu"; // Id sütununu da seçiyoruz

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DoktorNotu doktorNotu = new DoktorNotu();
                        doktorNotu.Id = (int)reader["Id"];
                        doktorNotu.NotContent = reader["NotContent"].ToString();
                        notlar.Add(doktorNotu);
                    }

                    connection.Close();
                }

                // Sıralamayı NotContent sütununa göre gerçekleştir
                notlar = notlar.OrderBy(x => x.NotContent).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            // Oturumdan kullanıcı adını al
            string userName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = userName;

            return View(notlar);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            try
            {
                // Silinecek notu veritabanından kaldır
                string connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";
                string query = "DELETE FROM DoktorNotu WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Başarılı bir şekilde silindiğinde, kullanıcıyı tekrar not listesine yönlendir
                return RedirectToAction("Not");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                // Hata durumunda aynı sayfaya yönlendir
                return RedirectToAction("Not");
            }
        }






        public IActionResult Giris()
		{
			return View();
		}
		public IActionResult Buy()
		{
			return View();
		}
        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult HomePage()
        {

            // Oturumdan kullanıcı adını al
            string userName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = userName;

            return View();
        }
        public IActionResult Sorgu()
        {
            List<DoktorTanı> sonuçlar = new List<DoktorTanı>();

            try
            {
                string connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";
                string query = "SELECT DISTINCT Beşeri_Tıbbi_Ürün_İsmi, Farmasötik_Form, Etkin_Madde, Tanı, Şikayet, column6, column7, column8, column9 FROM doktor_tanı_koyma";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DoktorTanı doktorTanı = new DoktorTanı();

                        doktorTanı.Beşeri_Tıbbi_Ürün_İsmi = reader["Beşeri_Tıbbi_Ürün_İsmi"].ToString();
                        doktorTanı.Farmasötik_Form = reader["Farmasötik_Form"].ToString();
                        doktorTanı.Etkin_Madde = reader["Etkin_Madde"].ToString();
                        doktorTanı.Tanı = reader["Tanı"].ToString();
                        doktorTanı.Şikayet = reader["Şikayet"].ToString();
                        doktorTanı.Column6 = reader["column6"].ToString();
                        doktorTanı.Column7 = reader["column7"].ToString();
                        doktorTanı.Column8 = reader["column8"].ToString();
                        doktorTanı.Column9 = reader["column9"].ToString();

                        sonuçlar.Add(doktorTanı);
                    }

                    connection.Close();
                }

                // Sıralamayı Tanı sütununa göre gerçekleştir
                sonuçlar = sonuçlar.OrderBy(x => x.Tanı).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            // Oturumdan kullanıcı adını al
            string userName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = userName;

            return View(sonuçlar);
        }



        [HttpPost]
        [HttpPost]
        public ActionResult Query(string yas, string cinsiyet, string hastalik, string doktor_notu)
        {
            List<DoktorTanı> sonuçlar = new List<DoktorTanı>();

            try
            {
                string connectionString = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";
                string query = @"SELECT [Beşeri_Tıbbi_Ürün_İsmi], [Farmasötik_Form], [Etkin_Madde], [Tanı], [Şikayet], [column6], [column7], [column8], [column9]
                 FROM [ilac_rehberim].[dbo].[doktor_tanı_koyma]
                 WHERE [Tanı] = @selectedTanı";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@selectedTanı", hastalik);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DoktorTanı doktorTanı = new DoktorTanı();

                        doktorTanı.Beşeri_Tıbbi_Ürün_İsmi = reader["Beşeri_Tıbbi_Ürün_İsmi"].ToString();
                        doktorTanı.Farmasötik_Form = reader["Farmasötik_Form"].ToString();
                        doktorTanı.Etkin_Madde = reader["Etkin_Madde"].ToString();
                        doktorTanı.Tanı = reader["Tanı"].ToString();
                        doktorTanı.Şikayet = reader["Şikayet"].ToString();
                        doktorTanı.Column6 = reader["column6"].ToString();
                        doktorTanı.Column7 = reader["column7"].ToString();
                        doktorTanı.Column8 = reader["column8"].ToString();
                        doktorTanı.Column9 = reader["column9"].ToString();

                        sonuçlar.Add(doktorTanı);
                    }

                    connection.Close();
                }

                // Sıralamayı Tanı sütununa göre gerçekleştir
                sonuçlar = sonuçlar.OrderBy(x => x.Tanı).ToList();


                // Veritabanı bağlantı dizesi
                string connectionString1 = "Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;";

                // SQL sorgusu
                string query1 = "INSERT INTO DoktorNotu (NotContent) VALUES (@notContent)";

                // Veritabanı bağlantısını oluştur
                using (SqlConnection connection = new SqlConnection(connectionString1))
                {
                    // SQL komutunu oluştur
                    SqlCommand command = new SqlCommand(query1, connection);

                    // Parametreyi ekle
                    command.Parameters.AddWithValue("@notContent", doktor_notu);

                    // Bağlantıyı aç
                    connection.Open();

                    // Komutu çalıştır
                    command.ExecuteNonQuery();

                    // Bağlantıyı kapat
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            // Oturumdan kullanıcı adını al
            string userName = HttpContext.Session.GetString("UserName");

            // Kullanıcı adını view'a aktar
            ViewBag.UserName = userName;

            // Sonuçları model olarak view'e ileterek göster
            return View("Sonuçlar", sonuçlar);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
