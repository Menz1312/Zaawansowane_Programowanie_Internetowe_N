namespace WebStore.Model
{
    public class StationaryStore
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        //1:M
        public IList<Address> Addresses { get; set; } = new List<Address>();
        public IList<StationaryStoreEmployee> StationaryStoreEmployees { get; set; } =  new List<StationaryStoreEmployee>();
    }
}