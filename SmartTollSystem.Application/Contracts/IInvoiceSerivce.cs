using SmartTollSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Contracts
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto);
        Task<bool> UpdateInvoiceAsync(Guid invoiceId, decimal amount);
        Task<bool> DeleteInvoiceAsync(Guid invoiceId);
        Task<InvoiceDto> GetInvoiceByIdAsync(Guid invoiceId);
        Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync();
        Task<IEnumerable<InvoiceDto>> GetInvoicesByPlateNumberAsync(string plateNumber);
    }


}
