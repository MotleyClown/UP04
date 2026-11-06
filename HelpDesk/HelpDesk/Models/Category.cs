using System.Collections.Generic;

namespace HelpDesk.Models
{
    /// <summary>
    /// Категория заявки (справочник).
    /// Свойства Id и Name соответствуют полям таблицы Categories
    /// базы данных MS SQL Server.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Стандартный список категорий (до подключения базы данных).
        /// </summary>
        public static List<Category> GetAll()
        {
            return new List<Category>
            {
                new Category(1, "Техника"),
                new Category(2, "Сеть"),
                new Category(3, "Программное обеспечение"),
                new Category(4, "Прочее")
            };
        }
    }
}
