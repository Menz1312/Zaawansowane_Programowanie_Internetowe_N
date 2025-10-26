namespace WebStore.ViewModels.VM
{
    public class InvoiceVm
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public IEnumerable<int> OrderIds { get; set; } = new List<int>();
    }
}