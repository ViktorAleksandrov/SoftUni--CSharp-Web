namespace PANDA.App.ViewModels.Receipts
{
    public class ReceiptsIndexViewModel
    {
        public int Id { get; set; }

        public decimal Fee { get; set; }

        public string IssuedOn { get; set; }

        public string Recipient { get; set; }
    }
}
