using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices
{
    public class StoreService : BaseService, IStoreService
    {
        private readonly UserManager<User> _userManager;

        public StoreService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
            : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public async Task<StoreVm> CreateStoreAsync(AddOrUpdateStoreVm storeVm)
        {
            try
            {
                var storeEntity = Mapper.Map<StationaryStore>(storeVm);
                await DbContext.StationaryStores.AddAsync(storeEntity);
                await DbContext.SaveChangesAsync();
                return Mapper.Map<StoreVm>(storeEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        public async Task<StoreVm> UpdateStoreAsync(AddOrUpdateStoreVm storeVm)
        {
            try
            {
                if (!storeVm.Id.HasValue)
                    throw new ArgumentException("Store Id is required for update");

                var storeEntity = await DbContext.StationaryStores.FindAsync(storeVm.Id.Value);
                if (storeEntity == null)
                    throw new KeyNotFoundException("Store not found");

                Mapper.Map(storeVm, storeEntity);
                await DbContext.SaveChangesAsync();
                return Mapper.Map<StoreVm>(storeEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<StoreVm>> GetAllStoresAsync()
        {
            try
            {
                var stores = await DbContext.StationaryStores.ToListAsync();
                return Mapper.Map<IEnumerable<StoreVm>>(stores);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<StoreVm> GetStoreByIdAsync(int storeId)
        {
            try
            {
                var store = await DbContext.StationaryStores
                    .Include(s => s.Addresses)
                    .Include(s => s.StationaryStoreEmployees)
                    .FirstOrDefaultAsync(s => s.Id == storeId);
                
                if (store == null)
                    throw new KeyNotFoundException("Store not found");
                    
                return Mapper.Map<StoreVm>(store);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        public async Task<AddressVm> AddAddressToStoreAsync(int storeId, AddOrUpdateAddressVm addressVm)
        {
            try
            {
                var store = await DbContext.StationaryStores
                    .Include(s => s.Addresses)
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store == null)
                    throw new KeyNotFoundException("Store not found");

                var addressEntity = Mapper.Map<Address>(addressVm);
                store.Addresses.Add(addressEntity);
                await DbContext.SaveChangesAsync();
                
                return Mapper.Map<AddressVm>(addressEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        public async Task<EmployeeVm> AssignEmployeeToStoreAsync(AddEmployeeVm employeeVm)
        {
            try
            {
                var store = await DbContext.StationaryStores.FindAsync(employeeVm.StoreId);
                if (store == null)
                    throw new KeyNotFoundException("Store not found");

                var employeeEntity = Mapper.Map<StationaryStoreEmployee>(employeeVm);
                employeeEntity.UserName = employeeVm.Email; // Identity wymaga UserName
                employeeEntity.StationaryStore = store;
                
                var result = await _userManager.CreateAsync(employeeEntity, employeeVm.Password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create employee: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                return Mapper.Map<EmployeeVm>(employeeEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        public async Task<IEnumerable<EmployeeVm>> GetStoreEmployeesAsync(int storeId)
        {
             try
            {
                var employees = await DbContext.StationaryStoreEmployees
                    .Where(e => e.StationaryStoreId == storeId)
                    .ToListAsync();
                    
                return Mapper.Map<IEnumerable<EmployeeVm>>(employees);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        
        public async Task RemoveEmployeeFromStoreAsync(int employeeId)
        {
            try
            {
                var employee = await DbContext.StationaryStoreEmployees.FindAsync(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException("Employee not found");
                
                var result = await _userManager.DeleteAsync(employee);
                 if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Failed to delete employee");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteStoreAsync(int storeId)
        {
            try
            {
                var store = await DbContext.StationaryStores.FindAsync(storeId);
                if (store != null)
                {
                    DbContext.StationaryStores.Remove(store);
                    await DbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}