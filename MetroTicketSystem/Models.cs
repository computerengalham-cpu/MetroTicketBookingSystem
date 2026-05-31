using System;
using System.Collections.Generic;

namespace MetroTicketSystem.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        // علاقة: المحطة الواحدة لها العديد من التذاكر
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

    public class Train
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public int Capacity { get; set; }

        // علاقة: القطار الواحد له العديد من التذاكر
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }

    public class Ticket
    {
        public int Id { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime TravelDate { get; set; }

        // المفاتيح الخارجية (Foreign Keys)
        public int TrainId { get; set; }
        public int StationId { get; set; }

        // روابط التنقل: كل تذكرة تنتمي لقطار واحد ومحطة واحدة
        public Train Train { get; set; } = null!;
        public Station Station { get; set; } = null!;
    }
}