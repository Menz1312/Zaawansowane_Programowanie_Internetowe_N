// Wersja poprawna
namespace WebStore.Model
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order Order { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}