using System.Data.SqlClient;

namespace HelpDesk.Data
{
    public static class Database
    {
        public const string ConnectionString = @"Data Source=ViviPC\MOTLYSQL;Initial Catalog=HelpDesk_Kholkin;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Создаёт объект подключения к базе данных.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
