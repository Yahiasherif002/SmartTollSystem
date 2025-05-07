using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto invoiceDto)
        {
            var invoice = new Invoice
            {
                VehicleId = invoiceDto.VehicleId,
                PlateNumber = invoiceDto.PlateNumber,
                Amount = invoiceDto.Amount,
                Location = invoiceDto.Location,
                IsPaid = invoiceDto.IsPaid,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.InvoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveAsync();

            invoiceDto.InvoiceId = invoice.InvoiceId;
            invoiceDto.CreatedAt = invoice.CreatedAt;
            return invoiceDto;
        }

        public async Task<bool> DeleteInvoiceAsync(Guid invoiceId)
        {
            await _unitOfWork.InvoiceRepository.DeleteAsync(invoiceId);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync()
        {
            var invoices = await _unitOfWork.InvoiceRepository.GetAllAsync();
            return invoices.Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                VehicleId = i.VehicleId,
                PlateNumber = i.PlateNumber,
                Amount = i.Amount,
                Location = i.Location,
                IsPaid = i.IsPaid,
                CreatedAt = i.CreatedAt
            });
        }

        public async Task<InvoiceDto> GetInvoiceByIdAsync(Guid invoiceId)
        {
            var i = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);
            if (i == null) return null;

            return new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                VehicleId = i.VehicleId,
                PlateNumber = i.PlateNumber,
                Amount = i.Amount,
                Location = i.Location,
                IsPaid = i.IsPaid,
                CreatedAt = i.CreatedAt
            };
        }

        public async Task<IEnumerable<InvoiceDto>> GetInvoicesByPlateNumberAsync(string plateNumber)
        {
            var invoices = await _unitOfWork.InvoiceRepository
                .FindAsync(i => i.PlateNumber == plateNumber);

            return invoices.Select(i => new InvoiceDto
            {
                InvoiceId = i.InvoiceId,
                VehicleId = i.VehicleId,
                PlateNumber = i.PlateNumber,
                Amount = i.Amount,
                Location = i.Location,
                IsPaid = i.IsPaid,
                CreatedAt = i.CreatedAt
            });
        }

        public async Task<bool> UpdateInvoiceAsync(Guid invoiceId, decimal amount)
        {
            var invoice = await _unitOfWork.InvoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null) return false;

            invoice.Amount = amount;
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
