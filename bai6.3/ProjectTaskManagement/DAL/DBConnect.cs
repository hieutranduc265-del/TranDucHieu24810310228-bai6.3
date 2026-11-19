using System.Data.SqlClient;

namespace ProjectTaskManagement.DAL
{
    public class DBConnect
    {
        private static readonly string connectionString =
            @"Data Source=.;Initial Catalog=ProjectTaskDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}