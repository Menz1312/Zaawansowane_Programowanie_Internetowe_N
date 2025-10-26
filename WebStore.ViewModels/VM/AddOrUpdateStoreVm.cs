using System.ComponentModel.DataAnnotations;
namespace WebStore.ViewModels.VM
{
    public class AddOrUpdateStoreVm
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
    }
}