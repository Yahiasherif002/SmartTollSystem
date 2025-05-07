using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;

namespace SmartTollSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        /// <summary>
        /// Get an invoice by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }
        /// <summary>
        /// Get all invoices for a specific vehicle by its plate number.
        /// </summary>
        /// <param name="plateNumber"></param>
        /// <returns></returns>
        [HttpGet("plate/{plateNumber}")]
        public async Task<IActionResult> GetInvoicesByPlateNumber(string plateNumber)
        {
            var invoices = await _invoiceService.GetInvoicesByPlateNumberAsync(plateNumber);
            if (invoices == null || !invoices.Any())
            {
                return NotFound();
            }
            return Ok(invoices);
        }
        /// <summary>
        /// Get all invoices.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();
            return Ok(invoices);
        }
        /// <summary>
        /// Create a new invoice.
        /// </summary>
        /// <param name="invoiceDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
            {
                return BadRequest("Invalid invoice data.");
            }
            var createdInvoice = await _invoiceService.CreateInvoiceAsync(invoiceDto);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = createdInvoice.InvoiceId }, createdInvoice);
        }
        /// <summary>
        /// Update an existing invoice.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="invoiceDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null || id != invoiceDto.InvoiceId)
            {
                return BadRequest("Invalid invoice data.");
            }
            var result = await _invoiceService.UpdateInvoiceAsync(id, invoiceDto.Amount);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        /// <summary>
        /// Delete an invoice by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var result = await _invoiceService.DeleteInvoiceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
