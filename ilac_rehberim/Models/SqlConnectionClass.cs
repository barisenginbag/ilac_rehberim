using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace ilac_rehberim.Models
{
    public class SqlConnectionClass
    {
        public static SqlConnection connection = new SqlConnection("Data Source=DESKTOP-69B0J7H;Initial Catalog=ilac_rehberim;Integrated Security=True;Trust Server Certificate=True");

        public static void CheckConnetion()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }else
            {

            }
        }
    }
}
