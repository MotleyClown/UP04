using System.Collections.Generic;

namespace HelpDesk.Models
{
    /// <summary>
    /// Приоритет заявки (справочник).
    /// Свойства Id и Name соответствуют полям таблицы Priorities
    /// базы данных MS SQL Server.
    /// </summary>
    public class Priority
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Priority(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Стандартный список приоритетов (до подключения базы данных).
        /// </summary>
        public static List<Priority> GetAll()
        {
            return new List<Priority>
            {
                new Priority(1, "Низкий"),
                new Priority(2, "Средний"),
                new Priority(3, "Высокий")
            };
        }
    }
}
