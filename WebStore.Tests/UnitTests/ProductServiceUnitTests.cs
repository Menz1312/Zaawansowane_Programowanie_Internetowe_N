using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.DAL.EF;
using WebStore.Model;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
using Xunit;
namespace WebStore.Tests.UnitTests {
    public class ProductServiceUnitTests : BaseUnitTests {
        private readonly IProductService _productService;
        public ProductServiceUnitTests (ApplicationDbContext dbContext,
        IProductService productService) : base (dbContext) {
            _productService = productService;
        }
        [Fact]
        public void GetProductTest () {
            var product = _productService.GetProduct (p => p.Name == "Monitor Dell 32");
            Assert.NotNull (product);
        }
        [Fact]
        public void GetMultipleProductsTest () {
            var products = _productService.GetProducts (p => p.Id >= 1 && p.Id <= 2);
            Assert.NotNull (products);
            Assert.NotEmpty (products);
            Assert.Equal (2, products.Count ());
        }
        [Fact]
        public void GetAllProductsTest () {
            var products = _productService.GetProducts ();
            Assert.NotNull (products);
            Assert.NotEmpty (products);
            Assert.Equal (products.Count (), products.Count ());
        }
        [Fact]
        public void AddNewProductTest () {
            var newProductVm = new AddOrUpdateProductVm () {
                Name = "MacBook Pro",
                CategoryId = 1,
                SupplierId = 1,
                ImageBytes = new byte[] {
                    0xff,
                    0xff,
                    0xff,
                    0x80
                },
                Price = 6000,
                Weight = 1.1f,
                Description = "MacBook Pro z procesorem M1 8GB RAM, Dysk 256 GB"
            };
            var createdProduct = _productService.AddOrUpdateProduct (newProductVm);
            Assert.NotNull (createdProduct);
            Assert.Equal ("MacBook Pro", createdProduct.Name);
        }
        [Fact]
        public void UpdateProductTest () {
            var updateProductVm = new AddOrUpdateProductVm () {
                Id = 1,
                Description = "Bardzo praktyczny monitor 32 cale",
                ImageBytes = new byte[] { 0xff, 0xff, 0xff, 0x80 },
                Name = "Monitor Dell 32",
                Price = 2000,
                Weight = 20,
                CategoryId = 1,
                SupplierId = 1
            };
            var editedProductVm = _productService.AddOrUpdateProduct (updateProductVm);
            Assert.NotNull (editedProductVm);
            Assert.Equal ("Monitor Dell 32", editedProductVm.Name);
            Assert.Equal (2000, editedProductVm.Price);
        }
    }
}