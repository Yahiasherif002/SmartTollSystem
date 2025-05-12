using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartTollSystem.Domain.DTOs;
using SmartTollSystem.Domain.Entities.Identity;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class UserService : IUserService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {                                          
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<bool> AssignRoleAsync(Guid userId, string role)
        {
            var user = _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return Task.FromResult(false);
            var roleExists = _roleManager.RoleExistsAsync(role);
            if (!roleExists.Result) return Task.FromResult(false);
            var result = _userManager.AddToRoleAsync(user.Result, role);
            if (result.Result.Succeeded)
            {

                return Task.FromResult(true);
            }
            return Task.FromResult(false);

        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            ApplicationUser user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return false;
            await _unitOfWork.UserRepository.DeleteAsync(userId);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = "User",
                Balance = user.Balance ?? 0 
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = "User",
                Balance = user.Balance ?? 0 
            };
        }
        public async Task<UserDto?> UpdateUserEntityAsync(UserDto userDto)
        {
            var users = await _unitOfWork.UserRepository.FindAsync(u => u.Id == userDto.Id);
            var user = users.FirstOrDefault();

            if (user == null)
                return null;

            // Map updated values from DTO to entity
            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
           
            // Add any other properties as needed

            _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            // Return updated DTO if needed
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
               
                // Map others as necessary
            };
        }
        public async Task<List<UserDto>> GetAllUsersWithVehiclesAsync()
        {
            var users = await _unitOfWork.UserRepository.GetWithIncludeAsync(
                u => true, 
                u => u.Vehicles 
            );

            // Map the users and their vehicles to DTOs
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Vehicles = u.Vehicles.Select(v => new VehicleDto
                {
                    PlateNumber = v.LicensePlate,
                    Type = v.Type 
                }).ToList()
            }).ToList();
        }




    }
}
