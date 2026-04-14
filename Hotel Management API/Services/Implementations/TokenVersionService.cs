using Hotel_Management_API.Data;
using Hotel_Management_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Services.Implementations
{
    namespace Hotel_Management_API.Services.Implementations
    {
        public class TokenVersionService : ITokenVersionService
        {
            private readonly AppDbContext _context;

            public TokenVersionService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<bool> IsTokenVersionValidAsync(int userId, int tokenVersion)
            {
                var user = await _context.AppUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                    return false;

                if (!user.IsActive)
                    return false;

                return user.TokenVersion == tokenVersion;
            }
        }
    }
}
