using Microsoft.AspNetCore.Http.HttpResults;
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
    internal class UserService : IUserService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                Role = "User"
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
                Role = "User"
            };
        }
    }
}
