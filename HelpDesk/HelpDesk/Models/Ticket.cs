using System;

namespace HelpDesk.Models
{
    /// <summary>
    /// Заявка технической поддержки.
    /// В дальнейшем данные будут загружаться из таблицы Tickets (MS SQL Server).
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
