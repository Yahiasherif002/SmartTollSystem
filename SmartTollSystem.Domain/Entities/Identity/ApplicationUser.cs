using Microsoft.AspNetCore.Identity;
using SmartTollSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "Full name is required")]
        public string? FullName { get; set; }

        public string? City { get; set; }

        public decimal? Balance { get; set; }


        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
