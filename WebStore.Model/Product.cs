namespace WebStore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public Category Category { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public byte[] ImageBytes { get; set; } = default!;
        public decimal Price { get; set; }
        public Supplier Supplier { get; set; } = default!;
        public float Weight { get; set; }
        //1:M
        public IList<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}