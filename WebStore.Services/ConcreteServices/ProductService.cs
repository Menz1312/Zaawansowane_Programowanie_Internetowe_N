using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WebStore.DAL.EF;
using WebStore.Model; // Upewnij się, że masz tu poprawny using
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Services.ConcreteServices
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }
        
        public ProductVm AddOrUpdateProduct(AddOrUpdateProductVm addOrUpdateProductVm)
        {
            try
            {
                if (addOrUpdateProductVm == null)
                    throw new ArgumentNullException("View model parameter is null");
                
                var productEntity = Mapper.Map<Product>(addOrUpdateProductVm);
                
                if (addOrUpdateProductVm.Id.HasValue || addOrUpdateProductVm.Id == 0)
                    DbContext.Products.Update(productEntity);
                else
                    DbContext.Products.Add(productEntity);
                
                DbContext.SaveChanges();
                
                var productVm = Mapper.Map<ProductVm>(productEntity);
                return productVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public ProductVm GetProduct(Expression<Func<Product, bool>> filterExpression)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException("Filter expression parameter is null");
                
                var productEntity = DbContext.Products.FirstOrDefault(filterExpression);
                var productVm = Mapper.Map<ProductVm>(productEntity);
                return productVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<ProductVm> GetProducts(Expression<Func<Product, bool>>? filterExpression = null)
        {
            try
            {
                var productsQuery = DbContext.Products.AsQueryable();
                
                if (filterExpression != null)
                    productsQuery = productsQuery.Where(filterExpression);
                
                var productVms = Mapper.Map<IEnumerable<ProductVm>>(productsQuery);
                return productVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public bool DeleteProduct(Expression<Func<Product, bool>> filterExpression)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException("Filter expression parameter is null");
                var productEntity = DbContext.Products.FirstOrDefault(filterExpression);
                if (productEntity == null)
                {
                    return false;
                }
                DbContext.Products.Remove(productEntity);
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}