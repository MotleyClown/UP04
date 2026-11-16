using System;

namespace HelpDesk.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; } // Текст для интерфейса
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; } // Текст для интерфейса
        public int StatusId { get; set; }
        public string Status { get; set; } // Текст для интерфейса
        public int? AssignedUserId { get; set; } // Может быть NULL
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } // Может быть NULL
    }
}
