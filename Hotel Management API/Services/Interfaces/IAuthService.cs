using Hotel_Management_API.DTOs;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterRequest request);
        Task<AuthResponseDto?> LoginAsync(LoginRequest request);
        Task<AuthResponseDto?> RefreshAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<bool> LogoutAllAsync(int userId);
    }
}
