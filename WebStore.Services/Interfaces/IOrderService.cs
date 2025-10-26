using WebStore.ViewModels.VM;

namespace WebStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderVm> CreateOrderAsync(CreateOrderVm orderVm);
        Task<OrderVm> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderVm>> GetOrdersForCustomerAsync(int customerId);
    }
}