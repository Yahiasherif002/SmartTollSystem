using SmartTollSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserDto userDto);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
        string GetRoleFromToken(string token);
        DateTime GetExpirationDateFromToken(string token);
        void InvalidateToken(string token);
        void InvalidateRefreshToken(string refreshToken);
    }
}
