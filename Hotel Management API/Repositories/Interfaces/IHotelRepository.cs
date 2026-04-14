using Hotel_Management_API.Models;

namespace Hotel_Management_API.Repositories.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllAsync();
        Task<Hotel?> GetByIdAsync(int id);
        Task AddAsync(Hotel hotel);
        void Update(Hotel hotel);
        void Delete(Hotel hotel);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasRoomsAsync(int hotelId);
        Task SaveChangesAsync();
    }
}
