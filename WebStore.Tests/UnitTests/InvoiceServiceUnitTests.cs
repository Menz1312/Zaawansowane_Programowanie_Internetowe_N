using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using Xunit;

namespace WebStore.Tests.UnitTests
{
    public class InvoiceServiceUnitTests : BaseUnitTests
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IOrderService _orderService; // Potrzebny do stworzenia zamówienia testowego

        public InvoiceServiceUnitTests(ApplicationDbContext dbContext, IInvoiceService invoiceService, IOrderService orderService)
            : base(dbContext)
        {
            _invoiceService = invoiceService;
            _orderService = orderService;
        }

        [Fact]
        public async Task GenerateAndGetInvoiceTest()
        {
            // ARRANGE
            // 1. Stwórz zamówienie, do którego wystawimy fakturę
            var createOrderVm = new CreateOrderVm
            {
                CustomerId = 2,
                Items = new List<CreateOrderItemVm> 
                { 
                    new CreateOrderItemVm { ProductId = 1, Quantity = 1 } 
                }
            };
            var order = await _orderService.CreateOrderAsync(createOrderVm);

            // 2. Przygotuj VM dla serwisu faktur
            var generateInvoiceVm = new GenerateInvoiceVm
            {
                OrderIds = new List<int> { order.Id }
            };

            // ACT
            var createdInvoice = await _invoiceService.GenerateInvoiceForOrdersAsync(generateInvoiceVm);
            var fetchedInvoice = await _invoiceService.GetInvoiceByIdAsync(createdInvoice.Id);

            // ASSERT
            Assert.NotNull(createdInvoice);
            Assert.NotNull(fetchedInvoice);
            Assert.NotEmpty(createdInvoice.InvoiceNumber);
            Assert.Contains(order.Id, fetchedInvoice.OrderIds);
        }
    }
}