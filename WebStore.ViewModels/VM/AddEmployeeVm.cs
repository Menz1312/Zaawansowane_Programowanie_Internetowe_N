using System.ComponentModel.DataAnnotations;
namespace WebStore.ViewModels.VM
{
    public class AddEmployeeVm
    {
        [Required]
        public string FirstName { get; set; } = default!;
        [Required]
        public string LastName { get; set; } = default!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
        public int StoreId { get; set; }
    }
}