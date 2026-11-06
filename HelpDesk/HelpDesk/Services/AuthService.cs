namespace HelpDesk.Services
{
    /// <summary>
    /// Сервис авторизации пользователя.
    /// Пока проверка выполняется по эталонным значениям (временная авторизация);
    /// после подключения базы данных метод Authenticate будет проверять
    /// логин и пароль по таблице Users через Database и ADO.NET.
    /// </summary>
    public static class AuthService
    {
        // Временные эталонные учётные данные администратора
        private const string AdminLogin = "admin";

        private const string AdminPassword = "12345";

        /// <summary>
        /// Проверяет логин и пароль пользователя.
        /// Возвращает true, если данные верные.
        /// </summary>
        public static bool Authenticate(string login, string password)
        {
            return login == AdminLogin && password == AdminPassword;
        }
    }
}
