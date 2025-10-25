namespace WebStore.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        //1:M
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}