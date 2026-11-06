using System.Collections.Generic;

namespace HelpDesk.Models
{
    /// <summary>
    /// Статус заявки (справочник).
    /// Свойства Id и Name соответствуют полям таблицы Statuses
    /// базы данных MS SQL Server.
    /// </summary>
    public class Status
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Status(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Стандартный список статусов (до подключения базы данных).
        /// </summary>
        public static List<Status> GetAll()
        {
            return new List<Status>
            {
                new Status(1, "Новая"),
                new Status(2, "В работе"),
                new Status(3, "Выполнена"),
                new Status(4, "Отменена")
            };
        }
    }
}
