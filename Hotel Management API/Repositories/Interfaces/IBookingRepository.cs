using Hotel_Management_API.Models;

namespace Hotel_Management_API.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> GetByIdForUpdateAsync(int id);
        Task AddAsync(Booking booking);
        Task<bool> HasOverlappingBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
        Task SaveChangesAsync();
    }
}
