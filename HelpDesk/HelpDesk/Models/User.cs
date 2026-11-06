namespace HelpDesk.Models
{
    /// <summary>
    /// Пользователь системы HelpDesk.
    /// В дальнейшем данные будут загружаться из таблицы Users (MS SQL Server).
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }
    }
}
