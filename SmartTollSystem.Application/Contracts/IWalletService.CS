using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Contracts
{
    public interface IWalletService
    {
        Task<bool> TopUpBalanceAsync(Guid vehicleId, decimal amount);
        Task<decimal> GetBalanceAsync(Guid vehicleId);
    }
}
