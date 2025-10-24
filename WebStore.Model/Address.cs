namespace WebStore.Model
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public string Country { get; set; }

        public Customer Customer { get; set; }
        public StationaryStore StationaryStore { get; set; }
    }
}