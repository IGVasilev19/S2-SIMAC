using Microsoft.Data.SqlClient;

namespace NotificationApp.Models

{
    public class DBConnection
    {
        private static readonly string _connectionString = @"Server=mssqlstud.fhict.local;Database=dbi561741_simacdb;
                                                            User Id=dbi561741;Password=simacpassword;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
