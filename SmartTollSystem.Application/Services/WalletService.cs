using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
     class WalletService:IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> TopUpBalanceAsync(Guid vehicleId, decimal amount)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null || amount <= 0) return false;
            vehicle.Balance += amount;
            await _unitOfWork.SaveAsync();
            return true;

        }
        public async Task<decimal> GetBalanceAsync(Guid vehicleId)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
            return vehicle?.Balance ?? 0;
        }
    }
}
