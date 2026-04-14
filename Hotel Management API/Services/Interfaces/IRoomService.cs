using Hotel_Management_API.DTOs;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResult<List<RoomResponse>>> GetAllAsync();
        Task<ServiceResult<RoomResponse>> GetByIdAsync(int id);
        Task<ServiceResult<RoomResponse>> CreateAsync(CreateRoomRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateRoomRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
