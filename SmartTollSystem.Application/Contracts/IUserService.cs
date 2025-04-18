using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> AssignRoleAsync(Guid userId, string role);
        Task<UserDto?> UpdateUserEntityAsync(UserDto userDto);
        Task<List<UserDto>> GetAllUsersWithVehiclesAsync();

    }
}
