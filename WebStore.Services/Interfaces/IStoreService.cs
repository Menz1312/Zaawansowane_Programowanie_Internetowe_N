using WebStore.ViewModels.VM;

namespace WebStore.Services.Interfaces
{
    public interface IStoreService
    {
        Task<StoreVm> GetStoreByIdAsync(int storeId);
        Task<IEnumerable<StoreVm>> GetAllStoresAsync();
        Task<StoreVm> CreateStoreAsync(AddOrUpdateStoreVm storeVm);
        Task<StoreVm> UpdateStoreAsync(AddOrUpdateStoreVm storeVm);
        Task DeleteStoreAsync(int storeId);
        Task<AddressVm> AddAddressToStoreAsync(int storeId, AddOrUpdateAddressVm addressVm);
        Task<IEnumerable<EmployeeVm>> GetStoreEmployeesAsync(int storeId);
        Task<EmployeeVm> AssignEmployeeToStoreAsync(AddEmployeeVm employeeVm);
        Task RemoveEmployeeFromStoreAsync(int employeeId);
    }
}