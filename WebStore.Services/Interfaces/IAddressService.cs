using WebStore.ViewModels.VM;

namespace WebStore.Services.Interfaces
{
    public interface IAddressService
    {
        // Pobiera jeden konkretny adres
        AddressVm GetAddress(int addressId);

        // Pobiera wszystkie adresy z "książki adresowej" klienta
        IEnumerable<AddressVm> GetCustomerAddresses(int customerId);

        // Dodaje nowy adres do "książki adresowej" klienta
        AddressVm AddAddressToCustomer(int customerId, AddOrUpdateAddressVm addAddressVm);

        // Aktualizuje istniejący adres
        AddressVm UpdateAddress(AddOrUpdateAddressVm updateAddressVm);

        // Usuwa adres (np. z książki adresowej)
        void DeleteAddress(int addressId);

        // Ustawia domyślny adres rozliczeniowy dla klienta
        void SetBillingAddress(int customerId, int addressId);

        // Ustawia domyślny adres wysyłkowy dla klienta
        void SetShippingAddress(int customerId, int addressId);
    }
}