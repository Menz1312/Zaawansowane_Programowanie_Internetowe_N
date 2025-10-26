namespace WebStore.ViewModels.VM
{
    public class OrderItemVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Cena jednostkowa w momencie zamówienia
    }
}