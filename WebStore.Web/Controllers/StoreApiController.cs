using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreApiController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreApiController> _logger;

        public StoreApiController(IStoreService storeService, ILogger<StoreApiController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var store = await _storeService.GetStoreByIdAsync(id);
                return Ok(store);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddOrUpdateStoreVm storeVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _storeService.CreateStoreAsync(storeVm);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddOrUpdateStoreVm storeVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != storeVm.Id) return BadRequest("Id mismatch");

            try
            {
                var result = await _storeService.UpdateStoreAsync(storeVm);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _storeService.DeleteStoreAsync(id);
            return NoContent();
        }
    }
}