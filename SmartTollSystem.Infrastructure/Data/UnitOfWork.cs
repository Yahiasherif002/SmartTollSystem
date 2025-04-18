using Microsoft.EntityFrameworkCore.Storage;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Entities.Identity;
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
        private IDbContextTransaction _currentTransaction;


        private IRepository<Vehicle>? _vehicleRepository;
        private IRepository<Radar>? _radarleRepository;
        private IRepository<TollHistory>? _tollRepository;
        private IRepository<Detection>? _detectionRepository;
        private IRepository<ApplicationUser>? _userRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            
        }

        public IRepository<Vehicle> VehicleRepository => _vehicleRepository ??= new Repository<Vehicle>(_context);
        public IRepository<Radar> RadarRepository => _radarleRepository ??= new Repository<Radar>(_context);
        public IRepository<TollHistory> TollRepository => _tollRepository ??= new Repository<TollHistory>(_context);

        public IRepository<Detection> DetectionRepository => _detectionRepository ??= new Repository<Detection>(_context);
        public IRepository<ApplicationUser> UserRepository => _userRepository ??= new Repository<ApplicationUser>(_context);

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();

        }

        public async Task CommitTransactionAsync()
        {
            if(_currentTransaction != null)
    {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
