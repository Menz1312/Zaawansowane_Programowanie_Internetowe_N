using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressApiController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressApiController> _logger;

        public AddressApiController(IAddressService addressService, ILogger<AddressApiController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        // GET: api/AddressApi/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var address = _addressService.GetAddress(id);
                return Ok(address);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/AddressApi/customer/10
        [HttpGet("customer/{customerId}")]
        public IActionResult GetForCustomer(int customerId)
        {
            try
            {
                var addresses = _addressService.GetCustomerAddresses(customerId);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting addresses");
                return BadRequest(ex.Message);
            }
        }

        // POST: api/AddressApi/customer/10
        [HttpPost("customer/{customerId}")]
        public IActionResult AddToCustomer(int customerId, [FromBody] AddOrUpdateAddressVm addressVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _addressService.AddAddressToCustomer(customerId, addressVm);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] AddOrUpdateAddressVm addressVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _addressService.UpdateAddress(addressVm);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _addressService.DeleteAddress(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}