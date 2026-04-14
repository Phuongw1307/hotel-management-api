using Hotel_Management_API.Models;

namespace Hotel_Management_API.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<Room?> GetByIdWithDetailsAsync(int id);
        Task<List<Room>> GetAllWithDetailsAsync();
        Task AddAsync(Room room);
        void Update(Room room);
        void Delete(Room room);
        Task<bool> ExistsAsync(int id);
        Task<bool> RoomNumberExistsInHotelAsync(int hotelId, int roomNumber, int? excludeRoomId = null);
        Task SaveChangesAsync();
    }
}
