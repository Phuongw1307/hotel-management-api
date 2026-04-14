using Hotel_Management_API.Data;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Room>> GetAllWithDetailsAsync()
        {
            return await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Rooms
                .AsNoTracking()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        public void Update(Room room)
        {
            _context.Rooms.Update(room);
        }

        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Rooms.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> RoomNumberExistsInHotelAsync(int hotelId, int roomNumber, int? excludeRoomId = null)
        {
            return await _context.Rooms.AnyAsync(r =>
                r.HotelId == hotelId &&
                r.RoomNumber == roomNumber &&
                (!excludeRoomId.HasValue || r.Id != excludeRoomId.Value));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
