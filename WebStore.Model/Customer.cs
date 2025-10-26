namespace WebStore.Model
{
    public class Customer : User
    {
        public Address BillingAddress { get; set; } = default!;
        public IList<Order> Orders { get; set; } = new List<Order>();
        public Address ShippingAddress { get; set; } = default!;
        //1:M
        public IList<Address> Addresses { get; set; } = new List<Address>();
    }
}