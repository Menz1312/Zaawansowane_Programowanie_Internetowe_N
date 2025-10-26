namespace WebStore.ViewModels.VM
{
    public class StoreVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public IEnumerable<AddressVm> Addresses { get; set; } = new List<AddressVm>();
        public IEnumerable<EmployeeVm> Employees { get; set; } = new List<EmployeeVm>();
    }
}