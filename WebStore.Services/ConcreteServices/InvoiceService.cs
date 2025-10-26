using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices
{
    public class InvoiceService : BaseService, IInvoiceService
    {
        public InvoiceService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger)
        {
        }

        public async Task<InvoiceVm> GenerateInvoiceForOrdersAsync(GenerateInvoiceVm invoiceVm)
        {
            try
            {
                var orders = await DbContext.Orders
                    .Where(o => invoiceVm.OrderIds.Contains(o.Id))
                    .ToListAsync();

                if (!orders.Any() || orders.Count != invoiceVm.OrderIds.Count())
                    throw new KeyNotFoundException("One or more orders not found");
                
                // Logika biznesowa: Sprawdź, czy zamówienia nie mają już faktury
                if (orders.Any(o => o.Invoice != null))
                    throw new InvalidOperationException("One or more orders are already invoiced");

                // Logika biznesowa: Sprawdź, czy wszystkie zamówienia są tego samego klienta
                var customerId = orders.First().Customer.Id;
                if(orders.Any(o => o.Customer.Id != customerId))
                    throw new InvalidOperationException("Orders must belong to the same customer");

                var newInvoice = new Invoice
                {
                    InvoiceNumber = $"FV/{DateTime.UtcNow:yyyy/MM/dd}/{orders.First().Id}",
                    Orders = orders
                };

                await DbContext.Invoices.AddAsync(newInvoice);
                await DbContext.SaveChangesAsync();

                return Mapper.Map<InvoiceVm>(newInvoice);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<InvoiceVm> GetInvoiceByIdAsync(int invoiceId)
        {
            try
            {
                var invoice = await DbContext.Invoices
                    .Include(i => i.Orders)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);
                
                if (invoice == null)
                    throw new KeyNotFoundException("Invoice not found");

                return Mapper.Map<InvoiceVm>(invoice);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}