using WebStore.DAL.EF;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using Xunit;
using System.Linq;

namespace WebStore.Tests.UnitTests
{
    public class OrderServiceUnitTests : BaseUnitTests
    {
        private readonly IOrderService _orderService;

        public OrderServiceUnitTests(ApplicationDbContext dbContext, IOrderService orderService)
            : base(dbContext)
        {
            _orderService = orderService;
        }

        [Fact]
        public async Task CreateAndGetOrderTest()
        {
            // ARRANGE
            int customerId = 2; // Z SeedData
            int productId1 = 1; // Monitor Dell z SeedData (Cena 1000)
            int productId2 = 2; // Mysz Logitech z SeedData (Cena 500)

            var createOrderVm = new CreateOrderVm
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemVm>
                {
                    new CreateOrderItemVm { ProductId = productId1, Quantity = 1 }, // 1 * 1000
                    new CreateOrderItemVm { ProductId = productId2, Quantity = 2 }  // 2 * 500
                }
            };

            // ACT
            var createdOrder = await _orderService.CreateOrderAsync(createOrderVm);
            var fetchedOrder = await _orderService.GetOrderByIdAsync(createdOrder.Id);

            // ASSERT
            Assert.NotNull(createdOrder);
            Assert.NotNull(fetchedOrder);
            Assert.Equal(3000m, createdOrder.TotalAmount);
            Assert.Equal(3000m, fetchedOrder.TotalAmount);
            Assert.Equal(2, fetchedOrder.Items.Count());
            Assert.Equal(customerId, fetchedOrder.CustomerId);
        }

        [Fact]
        public async Task GetOrdersForCustomerTest()
        {
             // ARRANGE
            int customerId = 2;
            var createOrderVm = new CreateOrderVm
            {
                CustomerId = customerId,
                Items = new List<CreateOrderItemVm> 
                { 
                    new CreateOrderItemVm { ProductId = 1, Quantity = 1 } 
                }
            };
            await _orderService.CreateOrderAsync(createOrderVm);

            // ACT
            var orders = await _orderService.GetOrdersForCustomerAsync(customerId);

            // ASSERT
            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.True(orders.All(o => o.CustomerId == customerId));
        }
    }
}