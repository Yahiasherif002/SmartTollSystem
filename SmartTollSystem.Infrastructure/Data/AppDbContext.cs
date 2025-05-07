using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartTollSystem.Domain.Entities;
using SmartTollSystem.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<TollHistory> TollHistories { get; set; }
        public DbSet<Radar> Radars { get; set; }
        public DbSet<Detection> Detections { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Add customizations after calling base.OnModelCreating(builder);
        }
    }
}
