using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderApiController> _logger;

        public OrderApiController(IOrderService orderService, ILogger<OrderApiController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        // GET: api/OrderApi/customer/5
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersForCustomer(int customerId)
        {
            try
            {
                var orders = await _orderService.GetOrdersForCustomerAsync(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for customer");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/OrderApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Order not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/OrderApi
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderVm orderVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _orderService.CreateOrderAsync(orderVm);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Np. nie znaleziono klienta lub produktu
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}