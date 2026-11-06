using System.Data.SqlClient;

namespace HelpDesk.Data
{
    /// <summary>
    /// Класс для взаимодействия с MS SQL Server через ADO.NET.
    /// Здесь хранится строка подключения, чтобы все компоненты
    /// программы работали с базой данных одинаково.
    /// Методы получения и сохранения данных будут добавлены
    /// на следующих практических работах.
    /// </summary>
    public static class Database
    {
        // Строка подключения к MS SQL Server (имя сервера и базы — примерные)
        public const string ConnectionString =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=HelpDeskDB;" +
            "Integrated Security=True;";

        /// <summary>
        /// Создаёт и открывает подключение к базе данных.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            return connection;
        }
    }
}
