namespace WebStore.Model
{
    public class OrderProduct
    {
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}