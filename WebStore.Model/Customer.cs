namespace WebStore.Model
{
    public class Customer : User
    {
        public Address BillingAddress { get; set; }
        public IList<Order> Orders { get; set; } = new List<Order>();
        public Address ShippingAddress { get; set; }
        //1:M
        public IList<Address> Addresses { get; set; } = new List<Address>();
    }
}