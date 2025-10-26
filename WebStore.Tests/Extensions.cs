using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.EF;
using WebStore.Model;
namespace WebStore.Tests {
    public static class Extensions {
        // Create sample data
        public static void SeedData (this IServiceCollection services) {
            var serviceProvider = services.BuildServiceProvider ();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext> ();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>> ();
            var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole<int>>>();
            // other seed data ...
            //Customers
            var customer1 = new Customer()
            {
                Id = 2, // Użyj innego Id niż Supplier (który ma Id = 1)
                FirstName = "Jan",
                LastName = "Kowalski",
                UserName = "customer1@eg.eg",
                Email = "customer1@eg.eg",
                RegistrationDate = new DateTime(2023, 1, 1),
            };
            userManager.CreateAsync(customer1, "User1234").GetAwaiter().GetResult();
            //Suppliers
            var supplier1 = new Supplier () {
                Id = 1,
                FirstName = "Adam",
                LastName = "Bednarski",
                UserName = "supp1@eg.eg",
                Email = "supp1@eg.eg",
                RegistrationDate = new DateTime (2010, 1, 1),
            };
            userManager.CreateAsync (supplier1, "User1234").GetAwaiter().GetResult();
            //Categories
            var category1 = new Category () {
                Id = 1,
                Name = "Computers",
                Tag = "#computer"
            };
            dbContext.Add (category1);
            //Products
            var p1 = new Product () {
                Id = 1,
                Category = category1,
                Supplier = supplier1,
                Description = "Bardzo praktyczny monitor 32 cale",
                ImageBytes = new byte[] { 0xff, 0xff, 0xff, 0x80 },
                Name = "Monitor Dell 32",
                Price = 2000,
                Weight = 20,
            };
            dbContext.Add (p1);
            var p2 = new Product () {
                Id = 2,
                Category = category1,
                Supplier = supplier1,
                Description = "Precyzyjna mysz do pracy",
                ImageBytes = new byte[] { 0xff, 0xff, 0xff, 0x70 },
                Name = "Mysz Logitech",
                Price = 500,
                Weight = 0.5f,
            };
            dbContext.Add (p2);
            // save changes
            dbContext.SaveChanges ();
        }
    }
}