namespace CHUSHKA.ViewModels.Orders
{
    public class AllOrdersViewModel : IdViewModel
    {
        public int Index { get; set; }

        public string Customer { get; set; }

        public string Product { get; set; }

        public string OrderedOn { get; set; }
    }
}
