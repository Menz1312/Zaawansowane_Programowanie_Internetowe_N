using System.ComponentModel.DataAnnotations;

namespace WebStore.Model
{
    public class Order
    {
        public Customer Customer { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; }
        public long TrackingNumber { get; set; }
        public Invoice Invoice { get; set; }
    }
}