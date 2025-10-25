using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using System.Linq.Expressions;

namespace WebStore.Services.ConcreteServices
{
    public class AddressService : BaseService, IAddressService
    {
        public AddressService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
        }

        public AddressVm AddAddressToCustomer(int customerId, AddOrUpdateAddressVm addAddressVm)
        {
            try
            {
                // 1. Znajdź klienta
                var customer = DbContext.Customers
                                        .Include(c => c.Addresses)
                                        .FirstOrDefault(c => c.Id == customerId);

                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                // 2. Zamapuj VM na encję
                var addressEntity = Mapper.Map<Address>(addAddressVm);

                // 3. Dodaj adres do kolekcji klienta (relacja 1:M)
                customer.Addresses.Add(addressEntity);
                
                // 4. Zapisz zmiany
                DbContext.SaveChanges();

                // 5. Zwróć zamapowany VM
                return Mapper.Map<AddressVm>(addressEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public AddressVm UpdateAddress(AddOrUpdateAddressVm updateAddressVm)
        {
            try
            {
                if (!updateAddressVm.Id.HasValue)
                    throw new ArgumentException("Address Id is required for update");

                // 1. Znajdź istniejącą, śledzoną encję w bazie
                var addressEntity = DbContext.Addresses.Find(updateAddressVm.Id.Value);

                if (addressEntity == null)
                {
                    // Adres nie istnieje, więc nie można go zaktualizować
                    throw new KeyNotFoundException($"Address with Id {updateAddressVm.Id.Value} not found");
                }

                // 2. Zaktualizuj właściwości istniejącej encji danymi z VM
                //    AutoMapper (Mapper.Map) inteligentnie zmapuje dane Z "updateAddressVm" DO "addressEntity"
                Mapper.Map(updateAddressVm, addressEntity);

                // 3. Zapisz zmiany (EF wie już, że encja jest zmodyfikowana)
                DbContext.SaveChanges();

                // 4. Zwróć zamapowany VM
                return Mapper.Map<AddressVm>(addressEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<AddressVm> GetCustomerAddresses(int customerId)
        {
            try
            {
                // 1. Znajdź klienta i załaduj jego adresy
                var customer = DbContext.Customers
                                .Include(c => c.Addresses) // Ważne: Załaduj powiązane adresy
                                .FirstOrDefault(c => c.Id == customerId);
                
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                // 2. Zamapuj kolekcję na VM
                return Mapper.Map<IEnumerable<AddressVm>>(customer.Addresses);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public AddressVm GetAddress(int addressId)
        {
            try
            {
                var address = DbContext.Addresses.Find(addressId);
                if (address == null)
                    throw new KeyNotFoundException("Address not found");
                return Mapper.Map<AddressVm>(address);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void DeleteAddress(int addressId)
        {
            try
            {
                var address = DbContext.Addresses.Find(addressId);
                if (address == null)
                    throw new KeyNotFoundException("Address not found");

                DbContext.Addresses.Remove(address);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void SetBillingAddress(int customerId, int addressId)
        {
            try
            {
                var customer = DbContext.Customers
                                .Include(c => c.Addresses)
                                .FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    throw new KeyNotFoundException("Address not found for this customer");

                customer.BillingAddress = address;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void SetShippingAddress(int customerId, int addressId)
        {
            try
            {
                var customer = DbContext.Customers
                                .Include(c => c.Addresses)
                                .FirstOrDefault(c => c.Id == customerId);
                if (customer == null)
                    throw new KeyNotFoundException("Customer not found");

                var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId);
                if (address == null)
                    throw new KeyNotFoundException("Address not found for this customer");

                customer.ShippingAddress = address;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}