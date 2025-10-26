using AutoMapper;
using WebStore.Model; // Lub WebStore.Model.DataModels
using WebStore.ViewModels.VM;

namespace WebStore.Services.Configuration.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<AddOrUpdateProductVm, Product>();
            CreateMap<Product, ProductVm>();

            CreateMap<Address, AddressVm>();
            CreateMap<AddOrUpdateAddressVm, Address>();
            CreateMap<Address, AddOrUpdateAddressVm>();

            // StoreService
            CreateMap<StationaryStore, StoreVm>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.StationaryStoreEmployees));
            CreateMap<AddOrUpdateStoreVm, StationaryStore>();
            CreateMap<StationaryStoreEmployee, EmployeeVm>()
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StationaryStoreId));
            CreateMap<AddEmployeeVm, StationaryStoreEmployee>();

            // OrderService
            CreateMap<Order, OrderVm>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderProducts));
            CreateMap<OrderProduct, OrderItemVm>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
            
            // InvoiceService
            CreateMap<Invoice, InvoiceVm>()
                .ForMember(dest => dest.OrderIds, opt => opt.MapFrom(src => src.Orders.Select(o => o.Id)));
        }
    }
}