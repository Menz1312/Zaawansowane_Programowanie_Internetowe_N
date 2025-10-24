namespace WebStore.Model
{
    public class Customer : User
    {
        //Do poprawy - Jeden customer może mieć wiele adresów, a tu są dwa pola adresowe, czy też do każdego powinna być możliwośc dopisania wielu adresów?
        public Address BillingAddress { get; set; }
        public IList<Order> Orders { get; set; } = new List<Order>();
        public Address ShippingAddress { get; set; }
    }
}