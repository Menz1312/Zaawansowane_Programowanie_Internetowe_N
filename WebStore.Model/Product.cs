namespace WebStore.Model
{
    public class Product
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ImageBytes { get; set; }
        public decimal Price { get; set; }
        public Supplier Supplier { get; set; }
        public float Weight { get; set; }
        //1:M
        public IList<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}