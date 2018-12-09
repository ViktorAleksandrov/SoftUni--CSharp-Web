using System;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels.Events
{
    public class CreateEventViewModel
    {
        [Required]
        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required]
        public string Place { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalTickets { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal PricePerTicket { get; set; }
    }
}
