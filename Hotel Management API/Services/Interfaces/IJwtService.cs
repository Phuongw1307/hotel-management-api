using Hotel_Management_API.Models;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user);
    }
}
