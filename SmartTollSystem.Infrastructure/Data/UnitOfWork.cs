using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Interfaces;
using SmartTollSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IRepository<Vehicle>? _vehicleRepository;
        private IRepository<Radar>? _radarleRepository;
        private IRepository<TollHistory>? _tollRepository;
        private IRepository<Detection>? _detectionRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Vehicle> VehicleRepository => _vehicleRepository ??= new Repository<Vehicle>(_context);
        public IRepository<Radar> RadarRepository => _radarleRepository ??= new Repository<Radar>(_context);
        public IRepository<TollHistory> TollRepository => _tollRepository ??= new Repository<TollHistory>(_context);

        public IRepository<Detection> DetectionRepository => _detectionRepository ??= new Repository<Detection>(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
