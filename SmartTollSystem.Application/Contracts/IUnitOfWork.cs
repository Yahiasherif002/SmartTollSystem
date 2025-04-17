using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IVehicleRepository Vehicles { get; }
        ITollRepository Tolls { get; }
        Task<int> CompleteAsync();

    }
}
