using Microsoft.EntityFrameworkCore;
using WebStore.DAL.EF;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using Xunit;

namespace WebStore.Tests.UnitTests
{
    public class AddressServiceUnitTests : BaseUnitTests
    {
        private readonly IAddressService _addressService;

        public AddressServiceUnitTests(ApplicationDbContext dbContext, IAddressService addressService) 
            : base(dbContext)
        {
            _addressService = addressService;
        }

        [Fact]
        public void AddAddressToCustomerTest()
        {
            // ARRANGE (Przygotuj)
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "TestCity",
                Street = "TestStreet",
                Country = "TestCountry",
                ZipCode = "12-345",
                BuildingNumber = 1
            };
            int customerId = 2; // Id klienta, którego dodaliśmy w SeedData

            // ACT (Działaj)
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            // ASSERT (Sprawdź)
            Assert.NotNull(createdAddress);
            Assert.Equal("TestCity", createdAddress.City);
            Assert.True(createdAddress.Id > 0);
        }

        [Fact]
        public void GetCustomerAddressesTest()
        {
            // ARRANGE
            int customerId = 2; // Id klienta
            var newAddress = new AddOrUpdateAddressVm // Dodajemy adres testowy
            {
                City = "ListCity",
                Street = "ListStreet",
                Country = "ListCountry",
                ZipCode = "55-555",
                BuildingNumber = 50
            };
            _addressService.AddAddressToCustomer(customerId, newAddress);


            // ACT
            var addresses = _addressService.GetCustomerAddresses(customerId);

            // ASSERT
            Assert.NotNull(addresses);
            Assert.NotEmpty(addresses); // Sprawdzamy, czy lista nie jest pusta
            Assert.Contains(addresses, a => a.City == "ListCity"); // Sprawdzamy, czy zawiera nasz adres
        }

        [Fact]
        public void GetAddressTest()
        {
            // ARRANGE
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "GetAddressCity",
                Street = "GetAddressStreet",
                Country = "GetAddressCountry",
                ZipCode = "44-444",
                BuildingNumber = 30
            };
            int customerId = 2;
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            // ACT
            var fetchedAddress = _addressService.GetAddress(createdAddress.Id);

            // ASSERT
            Assert.NotNull(fetchedAddress);
            Assert.Equal(createdAddress.Id, fetchedAddress.Id);
            Assert.Equal("GetAddressCity", fetchedAddress.City);
        }

        [Fact]
        public void UpdateAddressTest()
        {
            // ARRANGE
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "InitialCity",
                Street = "InitialStreet",
                Country = "InitialCountry",
                ZipCode = "00-000",
                BuildingNumber = 10
            };
            int customerId = 2;
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            var updatedAddressVm = new AddOrUpdateAddressVm
            {
                Id = createdAddress.Id,
                City = "UpdatedCity",
                Street = "UpdatedStreet",
                Country = "UpdatedCountry",
                ZipCode = "99-999",
                BuildingNumber = 20
            };

            // ACT
            var updatedAddress = _addressService.UpdateAddress(updatedAddressVm);

            // ASSERT
            Assert.NotNull(updatedAddress);
            Assert.Equal("UpdatedCity", updatedAddress.City);
            Assert.Equal(createdAddress.Id, updatedAddress.Id);
        }

        [Fact]
        public void DeleteAddressTest()
        {
            // ARRANGE
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "DeleteCity",
                Street = "DeleteStreet",
                Country = "DeleteCountry",
                ZipCode = "11-111",
                BuildingNumber = 5
            };
            int customerId = 2;
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            // ACT
            _addressService.DeleteAddress(createdAddress.Id);

            // ASSERT
            var addresses = _addressService.GetCustomerAddresses(customerId);
            Assert.DoesNotContain(addresses, a => a.Id == createdAddress.Id);
        }

        [Fact]
        public void SetBillingAddressTest()
        {
            // ARRANGE
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "BillingCity",
                Street = "BillingStreet",
                Country = "BillingCountry",
                ZipCode = "22-222",
                BuildingNumber = 15
            };
            int customerId = 2;
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            // ACT
            _addressService.SetBillingAddress(customerId, createdAddress.Id);

            // ASSERT
            var customer = DbContext.Customers
                                    .Include(c => c.BillingAddress)
                                    .FirstOrDefault(c => c.Id == customerId);
            Assert.NotNull(customer);
            Assert.NotNull(customer!.BillingAddress);
            Assert.Equal(createdAddress.Id, customer!.BillingAddress.Id);
        }

        [Fact]
        public void SetShippingAddressTest()
        {
            // ARRANGE
            var newAddress = new AddOrUpdateAddressVm
            {
                City = "ShippingCity",
                Street = "ShippingStreet",
                Country = "ShippingCountry",
                ZipCode = "33-333",
                BuildingNumber = 25
            };
            int customerId = 2;
            var createdAddress = _addressService.AddAddressToCustomer(customerId, newAddress);

            // ACT
            _addressService.SetShippingAddress(customerId, createdAddress.Id);

            // ASSERT
            var customer = DbContext.Customers
                                    .Include(c => c.ShippingAddress)
                                    .FirstOrDefault(c => c.Id == customerId);
            Assert.NotNull(customer);
            Assert.NotNull(customer!.ShippingAddress);
            Assert.Equal(createdAddress.Id, customer!.ShippingAddress.Id);
        }

        [Fact]
        public void GetCustomerAddresses_CustomerNotFound_Test()
        {
            // ARRANGE
            int customerId = 999; // ID, które na pewno nie istnieje

            // ACT & ASSERT
            // Sprawdzamy, czy serwis rzuci wyjątek KeyNotFoundException,
            // tak jak to zaimplementowaliśmy w kodzie serwisu.
            Assert.Throws<KeyNotFoundException>(() => _addressService.GetCustomerAddresses(customerId));
        }
    }
}