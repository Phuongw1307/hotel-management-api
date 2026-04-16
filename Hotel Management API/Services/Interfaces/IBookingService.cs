using Hotel_Management_API.DTOs;

namespace Hotel_Management_API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ServiceResult<List<CreateBookingResponse>>> GetAllAsync();
        Task<ServiceResult<CreateBookingResponse>> GetByIdAsync(int id);
        Task<ServiceResult<CreateBookingResponse>> CreateAsync(int appUserId, CreateBookingRequest request);
        Task<ServiceResult<List<AvailableRoomResponse>>> GetAvailableRoomsAsync(
            int hotelId,
            DateTime checkInDate,
            DateTime checkOutDate);
    }
}
