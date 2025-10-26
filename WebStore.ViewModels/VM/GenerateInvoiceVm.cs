using System.ComponentModel.DataAnnotations;
namespace WebStore.ViewModels.VM
{
    public class GenerateInvoiceVm
    {
        [MinLength(1)]
        public IEnumerable<int> OrderIds { get; set; } = new List<int>();
    }
}