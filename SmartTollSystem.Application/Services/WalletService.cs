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
            if (amount <= 0)
            {
                return false; // Ensure the amount is valid
            }

            // Begin a transaction to ensure atomicity
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Fetch the user by userId (not the vehicle)
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                // Check if the user exists
                if (user == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false; // User not found
                }

                if (user.Balance == null)
                {
                    user.Balance = 0;
                }
                user.Balance += amount;

                await _unitOfWork.UserRepository.UpdateAsync(user); // Update the user's balance

                // Save the changes to the database
                await _unitOfWork.SaveAsync();

                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();
                return true; // Successfully topped up
            }
            catch
            {
                // Rollback in case of error
                await _unitOfWork.RollbackTransactionAsync();
                throw; // Rethrow the exception to be handled at a higher level
            }
        }

        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            return user?.Balance ?? 0;
        }
    }
}
