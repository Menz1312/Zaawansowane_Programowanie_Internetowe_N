namespace WebStore.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Tag { get; set; } = default!;
        //1:M
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}