namespace WebStore.Model
{
    public class StationaryStoreEmployee : User
    {
        public int StationaryStoreId { get; set; }
        public StationaryStore StationaryStore { get; set; } = default!;
    }
}