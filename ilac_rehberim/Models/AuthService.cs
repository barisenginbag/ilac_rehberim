using System.Data.SqlClient;

namespace ilac_rehberim.Models
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Kayit WHERE KullaniciAdi = @KullaniciAdi AND Sifre = @Sifre";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciAdi", username);
                    command.Parameters.AddWithValue("@Sifre", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }

}
