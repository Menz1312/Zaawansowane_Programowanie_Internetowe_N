using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels.VM;

namespace WebStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceApiController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly ILogger<InvoiceApiController> _logger;

        public InvoiceApiController(IInvoiceService invoiceService, ILogger<InvoiceApiController> logger)
        {
            _invoiceService = invoiceService;
            _logger = logger;
        }

        // GET: api/InvoiceApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
                return Ok(invoice);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Invoice not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching invoice");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/InvoiceApi/generate
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateInvoiceVm invoiceVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _invoiceService.GenerateInvoiceForOrdersAsync(invoiceVm);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Np. zamówienia z różnych klientów
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating invoice");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}