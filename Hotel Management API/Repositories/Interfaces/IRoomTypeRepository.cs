using Hotel_Management_API.Models;

namespace Hotel_Management_API.Repositories.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<List<RoomType>> GetAllAsync();
        Task<RoomType?> GetByIdAsync(int id);
        Task AddAsync(RoomType roomType);
        void Update(RoomType roomType);
        void Delete(RoomType roomType);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasRoomsAsync(int roomTypeId);
        Task SaveChangesAsync();
    }
}
