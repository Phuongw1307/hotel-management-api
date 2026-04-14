using Hotel_Management_API.DTOs;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IRoomTypeService
    {
        Task<ServiceResult<List<RoomTypeResponse>>> GetAllAsync();
        Task<ServiceResult<RoomTypeResponse>> GetByIdAsync(int id);
        Task<ServiceResult<RoomTypeResponse>> CreateAsync(CreateRoomTypeRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateRoomTypeRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
