using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;
namespace WebStore.Web.Controllers;
public class ProductApiController : BaseApiController
{
    private readonly IProductService _productService;
    public ProductApiController(ILogger logger, IMapper mapper,
    IProductService productService)
    : base(logger, mapper)
    {
        _productService = productService;
    }
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            return StatusCode(500, "Error occured");
        }
    }
    [HttpGet("{id:int:min(1)}")]
    public IActionResult Get(int id)
    {
        try
        {
            var product = _productService.GetProduct(p => p.Id == id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            return StatusCode(500, "Error occured");
        }
    }
    [HttpPut]
    public IActionResult Put([FromBody] AddOrUpdateProductVm addOrUpdateProductVm)
    {
        return PostOrPutHelper(addOrUpdateProductVm);
    }
    [HttpPost]
    public IActionResult Post([FromBody] AddOrUpdateProductVm addOrUpdateProductVm)
    {
        return PostOrPutHelper(addOrUpdateProductVm);
    }
    [HttpDelete("{id:int:min(1)}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var result = _productService.DeleteProduct(p => p.Id == id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            return StatusCode(500, "Error occured");
        }
    }
    private IActionResult PostOrPutHelper(AddOrUpdateProductVm addOrUpdateProductVm)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_productService.AddOrUpdateProduct(addOrUpdateProductVm));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message, ex);
            return StatusCode(500, "Error occured");
        }
    }
}