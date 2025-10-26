using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger)
        {
        }

        public async Task<OrderVm> CreateOrderAsync(CreateOrderVm orderVm)
        {
            await using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var customer = await DbContext.Customers.FindAsync(orderVm.CustomerId);
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                var newOrder = new Order
                {
                    Customer = customer,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 0 // Obliczymy poniżej
                };

                var orderProducts = new List<OrderProduct>();
                decimal totalAmount = 0;

                foreach (var item in orderVm.Items)
                {
                    var product = await DbContext.Products.FindAsync(item.ProductId);
                    if (product == null)
                        throw new KeyNotFoundException($"Product with Id {item.ProductId} not found");

                    var orderProduct = new OrderProduct
                    {
                        Order = newOrder,
                        Product = product,
                        Quantity = item.Quantity
                    };
                    orderProducts.Add(orderProduct);
                    totalAmount += product.Price * item.Quantity;
                }
                
                newOrder.TotalAmount = totalAmount;
                newOrder.OrderProducts = orderProducts;
                
                await DbContext.Orders.AddAsync(newOrder);
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Mapper.Map<OrderVm>(newOrder);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Logger.LogError(ex, "Failed to create order");
                throw;
            }
        }

        public async Task<OrderVm> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await DbContext.Orders
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
                
                if (order == null)
                    throw new KeyNotFoundException("Order not found");

                return Mapper.Map<OrderVm>(order);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<OrderVm>> GetOrdersForCustomerAsync(int customerId)
        {
            try
            {
                var orders = await DbContext.Orders
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .Where(o => o.Customer.Id == customerId)
                    .ToListAsync();

                return Mapper.Map<IEnumerable<OrderVm>>(orders);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}