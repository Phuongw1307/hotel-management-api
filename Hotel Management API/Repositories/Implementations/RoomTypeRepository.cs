using Hotel_Management_API.Data;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Repositories.Implementations
{
    public class RoomTypeRepository(AppDbContext _context) : IRoomTypeRepository
    {
        public async Task<List<RoomType>> GetAllAsync()
        {
            return await _context.RoomTypes
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RoomType?> GetByIdAsync(int id)
        {
            return await _context.RoomTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task AddAsync(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
        }

        public void Update(RoomType roomType)
        {
            _context.RoomTypes.Update(roomType);
        }

        public void Delete(RoomType roomType)
        {
            _context.RoomTypes.Remove(roomType);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RoomTypes.AnyAsync(rt => rt.Id == id);
        }

        public async Task<bool> HasRoomsAsync(int roomTypeId)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomTypeId == roomTypeId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
