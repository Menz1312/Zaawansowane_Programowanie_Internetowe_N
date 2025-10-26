using WebStore.ViewModels.VM;

namespace WebStore.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceVm> GenerateInvoiceForOrdersAsync(GenerateInvoiceVm invoiceVm);
        Task<InvoiceVm> GetInvoiceByIdAsync(int invoiceId);
    }
}