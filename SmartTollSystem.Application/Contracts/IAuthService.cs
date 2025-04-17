using SmartTollSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Contracts
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(UserRegisterDTO userRegisterDTO);

        Task<AuthResultDto> LoginAsync(UserLoginDTO userLoginDTO);
      //  Task<string> RefreshTokenAsync(string token, string refreshToken);
        Task LogoutAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
       // Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        Task<string> GetUserIdFromTokenAsync(string token);
        Task<string> GetRoleFromTokenAsync(string token);
        Task<DateTime> GetExpirationDateFromTokenAsync(string token);
    }
}
