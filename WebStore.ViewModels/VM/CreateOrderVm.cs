using System.ComponentModel.DataAnnotations;
namespace WebStore.ViewModels.VM
{
    public class CreateOrderVm
    {
        public int CustomerId { get; set; }
        [MinLength(1)]
        public IEnumerable<CreateOrderItemVm> Items { get; set; } = new List<CreateOrderItemVm>();
    }
}