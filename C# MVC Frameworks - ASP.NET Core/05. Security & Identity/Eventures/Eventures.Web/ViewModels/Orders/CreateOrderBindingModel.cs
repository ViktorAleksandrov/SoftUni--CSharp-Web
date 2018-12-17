using System.ComponentModel.DataAnnotations;

namespace Eventures.Web.ViewModels.Orders
{
    public class CreateOrderBindingModel
    {
        public string EventId { get; set; }

        [Range(1, int.MaxValue)]
        public int Tickets { get; set; }
    }
}
