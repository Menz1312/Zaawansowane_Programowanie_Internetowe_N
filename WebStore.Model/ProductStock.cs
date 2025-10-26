namespace WebStore.Model
{
    public class ProductStock
    {
        public int Id { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}