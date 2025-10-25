namespace WebStore.Model
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        //1:M
        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}