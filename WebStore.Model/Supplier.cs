namespace WebStore.Model
{
    public class Supplier : User
    {
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}