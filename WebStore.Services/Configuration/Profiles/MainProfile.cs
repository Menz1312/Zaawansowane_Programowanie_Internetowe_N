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
        }
    }
}