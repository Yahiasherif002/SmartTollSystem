using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
   public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVehicleService _vehicleService;
        public WalletService(IUnitOfWork unitOfWork, IVehicleService vehicleService)
        {
            _unitOfWork = unitOfWork;
            _vehicleService = vehicleService;
        }
        public async Task<bool> TopUpBalanceAsync(Guid userId, decimal amount)
        {
            var vehicleId = (await _vehicleService.GetVehicleByLoggedInUserAsync(userId)).VehicleId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null || amount <= 0)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                vehicle.Balance += amount;
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync(); 
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(); 
                throw; 
            }
        }
        public async Task<decimal> GetBalanceAsync(Guid vehicleId)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(vehicleId);
            return vehicle?.Balance ?? 0;
        }
    }
}
