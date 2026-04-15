using Hotel_Management_API.Data;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Repositories.Implementations
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Hotel>> GetAllAsync()
        {
            return await _context.Hotels
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Hotel?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Hotels
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
        }

        public void Delete(Hotel hotel)
        {
            _context.Hotels.Remove(hotel);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Hotels.AnyAsync(h => h.Id == id);
        }

        public async Task<bool> HasRoomsAsync(int hotelId)
        {
            return await _context.Rooms.AnyAsync(r => r.HotelId == hotelId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}