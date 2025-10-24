namespace WebStore.Model
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public Order Order { get; set; }
    }
}