using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SmartTollSystem.Application.Contracts;
using SmartTollSystem.Application.DTOs;
using SmartTollSystem.Domain.Entities.Identity;
using SmartTollSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartTollSystem.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IVehicleService _vehicleService;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,IConfiguration configuration, IVehicleService vehicleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _vehicleService = vehicleService;
        }


        public async Task<AuthResultDto> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {
            

            var user = new ApplicationUser
            {
                UserName = userRegisterDTO.Email,
                Email = userRegisterDTO.Email,
                PhoneNumber = userRegisterDTO.PhoneNumber,
                FullName = userRegisterDTO.FirstName + " " + userRegisterDTO.LastName
            };

            var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);
            if (result.Succeeded)
            {
                

                // Register vehicles if any are provided
                if (userRegisterDTO.Vehicles != null)
                {
                    foreach (var vehicleDto in userRegisterDTO.Vehicles)
                    {
                        // Set the OwnerId to the registered user's UserId
                        vehicleDto.OwnerId = user.Id;
                        // Register the vehicle
                        await _vehicleService.RegisterVehicleAsync(vehicleDto);
                    }
                }

                return new AuthResultDto(GenerateToken(user), true, "User registered successfully");
            }

            return new AuthResultDto(
                Token: string.Empty,
                Success: false,
                Message: string.Join(", ", result.Errors.Select(e => e.Description))
            );
        }



        public async Task<AuthResultDto> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user == null)
            {
                return new AuthResultDto(
                    Token: string.Empty,
                    Success: false,
                    Message: "Invalid email or password"
                );
            }

            var result = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);
            if (result)
            {
                return new AuthResultDto(
                    Token: GenerateToken(user),
                    Success: true,
                    Message: "User logged in successfully"
                );
            }

            return new AuthResultDto(
                Token: string.Empty,
                Success: false,
                Message: "Invalid email or password"
            );
        }
        private  string GenerateToken(ApplicationUser user)
        {
            DateTime Expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireTime"]));
            var roles =  _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Email,user.Email??string.Empty),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.FullName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: Expiration,
                signingCredentials: creds 
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenGenerator);


        }

        
        public Task<string> GetUserIdFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return Task.FromResult(userIdClaim?.Value);
        }
        public Task<string> GetRoleFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return Task.FromResult(roleClaim?.Value);
        }
        public Task<DateTime> GetExpirationDateFromTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expirationClaim != null)
            {
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value)).UtcDateTime;
                return Task.FromResult(expirationDate);
            }
            return Task.FromResult(DateTime.MinValue);
        }

        public Task<CurrentUserDto> GetCurrentUserAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult<CurrentUserDto>(null);

            // Remove 'bearer ' prefix if present
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return Task.FromResult<CurrentUserDto>(null);

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (userIdClaim != null && emailClaim != null && nameClaim != null && roleClaim != null && expirationClaim != null)
            {
                var email = emailClaim.Value;
                var mailAddress = new MailAddress(email);
                var username = mailAddress.User;
                return Task.FromResult(new CurrentUserDto
                {
                    Id = Guid.Parse(userIdClaim.Value),
                    Email = emailClaim.Value,
                    FullName = nameClaim.Value,
                    Role = roleClaim.Value,
                    ExpirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim.Value)).UtcDateTime,
                    Token = token,
                    UserName=username
                    
                });
            }

            return Task.FromResult<CurrentUserDto>(null);
        }

    }
}
