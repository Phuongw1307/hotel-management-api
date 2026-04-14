using Hotel_Management_API.Data;
using Hotel_Management_API.DTOs;
using Hotel_Management_API.Models;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Hotel_Management_API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequest request)
        {
            var normalizedUsername = request.Username.Trim();

            var existedUser = await _context.AppUsers
                .AnyAsync(x => x.Username == normalizedUsername);

            if (existedUser)
                return null;

            var user = new AppUser
            {
                Username = normalizedUsername,
                FullName = request.FullName.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // Không lưu password thô
                Role = "User",
                IsActive = true,
                TokenVersion = 0
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            // Register xong thì cấp token luôn cho tiện
            return await CreateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequest request)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(x => x.Username == request.Username);

            if (user == null || !user.IsActive)
                return null;

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!passwordValid)
                return null;

            return await CreateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
        {
            var tokenInDb = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenInDb == null)
                return null;

            if (tokenInDb.IsRevoked)
                return null;

            if (tokenInDb.ExpiryDate <= DateTime.UtcNow)
                return null;

            if (!tokenInDb.User.IsActive)
                return null;

            // Rotate refresh token: token cũ bị vô hiệu, token mới được cấp
            tokenInDb.IsRevoked = true;

            var user = tokenInDb.User;
            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return BuildAuthResponse(user, newAccessToken, newRefreshToken);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var tokenInDb = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenInDb == null)
                return false;

            // Logout 1 thiết bị: revoke refresh token hiện tại
            if (!tokenInDb.IsRevoked)
            {
                tokenInDb.IsRevoked = true;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> LogoutAllAsync(int userId)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return false;

            // Tăng version để toàn bộ access token cũ bị vô hiệu
            user.TokenVersion++;

            var activeRefreshTokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            foreach (var token in activeRefreshTokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<AuthResponseDto> CreateAuthResponseAsync(AppUser user)
        {
            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return BuildAuthResponse(user, accessToken, refreshToken);
        }

        private AuthResponseDto BuildAuthResponse(AppUser user, string accessToken, string refreshToken)
        {
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(expireMinutes),
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
