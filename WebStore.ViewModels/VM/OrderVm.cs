namespace WebStore.ViewModels.VM
{
    public class OrderVm
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public long TrackingNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<OrderItemVm> Items { get; set; } = new List<OrderItemVm>();
    }
}