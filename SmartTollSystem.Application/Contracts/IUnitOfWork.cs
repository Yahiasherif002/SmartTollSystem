using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
       IRepository<TollHistory> TollRepository { get; }
        IRepository<Vehicle> VehicleRepository { get; }
        IRepository<Radar> RadarRepository { get; }
        IRepository<Detection> DetectionRepository { get; }

        Task<int> SaveAsync();

    }
}
