using Hotel_Management_API.Data;
using Hotel_Management_API.Models;
using Hotel_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Repositories.Implementations
{
    public class BookingRepository(AppDbContext _context) : IBookingRepository
    {
        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Hotel)
                .Include(b => b.Room)
                    .ThenInclude(r => r.RoomType)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task<bool> HasOverlappingBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.CheckedOut &&
                checkInDate < b.CheckOutDate &&
                checkOutDate > b.CheckInDate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

