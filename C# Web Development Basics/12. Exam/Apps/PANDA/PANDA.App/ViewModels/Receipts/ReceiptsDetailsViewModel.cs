namespace PANDA.App.ViewModels.Receipts
{
    public class ReceiptsDetailsViewModel : ReceiptsIndexViewModel
    {
        public string DeliveryAddress { get; set; }

        public decimal Weight { get; set; }

        public string Description { get; set; }
    }
}
