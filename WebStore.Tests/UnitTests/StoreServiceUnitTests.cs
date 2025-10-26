using WebStore.DAL.EF;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using Xunit;

namespace WebStore.Tests.UnitTests
{
    public class StoreServiceUnitTests : BaseUnitTests
    {
        private readonly IStoreService _storeService;

        public StoreServiceUnitTests(ApplicationDbContext dbContext, IStoreService storeService)
            : base(dbContext)
        {
            _storeService = storeService;
        }

        [Fact]
        public async Task CreateAndGetStoreTest()
        {
            // ARRANGE
            var newStoreVm = new AddOrUpdateStoreVm { Name = "Test Store" };

            // ACT
            var createdStore = await _storeService.CreateStoreAsync(newStoreVm);
            var fetchedStore = await _storeService.GetStoreByIdAsync(createdStore.Id);

            // ASSERT
            Assert.NotNull(createdStore);
            Assert.NotNull(fetchedStore);
            Assert.Equal("Test Store", createdStore.Name);
            Assert.Equal(createdStore.Id, fetchedStore.Id);
        }

        [Fact]
        public async Task AssignAndGetEmployeesTest()
        {
            // ARRANGE
            var newStore = await _storeService.CreateStoreAsync(new AddOrUpdateStoreVm { Name = "Store With Employees" });
            var newEmployeeVm = new AddEmployeeVm
            {
                FirstName = "Test",
                LastName = "Employee",
                Email = "test.employee@store.com",
                Password = "Password123!",
                StoreId = newStore.Id
            };

            // ACT
            var createdEmployee = await _storeService.AssignEmployeeToStoreAsync(newEmployeeVm);
            var employees = await _storeService.GetStoreEmployeesAsync(newStore.Id);

            // ASSERT
            Assert.NotNull(createdEmployee);
            Assert.NotEmpty(employees);
            Assert.Contains(employees, e => e.Email == "test.employee@store.com");
        }
        
        [Fact]
        public async Task AddAddressToStoreTest()
        {
            // ARRANGE
            var newStore = await _storeService.CreateStoreAsync(new AddOrUpdateStoreVm { Name = "Store With Address" });
            var newAddressVm = new AddOrUpdateAddressVm
            {
                City = "Store City",
                Street = "Store Street",
                Country = "Store Country",
                ZipCode = "12-345",
                BuildingNumber = 1
            };

            // ACT
            var addedAddress = await _storeService.AddAddressToStoreAsync(newStore.Id, newAddressVm);
            var storeWithData = await _storeService.GetStoreByIdAsync(newStore.Id);

            // ASSERT
            Assert.NotNull(addedAddress);
            Assert.NotEmpty(storeWithData.Addresses);
            Assert.Contains(storeWithData.Addresses, a => a.City == "Store City");
        }
    }
}